using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.ProjectSystem.VS;

using XSharp.ProjectSystem;

[assembly: ProjectTypeRegistration(XSharpProjectSystemPackage.ProjectTypeGuid, "#1", "#2", "xsproj", "XSharp",
    XSharpProjectSystemPackage.PackageGuid, Capabilities = ProjectCapability.XSharp, DisplayProjectTypeVsTemplate = "#1")]

namespace XSharp.ProjectSystem
{
    [Guid(PackageGuid)]
    [PackageRegistration(RegisterUsing = RegistrationMethod.CodeBase, UseManagedResourcesOnly = true)]
    public sealed class XSharpProjectSystemPackage : Package
    {
        /// <summary>
        /// The GUID for this package.
        /// </summary>
        public const string PackageGuid = "b184171f-9711-4627-be0f-0168fbaa746a";

        /// <summary>
        /// The GUID for this project type.  It is unique with the project file extension and
        /// appears under the VS registry hive's Projects key.
        /// </summary>
        public const string ProjectTypeGuid = "b0fd1319-c5a1-4bf7-9ad9-63b47a426593";
    }
}
