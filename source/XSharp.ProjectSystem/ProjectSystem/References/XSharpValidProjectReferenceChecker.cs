using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.References;

namespace XSharp.ProjectSystem.References
{
    [Export(typeof(IValidProjectReferenceChecker))]
    [AppliesTo(ProjectCapability.XSharp)]
    internal class XSharpValidProjectReferenceChecker : IValidProjectReferenceChecker
    {
        public Task<SupportedCheckResult> CanAddProjectReferenceAsync(object aReferencedProject)
        {
            var xReferencedProject = (IUnresolvedBuildDependencyProjectReference)aReferencedProject;
            return Task.FromResult(xReferencedProject != null
                                   && Path.GetExtension(xReferencedProject.EvaluatedIncludeAsFullPath) == ".xsproj"
                                   ? SupportedCheckResult.Supported
                                   : SupportedCheckResult.NotSupported);
        }

        public async Task<CanAddProjectReferencesResult> CanAddProjectReferencesAsync(IImmutableSet<object> aReferencedProjects)
        {
            var xBuilder = ImmutableDictionary.CreateBuilder<object, SupportedCheckResult>();
            var xErrors = new List<string>();

            foreach (var xReferencedProject in aReferencedProjects)
            {
                var xCanAddProjectReference = await CanAddProjectReferenceAsync(xReferencedProject);

                if (xCanAddProjectReference != SupportedCheckResult.Supported)
                {
                    var xProjectName = await ((IUnresolvedBuildDependencyProjectReference)xReferencedProject).GetNameAsync();
                    xErrors.Add("Unsupported project reference! Project name: " + xProjectName);
                }

                xBuilder.Add(xReferencedProject, xCanAddProjectReference);
            }

            var xIndividualResults = xBuilder.ToImmutable();

            return new CanAddProjectReferencesResult(xIndividualResults, String.Join(Environment.NewLine, xErrors));
        }

        public Task<SupportedCheckResult> CanBeReferencedAsync(object aReferencingProject) =>
            Task.FromResult(SupportedCheckResult.Supported);
    }
}
