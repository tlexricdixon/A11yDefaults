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
        await RendersDefaultSkipLink();
        await HonorsSkipLinkOverrides();
        await PreservesSkipLinkHrefAndContent();
        AppliesFocusRingClassToCustomElements();
        AppliesMinimumTargetClassToButtons();
        AppliesEnhancedTargetClassToInputs();
        await AppliesFocusRingClassToAnchors();
        await CanDisableAutomaticFocusRingClasses();
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

    private static async Task RendersDefaultSkipLink()
    {
        var helper = new A11ySkipLinkTagHelper(Options.Create(new A11yDefaultsOptions()));
        var output = CreateOutput("a", null, new TagHelperAttribute("a11y-skip-link"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertEqual("#main", output.Attributes["href"]?.Value?.ToString(), "skip links should default to the main landmark");
        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-skip-link", "skip links should get the skip-link class");
        AssertEqual("Skip to main content", output.Content.GetContent(), "skip links should render default text when empty");
    }

    private static async Task HonorsSkipLinkOverrides()
    {
        var helper = new A11ySkipLinkTagHelper(Options.Create(new A11yDefaultsOptions()))
        {
            Target = "content",
            Text = "Jump to content"
        };

        var output = CreateOutput(
            "a",
            null,
            new TagHelperAttribute("a11y-skip-link"),
            new TagHelperAttribute("class", "custom-skip"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertEqual("#content", output.Attributes["href"]?.Value?.ToString(), "skip target overrides should be normalized");
        AssertContains(output.Attributes["class"]?.Value?.ToString(), "custom-skip", "skip links should preserve author classes");
        AssertEqual("Jump to content", output.Content.GetContent(), "skip text overrides should render when empty");
    }

    private static async Task PreservesSkipLinkHrefAndContent()
    {
        var helper = new A11ySkipLinkTagHelper(Options.Create(new A11yDefaultsOptions()));
        var output = CreateOutput(
            "a",
            "Skip navigation",
            new TagHelperAttribute("a11y-skip-link"),
            new TagHelperAttribute("href", "#content"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertEqual("#content", output.Attributes["href"]?.Value?.ToString(), "skip links should preserve explicit href values");
        AssertEqual("Skip navigation", output.Content.GetContent(), "skip links should preserve author-provided child content");
    }

    private static void AppliesMinimumTargetClassToButtons()
    {
        var helper = new A11yButtonTagHelper(Options.Create(new A11yDefaultsOptions()));
        var output = CreateOutput("button", null);

        helper.Process(CreateContext(), output);

        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-target-min", "buttons should get the minimum target class");
        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-focus-ring", "buttons should get the focus-ring class");
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
        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-focus-ring", "input buttons should get the focus-ring class");
    }

    private static async Task AppliesFocusRingClassToAnchors()
    {
        var helper = new A11yAnchorTagHelper(Options.Create(new A11yDefaultsOptions()));
        var output = CreateOutput(
            "a",
            "Agency services",
            new TagHelperAttribute("href", "/services"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-focus-ring", "anchors should get the focus-ring class");
    }

    private static void AppliesFocusRingClassToCustomElements()
    {
        var helper = new A11yFocusRingTagHelper(Options.Create(new A11yDefaultsOptions()));
        var output = CreateOutput(
            "span",
            "Custom control",
            new TagHelperAttribute("a11y-focus-ring"),
            new TagHelperAttribute("class", "custom-control"));

        helper.Process(CreateContext(), output);

        AssertContains(output.Attributes["class"]?.Value?.ToString(), "custom-control", "focus-ring helper should preserve author classes");
        AssertContains(output.Attributes["class"]?.Value?.ToString(), "a11y-focus-ring", "focus-ring helper should add the configured class");
        AssertFalse(output.Attributes.ContainsName("a11y-focus-ring"), "focus-ring helper should remove authoring-only attributes");
    }

    private static async Task CanDisableAutomaticFocusRingClasses()
    {
        var helper = new A11yAnchorTagHelper(Options.Create(new A11yDefaultsOptions
        {
            AddFocusRingToInteractiveElements = false
        }));

        var output = CreateOutput(
            "a",
            "Agency services",
            new TagHelperAttribute("href", "/services"));

        await helper.ProcessAsync(CreateContext(), output);

        AssertNotContains(output.Attributes["class"]?.Value?.ToString(), "a11y-focus-ring", "automatic focus-ring classes should be configurable");
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

    private static void AssertFalse(bool actual, string message)
    {
        if (actual)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void AssertNotContains(string? actual, string expected, string message)
    {
        if (actual is not null && actual.Contains(expected, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"{message}. Expected not to find '{expected}' in '{actual}'.");
        }
    }
}
