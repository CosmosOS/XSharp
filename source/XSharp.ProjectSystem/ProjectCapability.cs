using static Microsoft.VisualStudio.ProjectSystem.ProjectCapabilities;

namespace XSharp.ProjectSystem
{
    internal class ProjectCapability
    {
        public const string AppDesigner = nameof(AppDesigner);
        public const string OpenProjectFile = nameof(OpenProjectFile);
        public const string UseFileGlobs = nameof(UseFileGlobs);
        public const string XSharp = nameof(XSharp);
        
        public const string InitialCapabilities = //AppDesigner + ";" +
                                                  Cps + ";" +
                                                  HandlesOwnReload + ";" +
                                                  //HostSetActiveProjectConfiguration + ";" +
                                                  OpenProjectFile + ";" +
                                                  ProjectReferences + ";" +
                                                  //ReferencesFolder + ";" +
                                                  //SdkReferences + ";" +
                                                  //SingleFileGenerators + ";" +
                                                  UseFileGlobs + ";" +
                                                  XSharp;
    }
}
