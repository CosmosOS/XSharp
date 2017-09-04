using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.Shell;

using XSharp.ProjectSystem;

[assembly: ProjectTypeRegistration(XSharpProjectSystemPackage.ProjectTypeGuid, "#1", "#2", "xsproj", "XSharp",
    XSharpProjectSystemPackage.PackageGuid, Capabilities = ProjectCapability.InitialCapabilities,
    ProjectTemplatesDir = "ProjectTemplates"/*, DisplayProjectTypeVsTemplate = "#1"*/)]

namespace XSharp.ProjectSystem
{
    [Guid(PackageGuid)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    public sealed class XSharpProjectSystemPackage : Package
    {
        /// <summary>
        /// The GUID for this package.
        /// </summary>
        public const string PackageGuid = "d9eacd85-6e48-4c5f-95a2-51f85a57b517";

        /// <summary>
        /// The GUID for this project type.  It is unique with the project file extension and
        /// appears under the VS registry hive's Projects key.
        /// </summary>
        public const string ProjectTypeGuid = "68a6f609-e61d-4b95-b063-dbb124f0f0ac";
    }
}
