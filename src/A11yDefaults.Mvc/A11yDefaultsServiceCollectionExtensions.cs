// Copyright (c) 2026 tlex.dev
// SPDX-License-Identifier: MIT

using Microsoft.Extensions.DependencyInjection;

namespace A11yDefaults.Mvc;

/// <summary>
/// Registers package options for the accessibility-default tag helpers.
/// </summary>
public static class A11yDefaultsServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="A11yDefaultsOptions"/> to the service collection and
    /// optionally applies caller-provided configuration.
    /// </summary>
    /// <param name="services">The application service collection.</param>
    /// <param name="configure">An optional callback for overriding package defaults.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddA11yDefaults(
        this IServiceCollection services,
        Action<A11yDefaultsOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configure is null)
        {
            services.AddOptions<A11yDefaultsOptions>();
        }
        else
        {
            services.Configure(configure);
        }

        return services;
    }
}
