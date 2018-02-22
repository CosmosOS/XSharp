using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.References;
using Microsoft.VisualStudio.Shell.Interop;

namespace XSharp.ProjectSystem.VS.References
{
    [Export(typeof(IValidProjectReferenceChecker))]
    [AppliesTo(ProjectCapability.XSharp)]
    [Order(50)]
    internal class ValidProjectReferenceChecker : IValidProjectReferenceChecker
    {
        private IProjectThreadingService _projectThreadingService;

        [ImportingConstructor]
        public ValidProjectReferenceChecker(IProjectThreadingService projectThreadingService)
        {
            _projectThreadingService = projectThreadingService;
        }

        public async Task<SupportedCheckResult> CanAddProjectReferenceAsync(object referencedProject)
        {
            await _projectThreadingService.SwitchToUIThread();

            if (referencedProject is IVsReference reference)
            {
                return String.Equals(
                    Path.GetExtension(reference.FullPath), ".xsproj", StringComparison.OrdinalIgnoreCase)
                    ? SupportedCheckResult.Supported
                    : SupportedCheckResult.NotSupported;
            }

            return SupportedCheckResult.NotSupported;
        }

        public async Task<CanAddProjectReferencesResult> CanAddProjectReferencesAsync(
            IImmutableSet<object> referencedProjects)
        {
            var builder = ImmutableDictionary.CreateBuilder<object, SupportedCheckResult>();
            var errors = new List<string>();

            foreach (var referencedProject in referencedProjects)
            {
                var canAddProjectReference = await CanAddProjectReferenceAsync(referencedProject).ConfigureAwait(false);

                if (canAddProjectReference != SupportedCheckResult.Supported)
                {
                    await _projectThreadingService.SwitchToUIThread();

                    var projectName = "(Unknown)";

                    if (referencedProject is IVsReference reference)
                    {
                        projectName = reference.Name;
                    }

                    errors.Add("Unsupported project reference! Project name: " + projectName);
                }

                builder.Add(referencedProject, canAddProjectReference);
            }

            var individualResults = builder.ToImmutable();

            return new CanAddProjectReferencesResult(individualResults, String.Join(Environment.NewLine, errors));
        }

        public Task<SupportedCheckResult> CanBeReferencedAsync(object referencingProject) =>
            Task.FromResult(SupportedCheckResult.Supported);
    }
}
