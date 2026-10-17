using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

sealed partial class RenderRuntime
{
    private sealed class RenderFrameScratch
    {
        internal readonly List<RenderContributorSnapshot.Entry> contributors = [];
        internal readonly List<ScheduledWork> scheduled = [];
        internal readonly List<KeyValuePair<RenderPipelineAsset, GenerationCacheEntry>> generations = [];
        internal readonly List<RenderExtensionRegistry.RenderModelEntry> applicableModels = [];
        internal readonly List<RenderExtensionRegistry.RenderModelEntry> orderedModels = [];
        internal readonly List<RenderRequest> modelRequests = [];
        internal readonly RenderPresentationComposer presentation = new();

        internal void Clear()
        {
            contributors.Clear();
            scheduled.Clear();
            generations.Clear();
            presentation.Clear();
            applicableModels.Clear();
            orderedModels.Clear();
            modelRequests.Clear();
        }
    }
}
