// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using A11yDefaults.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

internal static class OverlengthTagHelperSmokeTests
{
    public static void Run()
    {
        RemovesGeneratedInputMaxlengthWithoutLosingValidationMetadata();
        RemovesGeneratedTextareaMaxlength();
        PreservesExplicitAuthorMaxlength();
        OptOutDoesNotChangeTheElement();
        UnsupportedInputTypesAreUntouched();
        OnlyOptedInElementsAreTargeted();
    }

    private static void RemovesGeneratedInputMaxlengthWithoutLosingValidationMetadata()
    {
        var (context, output) = Create(
            "input",
            new TagHelperAttribute("asp-for", "ContactName"),
            new TagHelperAttribute("a11y-allow-overlength", "true"),
            new TagHelperAttribute("id", "ContactName"),
            new TagHelperAttribute("name", "ContactName"),
            new TagHelperAttribute("type", "text"),
            new TagHelperAttribute("value", "Example"),
            new TagHelperAttribute("class", "form-control"),
            new TagHelperAttribute("aria-describedby", "ContactName-error"),
            new TagHelperAttribute("data-val", "true"),
            new TagHelperAttribute("data-val-required", "Contact name is required."),
            new TagHelperAttribute("data-val-length-max", "255"),
            new TagHelperAttribute("maxlength", "255"),
            new TagHelperAttribute("data-val-length", "Cannot exceed 255 characters."));

        // Simulate the MVC Input Tag Helper having already produced its output.
        // maxlength is generated, not present in the authored context.
        var helper = new A11yAllowOverlengthTagHelper { AllowOverlength = true };
        Assert(helper.Order > 0, "The helper must execute after MVC's Input Tag Helper.");
        helper.Process(context, output);

        Assert(!output.Attributes.ContainsName("maxlength"), "Generated maxlength must be removed.");
        Assert(!output.Attributes.ContainsName("a11y-allow-overlength"), "Authoring attribute must not render.");
        Check(output, "id", "ContactName");
        Check(output, "name", "ContactName");
        Check(output, "value", "Example");
        Check(output, "class", "form-control");
        Check(output, "aria-describedby", "ContactName-error");
        Check(output, "data-val", "true");
        Check(output, "data-val-required", "Contact name is required.");
        Check(output, "data-val-length-max", "255");
        Check(output, "data-val-length", "Cannot exceed 255 characters.");
    }

    private static void RemovesGeneratedTextareaMaxlength()
    {
        var (context, output) = Create(
            "textarea",
            new TagHelperAttribute("asp-for", "Explanation"),
            new TagHelperAttribute("a11y-allow-overlength", "true"),
            new TagHelperAttribute("data-val-length-max", "2000"),
            new TagHelperAttribute("maxlength", "2000"));

        new A11yAllowOverlengthTagHelper { AllowOverlength = true }.Process(context, output);

        Assert(!output.Attributes.ContainsName("maxlength"), "Textarea must allow overlength input.");
        Check(output, "data-val-length-max", "2000");
    }

    private static void PreservesExplicitAuthorMaxlength()
    {
        var (context, output) = CreateWithExplicitMaxlength(
            "input",
            new TagHelperAttribute("asp-for", "Code"),
            new TagHelperAttribute("a11y-allow-overlength", "true"),
            new TagHelperAttribute("maxlength", "12"));

        new A11yAllowOverlengthTagHelper { AllowOverlength = true }.Process(context, output);

        Check(output, "maxlength", "12");
    }

    private static void OptOutDoesNotChangeTheElement()
    {
        var (context, output) = Create(
            "input",
            new TagHelperAttribute("asp-for", "Name"),
            new TagHelperAttribute("a11y-allow-overlength", "false"),
            new TagHelperAttribute("maxlength", "30"));

        new A11yAllowOverlengthTagHelper { AllowOverlength = false }.Process(context, output);

        Check(output, "maxlength", "30");
        Assert(!output.Attributes.ContainsName("a11y-allow-overlength"), "Authoring attribute must not render.");
    }

    private static void UnsupportedInputTypesAreUntouched()
    {
        foreach (var type in new[] { "number", "radio", "checkbox", "submit", "hidden" })
        {
            var (context, output) = Create(
                "input",
                new TagHelperAttribute("asp-for", "Choice"),
                new TagHelperAttribute("a11y-allow-overlength", "true"),
                new TagHelperAttribute("type", type),
                new TagHelperAttribute("maxlength", "5"));

            new A11yAllowOverlengthTagHelper { AllowOverlength = true }.Process(context, output);
            Check(output, "maxlength", "5");
        }
    }

    private static void OnlyOptedInElementsAreTargeted()
    {
        var declarations = typeof(A11yAllowOverlengthTagHelper)
            .GetCustomAttributes(typeof(HtmlTargetElementAttribute), false)
            .Cast<HtmlTargetElementAttribute>()
            .ToArray();

        Assert(declarations.Length == 2, "Only input and textarea should be targeted.");
        Assert(declarations.All(d => d.Attributes == "asp-for,a11y-allow-overlength"),
            "Both asp-for and explicit opt-in must be required.");
    }

    private static (TagHelperContext Context, TagHelperOutput Output) Create(
        string tagName, params TagHelperAttribute[] attributes)
    {
        // Author-supplied attributes are kept separate from MVC-generated output.
        var authored = attributes
            .Where(a => !string.Equals(a.Name, "data-val", StringComparison.OrdinalIgnoreCase)
                     && !a.Name.StartsWith("data-val-", StringComparison.OrdinalIgnoreCase)
                     && !string.Equals(a.Name, "maxlength", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        var context = new TagHelperContext(
            new TagHelperAttributeList(authored),
            new Dictionary<object, object?>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            tagName,
            new TagHelperAttributeList(attributes),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        return (context, output);
    }

    private static (TagHelperContext Context, TagHelperOutput Output) CreateWithExplicitMaxlength(
        string tagName, params TagHelperAttribute[] attributes)
    {
        var (_, output) = Create(tagName, attributes);
        var context = new TagHelperContext(
            new TagHelperAttributeList(attributes),
            new Dictionary<object, object?>(),
            Guid.NewGuid().ToString("N"));
        return (context, output);
    }

    private static void Check(TagHelperOutput output, string name, string value)
    {
        Assert(output.Attributes[name]?.Value?.ToString() == value,
            $"Expected {name}={value} to be preserved.");
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
