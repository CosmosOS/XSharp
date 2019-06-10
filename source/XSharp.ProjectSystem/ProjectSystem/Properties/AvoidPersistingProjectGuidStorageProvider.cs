using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio.ProjectSystem.Build;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using XSharp.ProjectSystem;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Properties
{

    /// <summary>
    ///     Implementation of <see cref="IProjectGuidStorageProvider"/> that avoids persisting the 
    ///     project GUID property to the project file if isn't already present in the file.
    /// </summary>
    [Export(typeof(IProjectGuidStorageProvider))]
    [AppliesTo(ProjectCapability.XSharp)]
    [Order(10)]
    internal sealed class AvoidPersistingProjectGuidStorageProvider : IProjectGuidStorageProvider
    {
        private static readonly Guid Guid = new Guid(XSharpProjectSystemPackage.ProjectTypeGuid);

        // I think UnconfiguredProject has to be imported for scoping
        [ImportingConstructor]
        public AvoidPersistingProjectGuidStorageProvider(UnconfiguredProject unconfiguredProject)
        {
        }

        public Task<Guid> GetProjectGuidAsync() => Task.FromResult(Guid);

        // The function is called but we simply ignore it
        public Task SetProjectGuidAsync(Guid guid)
        {
            return Task.FromResult(0);
        }
    }
}
