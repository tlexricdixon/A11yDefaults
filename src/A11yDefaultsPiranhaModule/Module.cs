using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;

namespace A11yDefaultsPiranhaModule
{
    public class Module : IModule
    {
        private readonly List<PermissionItem> _permissions = new List<PermissionItem>
        {
            new PermissionItem { Name = Permissions.A11yDefaultsPiranhaModule, Title = "List A11yDefaultsPiranhaModule content", Category = "A11yDefaultsPiranhaModule", IsInternal = true },
            new PermissionItem { Name = Permissions.A11yDefaultsPiranhaModuleAdd, Title = "Add A11yDefaultsPiranhaModule content", Category = "A11yDefaultsPiranhaModule", IsInternal = true },
            new PermissionItem { Name = Permissions.A11yDefaultsPiranhaModuleEdit, Title = "Edit A11yDefaultsPiranhaModule content", Category = "A11yDefaultsPiranhaModule", IsInternal = true },
            new PermissionItem { Name = Permissions.A11yDefaultsPiranhaModuleDelete, Title = "Delete A11yDefaultsPiranhaModule content", Category = "A11yDefaultsPiranhaModule", IsInternal = true }
        };

        /// <summary>
        /// Gets the module author
        /// </summary>
        public string Author => "";

        /// <summary>
        /// Gets the module name
        /// </summary>
        public string Name => "";

        /// <summary>
        /// Gets the module version
        /// </summary>
        public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// Gets the module description
        /// </summary>
        public string Description => "";

        /// <summary>
        /// Gets the module package url
        /// </summary>
        public string PackageUrl => "";

        /// <summary>
        /// Gets the module icon url
        /// </summary>
        public string IconUrl => "/manager/PiranhaModule/piranha-logo.png";

        public void Init()
        {
            // Register permissions
            foreach (var permission in _permissions)
            {
                App.Permissions["A11yDefaultsPiranhaModule"].Add(permission);
            }

            // Add manager menu items
            Menu.Items.Add(new MenuItem
            {
                InternalId = "A11yDefaultsPiranhaModule",
                Name = "A11yDefaultsPiranhaModule",
                Css = "fas fa-box"
            });
            Menu.Items["A11yDefaultsPiranhaModule"].Items.Add(new MenuItem
            {
                InternalId = "A11yDefaultsPiranhaModuleStart",
                Name = "Module Start",
                Route = "~/manager/a11ydefaultspiranhamodule",
                Policy = Permissions.A11yDefaultsPiranhaModule,
                Css = "fas fa-box"
            });
        }
    }
}
