using A11yDefaultsPiranhaModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha;
using Piranha.AspNetCore;

public static class A11yDefaultsPiranhaModuleExtensions
{
    /// <summary>
    /// Adds the A11yDefaultsPiranhaModule module.
    /// </summary>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public static PiranhaServiceBuilder UseA11yDefaultsPiranhaModule(this PiranhaServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddA11yDefaultsPiranhaModule();

        return serviceBuilder;
    }

    /// <summary>
    /// Uses the A11yDefaultsPiranhaModule module.
    /// </summary>
    /// <param name="applicationBuilder">The current application builder</param>
    /// <returns>The builder</returns>
    public static PiranhaApplicationBuilder UseA11yDefaultsPiranhaModule(this PiranhaApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Builder.UseA11yDefaultsPiranhaModule();

        return applicationBuilder;
    }

    /// <summary>
    /// Adds the A11yDefaultsPiranhaModule module.
    /// </summary>
    /// <param name="services">The current service collection</param>
    /// <returns>The services</returns>
    public static IServiceCollection AddA11yDefaultsPiranhaModule(this IServiceCollection services)
    {
        // Add the A11yDefaultsPiranhaModule module
        Piranha.App.Modules.Register<Module>();

        // Setup authorization policies
        services.AddAuthorization(o =>
        {
            // A11yDefaultsPiranhaModule policies
            o.AddPolicy(Permissions.A11yDefaultsPiranhaModule, policy =>
            {
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModule, Permissions.A11yDefaultsPiranhaModule);
            });

            // A11yDefaultsPiranhaModule add policy
            o.AddPolicy(Permissions.A11yDefaultsPiranhaModuleAdd, policy =>
            {
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModule, Permissions.A11yDefaultsPiranhaModule);
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModuleAdd, Permissions.A11yDefaultsPiranhaModuleAdd);
            });

            // A11yDefaultsPiranhaModule edit policy
            o.AddPolicy(Permissions.A11yDefaultsPiranhaModuleEdit, policy =>
            {
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModule, Permissions.A11yDefaultsPiranhaModule);
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModuleEdit, Permissions.A11yDefaultsPiranhaModuleEdit);
            });

            // A11yDefaultsPiranhaModule delete policy
            o.AddPolicy(Permissions.A11yDefaultsPiranhaModuleDelete, policy =>
            {
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModule, Permissions.A11yDefaultsPiranhaModule);
                policy.RequireClaim(Permissions.A11yDefaultsPiranhaModuleDelete, Permissions.A11yDefaultsPiranhaModuleDelete);
            });
        });

        // Return the service collection
        return services;
    }

    /// <summary>
    /// Uses the A11yDefaultsPiranhaModule.
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The builder</returns>
    public static IApplicationBuilder UseA11yDefaultsPiranhaModule(this IApplicationBuilder builder)
    {
        return builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "A11yDefaultsPiranhaModule.assets.dist"),
            RequestPath = "/manager/A11yDefaultsPiranhaModule"
        });
    }

    /// <summary>
    /// Static accessor to A11yDefaultsPiranhaModule module if it is registered in the Piranha application.
    /// </summary>
    /// <param name="modules">The available modules</param>
    /// <returns>The A11yDefaultsPiranhaModule module</returns>
    public static Module A11yDefaultsPiranhaModule(this Piranha.Runtime.AppModuleList modules)
    {
        return modules.Get<Module>();
    }
}
