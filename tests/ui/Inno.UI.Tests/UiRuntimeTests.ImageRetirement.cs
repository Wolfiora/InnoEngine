using System;
using System.IO;
using Inno.Assets;
using Inno.Core.Execution;
using Inno.Rendering.Assets;
using Inno.UI.Runtime;
using Xunit;

namespace Inno.UI.Tests;

public sealed partial class UiRuntimeTests
{
    [Fact]
    public void BackendRetirementProtectsImageArtifactsAndRejectsNewWorkUntilRetryCompletes()
    {
        bool backendPending = true;
        int backendStops = 0;
        var image = new PendingImage();
        var artifacts = new ImageArtifacts();
        var backend = new CapturingBackend
        {
            registerImage = (
                _,
                _
            ) => image,
            stopping = () =>
            {
                backendStops++;
                if (backendPending)
                    throw new RetirementPendingException("Native frames are still draining.");
            }
        };
        var runtime = new UiRuntime(backend, artifacts);
        runtime.RegisterImage(new(1), ImageSource());
        try
        {
            Exception failure = Assert.ThrowsAny<Exception>(runtime.Dispose);
            Assert.NotNull(RetirementPendingException.Find(failure));
            Assert.Equal(0, image.disposeCalls);
            Assert.Equal(1, artifacts.retained);
            Assert.Throws<InvalidOperationException>(() => runtime.CreateContext(new("rejected", 10, 10)));

            backendPending = false;
            image.pending = true;
            Assert.NotNull(RetirementPendingException.Find(Assert.ThrowsAny<Exception>(runtime.Dispose)));
            Assert.Equal(1, artifacts.retained);
            image.pending = false;
            runtime.Dispose();
            Assert.Equal(0, artifacts.retained);
            Assert.Equal(2, backendStops);
        }
        finally
        {
            backendPending = false;
            image.pending = false;
            runtime.Dispose();
        }
    }

    [Fact]
    public void ContextRetirementRetriesResourcesWithoutDestroyingTheNativeContextTwice()
    {
        var image = new PendingImage { pending = true };
        var artifacts = new ImageArtifacts();
        var backend = new CapturingBackend
        {
            registerImage = (
                _,
                _
            ) => image
        };
        using var runtime = new UiRuntime(backend, artifacts);
        UiContextHandle context = runtime.CreateContext(new("retiring", 10, 10));
        runtime.RegisterImage(context, ImageSource());
        try
        {
            Assert.ThrowsAny<Exception>(() => runtime.DestroyContext(context));
            Assert.Equal(1, backend.destroyedContexts);
            Assert.Equal(1, artifacts.retained);
            Assert.Throws<InvalidOperationException>(() => runtime.Render(context));
            image.pending = false;
            runtime.DestroyContext(context);
            Assert.Equal(1, backend.destroyedContexts);
            Assert.Equal(0, artifacts.retained);
            runtime.CreateContext(new("next", 10, 10));
        }
        finally
        {
            image.pending = false;
        }
    }

    private static UiImageSource ImageSource()
        => new(new(Guid.NewGuid(), 1, new("color", "image", TextureColorSpace.Srgb)), 2, 1);

    private sealed class PendingImage : IUiImageRegistration
    {
        internal bool pending;
        internal int disposeCalls;

        public string source => "test-image://owned";

        public void Dispose()
        {
            disposeCalls++;
            if (pending)
                throw new RetirementPendingException("Image retirement is pending.");
        }
    }

    private sealed class ImageArtifacts : AssetResidencyProvider, IAssetArtifactLookup
    {
        internal int retained;

        public ArtifactLease AcquireArtifact(
            Guid persistentId,
            string outputName
        ) {
            retained++;
            return CreateArtifactLease(new(new(new string('A', 64)), outputName, "HASH", 0),
                () => new MemoryStream(), () => retained--);
        }

        public bool TryGetArtifact(
            Guid persistentId,
            string outputName,
            out AssetArtifactInfo? artifact
        ) {
            artifact = null;
            return false;
        }
    }
}
