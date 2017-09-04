using Microsoft.VisualStudio.ProjectSystem;

namespace XSharp.ProjectSystem
{
    internal class ProjectCapability
    {
        public const string AppDesigner = "AppDesigner";
        public const string OpenProjectFile = "OpenProjectFile";
        public const string UseFileGlobs = "UseFileGlobs";
        public const string XSharp = "XSharp";
        
        public const string InitialCapabilities = //ProjectCapability.AppDesigner + ";" +
                                                  ProjectCapabilities.Cps + ";" +
                                                  ProjectCapabilities.HandlesOwnReload + ";" +
                                                  //ProjectCapabilities.HostSetActiveProjectConfiguration + ";" +
                                                  //ProjectCapabilities.LanguageService + ";" +
                                                  ProjectCapability.OpenProjectFile + ";" +
                                                  //ProjectCapabilities.ProjectReferences + ";" +
                                                  ProjectCapabilities.RunningInVisualStudio + ";" +
                                                  //ProjectCapabilities.SdkReferences + ";" +
                                                  //ProjectCapabilities.SingleFileGenerators + ";" +
                                                  ProjectCapability.UseFileGlobs + ";" +
                                                  ProjectCapability.XSharp;
    }
}
