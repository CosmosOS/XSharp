namespace VSPropertyPages.Sample
{
    internal class ProjectCapability
    {
        #region CPS Capabilities

        /// <summary>
        /// Indicates that the project uses the app designer for managing project properties.
        /// </summary>
        public const string AppDesigner = nameof(AppDesigner);

        #endregion

        /// <summary>
        /// The project shows sample property pages, if the <see cref="AppDesigner"/> capability is declared.
        /// </summary>
        public const string VSPropertyPagesSample = nameof(VSPropertyPagesSample);

        public const string VSPropertyPagesSampleAndAppDesigner = VSPropertyPagesSample + " & " + AppDesigner;
    }
}
