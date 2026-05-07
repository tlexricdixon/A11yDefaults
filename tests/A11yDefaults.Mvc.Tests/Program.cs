using A11yDefaults.Mvc;
using A11yDefaults.Mvc.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

await SmokeTests.RunAsync();

internal static class SmokeTests
{
    public static async Task RunAsync()
    {
        await AddsNoopenerAndHintToBlankTargets();
        await HonorsScreenReaderOnlyHints();
        AppliesMinimumTargetClassToButtons();
        AppliesEnhancedTargetClassToInputs();
    }

    private static async Task AddsNoopenerAndHintToBlankTargets()
    {
        var helper = new A11yAnchorTagHelper(Options.Create(new A11yDefaultsOptions()))
        {
            Label = "WCAG target size"
        };

        var output = CreateOutput(
            "a",
            "WCAG target size",
            new TagHelperAttribute("href", "https://www.w3.org/WAI/WCAG22/Understanding/target-size-minimum.html"),
            new TagHelperAttribute("target", "_blank"),
            new TagHelperAttribute("rel", "nofollow"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertEqual("nofollow noopener", output.Attributes["rel"]?.Value?.ToString(), "blank targets should include noopener");
        AssertEqual(
            "WCAG target size (opens in new tab)",
            output.Attributes["aria-label"]?.Value?.ToString(),
            "aria-label should include the new-tab hint");
        AssertContains(output.Content.GetContent(), "opens in new tab", "visible hint should be appended to content");
    }

    private static async Task HonorsScreenReaderOnlyHints()
    {
        var helper = new A11yAnchorTagHelper(Options.Create(new A11yDefaultsOptions()))
        {
            ScreenReaderOnlyHint = true
        };

        var output = CreateOutput(
            "a",
            "MDN anchor reference",
            new TagHelperAttribute("href", "https://developer.mozilla.org/docs/Web/HTML/Reference/Elements/a"),
            new TagHelperAttribute("target", "_blank"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertContains(output.Content.GetContent(), "a11y-sr-only", "sr-only hint class should be rendered");
    }

    private static void AppliesMinimumTargetClassToButtons()
    {
        var helper = new A11yButtonTagHelper(Options.Create(new A11yDefaultsOptions()));
        var output = CreateOutput("button", null);

        helper.Process(CreateContext(), output);

        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-target-min", "buttons should get the minimum target class");
    }

    private static void AppliesEnhancedTargetClassToInputs()
    {
        var helper = new A11yInputTagHelper(Options.Create(new A11yDefaultsOptions()))
        {
            HitTarget = A11yTargetSize.Enhanced
        };

        var output = CreateOutput(
            "input",
            null,
            new TagHelperAttribute("type", "submit"),
            new TagHelperAttribute("value", "Publish"));

        helper.Process(CreateContext(), output);

        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-target-enhanced", "input buttons should honor explicit enhanced target size");
    }

    private static TagHelperContext CreateContext()
    {
        return new TagHelperContext(
            allAttributes: [],
            items: new Dictionary<object, object?>(),
            uniqueId: Guid.NewGuid().ToString("N"));
    }

    private static TagHelperOutput CreateOutput(string tagName, string? childHtml, params TagHelperAttribute[] attributes)
    {
        var output = new TagHelperOutput(
            tagName,
            new TagHelperAttributeList(attributes),
            (_, _) =>
            {
                TagHelperContent content = new DefaultTagHelperContent();
                if (!string.IsNullOrWhiteSpace(childHtml))
                {
                    content.SetHtmlContent(new HtmlString(childHtml));
                }

                return Task.FromResult(content);
            });

        if (!string.IsNullOrWhiteSpace(childHtml))
        {
            output.Content.SetHtmlContent(new HtmlString(childHtml));
        }

        return output;
    }

    private static void AssertContains(string? actual, string expected, string message)
    {
        if (actual is null || !actual.Contains(expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"{message}. Expected to find '{expected}' in '{actual}'.");
        }
    }

    private static void AssertEqual(string expected, string? actual, string message)
    {
        if (!string.Equals(expected, actual, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"{message}. Expected '{expected}' but got '{actual}'.");
        }
    }
}
