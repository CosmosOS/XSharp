using static Microsoft.VisualStudio.ProjectSystem.ProjectCapabilities;

namespace XSharp.ProjectSystem
{
    internal class ProjectCapability
    {
        #region Capabilities

        /// <summary>
        /// Indicates that the project uses the app designer for managing project properties.
        /// </summary>
        public const string AppDesigner = nameof(AppDesigner);
        /// <summary>
        /// Indicates that the project is a typical MSBuild project (not DNX) in that it declares source items in the project itself (rather than a project.json file that assumes all files in the directory are part of a compilation).
        /// </summary>
        public const string DeclaredSourceItems = nameof(DeclaredSourceItems);
        /// <summary>
        /// Indicates that the project supports the dependencies node in Visual Studio.
        /// </summary>
        public const string DependenciesTree = nameof(DependenciesTree);
        /// <summary>
        /// Indicates that the project supports the edit and continue debugging feature.
        /// </summary>
        public const string EditAndContinue = nameof(EditAndContinue);
        /// <summary>
        /// Indicates that the project is capable of handling the project file being edited live in an IDE while the project is already loaded.
        /// </summary>
        public const string OpenProjectFile = nameof(OpenProjectFile);
        /// <summary>
        /// Indicates that the project file can include files using MSBuild file globbing patterns.
        /// </summary>
        public const string UseFileGlobs = nameof(UseFileGlobs);
        /// <summary>
        /// Indicates that the user is allowed to add arbitrary files to their project.
        /// </summary>
        public const string UserSourceItems = nameof(UserSourceItems);
        /// <summary>
        /// Project may contain or compile X# source files.
        /// </summary>
        public const string XSharp = nameof(XSharp);

        #endregion

        public const string InitialCapabilities = AppDesigner + ";" +
                                                  // todo: implement
                                                  //EditAndContinue + ";" +
                                                  HandlesOwnReload + ";" +
                                                  OpenProjectFile + ";" +
                                                  PreserveFormatting + ";" +
                                                  XSharp;
    }
}
