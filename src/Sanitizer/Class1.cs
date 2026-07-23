using Ganss.Xss;
using Microsoft.Extensions.DependencyInjection;

namespace Sanitizer;

public static class HtmlSanitizerConfiguration
{
    public static IServiceCollection AddCmsHtmlSanitizer(
    this IServiceCollection services)
    {
        services.AddSingleton<HtmlSanitizer>(_ =>
        {
            var sanitizer = new HtmlSanitizer();

            // Remove anything the public site should never receive.
            sanitizer.AllowedTags.Remove("script");
            sanitizer.AllowedTags.Remove("style");
            sanitizer.AllowedTags.Remove("iframe");
            sanitizer.AllowedTags.Remove("object");
            sanitizer.AllowedTags.Remove("embed");
            sanitizer.AllowedTags.Remove("form");
            sanitizer.AllowedTags.Remove("input");
            sanitizer.AllowedTags.Remove("button");

            // Keep the useful attributes needed by CMS content.
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("id");
            sanitizer.AllowedAttributes.Add("title");
            sanitizer.AllowedAttributes.Add("role");

            // Accessibility attributes are not always included by default.
            sanitizer.AllowedAttributes.Add("aria-label");
            sanitizer.AllowedAttributes.Add("aria-labelledby");
            sanitizer.AllowedAttributes.Add("aria-describedby");
            sanitizer.AllowedAttributes.Add("aria-hidden");
            sanitizer.AllowedAttributes.Add("aria-current");
            sanitizer.AllowedAttributes.Add("aria-expanded");
            sanitizer.AllowedAttributes.Add("aria-controls");

            // Useful content attributes.
            sanitizer.AllowedAttributes.Add("target");
            sanitizer.AllowedAttributes.Add("rel");
            sanitizer.AllowedAttributes.Add("loading");
            sanitizer.AllowedAttributes.Add("width");
            sanitizer.AllowedAttributes.Add("height");
            sanitizer.AllowedAttributes.Add("colspan");
            sanitizer.AllowedAttributes.Add("rowspan");
            sanitizer.AllowedAttributes.Add("scope");

            return sanitizer;
        });

        return services;
    }
}

}
