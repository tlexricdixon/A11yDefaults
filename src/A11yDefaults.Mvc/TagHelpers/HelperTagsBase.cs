// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace A11yDefaults.Mvc.TagHelpers;

/// <summary>
/// Base class for A11yDefaults tag helpers providing shared dependency injection,
/// constants, and common utility methods.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HelperTagsBase"/> class.
/// </remarks>
/// <param name="options">The configured package defaults.</param>
public abstract class HelperTagsBase(IOptions<A11yDefaultsOptions> options) : TagHelper
{
    /// <summary>
    /// Package-wide defaults used when a tag element does not override behavior
    /// with an <c>a11y-*</c> attribute.
    /// </summary>
    protected readonly A11yDefaultsOptions Options = options.Value;

    /// <summary>
    /// Runs late so framework tag generation and most consumer tag helpers have
    /// already produced final attributes before accessibility enhancements are applied.
    /// </summary>
    public override int Order => 1000;

    /// <summary>
    /// Validator constant for min values in range validation.
    /// Used for validating min/max attributes on tag helpers.
    /// </summary>
    protected const string MinValidator = "min";

    /// <summary>
    /// Validator constant for max values in range validation.
    /// Used for validating min/max attributes on tag helpers.
    /// </summary>
    protected const string MaxValidator = "max";

    /// <summary>
    /// Removes all <c>a11y-*</c> attributes from the tag output to prevent
    /// them from appearing in the rendered HTML.
    /// </summary>
    /// <param name="output">The tag-helper output being modified.</param>
    protected static void RemovePackageAttributes(TagHelperOutput output)
    {
        var attributesToRemove = output.Attributes
            .Where(attr => attr.Name.StartsWith("a11y-", StringComparison.OrdinalIgnoreCase))
            .Select(attr => attr.Name)
            .ToList();

        foreach (var attributeName in attributesToRemove)
        {
            output.Attributes.RemoveAll(attributeName);
        }
    }

    /// <summary>
    /// Adds a CSS class to the element's class attribute, avoiding duplicates.
    /// </summary>
    /// <param name="output">The tag-helper output being modified.</param>
    /// <param name="cssClass">The CSS class to add.</param>
    protected static void AddCssClass(TagHelperOutput output, string cssClass)
    {
        if (string.IsNullOrWhiteSpace(cssClass))
        {
            return;
        }

        output.Attributes.SetAttribute("class", output.Attributes["class"]?.Value + $" {cssClass}".Trim());
    }
}   
