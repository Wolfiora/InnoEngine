using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Inno.Audio;
using Inno.Core.Execution;
using Xunit;

namespace Inno.Adapter.Audio.MiniAudio.Tests;

public sealed class EncodedContentTests
{
    [Theory]
    [InlineData(AudioClipLoadMode.Decode)]
    [InlineData(AudioClipLoadMode.Stream)]
    public void NativePlaybackUsesOneAsynchronousEncodingPreparationForSharedClips(AudioClipLoadMode mode)
    {
        using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
        IAudioDevice backend = device;
        using var source = new GatedSource(CreateWave());
        AudioClipDescriptor descriptor = Describe(source, mode);
        int owner = Environment.CurrentManagedThreadId;
        AudioClipHandle first = backend.CreateClip(descriptor, source);
        AudioClipHandle second = backend.CreateClip(descriptor, source);
        try
        {
            Assert.True(source.entered.Wait(TimeSpan.FromSeconds(5)));
            Assert.NotEqual(owner, source.openThread);
            Assert.Equal(AudioClipState.Preparing, backend.GetClipState(first));
            Assert.Throws<RetirementPendingException>(() => backend.DestroyClip(first));
            Assert.Equal(0, source.closures);
            source.Release();
            WaitForReady(backend, first);
            WaitForReady(backend, second);
            Assert.Equal(1, source.opens);
            Assert.Equal(1, source.closures);
            Assert.True(backend.DestroyClip(first));
            AudioBusHandle master = backend.CreateBus(AudioBusId.master);
            AudioDeviceVoiceHandle voice = backend.Play(second, master, new AudioPlayOptions(loop: true));
            Assert.True(voice.isValid);
            Assert.True(backend.Stop(voice));
            Assert.True(backend.DestroyClip(second));
        }
        finally
        {
            source.Release();
            Retire(() => backend.DestroyClip(first));
            Retire(() => backend.DestroyClip(second));
        }
    }

    [Fact]
    public void CancellationDrainsTheSourceAndAllowsTheSameContentToBePreparedAgain()
    {
        using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
        IAudioDevice backend = device;
        using var source = new GatedSource(CreateWave());
        AudioClipHandle abandoned = backend.CreateClip(Describe(source, AudioClipLoadMode.Stream), source);
        Assert.True(source.entered.Wait(TimeSpan.FromSeconds(5)));
        Retire(() => backend.DestroyClip(abandoned));
        Assert.Equal(1, source.closures);
        source.Release();
        AudioClipHandle replacement = backend.CreateClip(Describe(source, AudioClipLoadMode.Stream), source);
        WaitForReady(backend, replacement);
        Assert.Equal(2, source.opens);
        Assert.Equal(2, source.closures);
        Assert.True(backend.DestroyClip(replacement));
    }

    [Fact]
    public void InvalidEncodingIdentityFailsPreparationWithoutLeakingTheSource()
    {
        using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
        IAudioDevice backend = device;
        using var source = new GatedSource(CreateWave()) { contentHash = new string('A', 64) };
        source.Release();
        AudioClipHandle clip = backend.CreateClip(Describe(source, AudioClipLoadMode.Decode), source);
        Assert.True(SpinWait.SpinUntil(() => backend.GetClipState(clip) != AudioClipState.Preparing,
            TimeSpan.FromSeconds(5)));
        Assert.Equal(AudioClipState.Failed, backend.GetClipState(clip));
        Assert.Equal(1, source.closures);
        Assert.True(backend.DestroyClip(clip));
        Assert.Equal(0, backend.statistics.loadedClips);
    }

    private static AudioClipDescriptor Describe(
        IAudioClipSource source,
        AudioClipLoadMode mode
    ) => new(AudioCodecId.wav, mode, 1, 48000, 4800, source.length);

    private static void WaitForReady(
        IAudioDevice backend,
        AudioClipHandle clip
    ) {
        Assert.True(SpinWait.SpinUntil(() => backend.GetClipState(clip) != AudioClipState.Preparing,
            TimeSpan.FromSeconds(5)));
        Assert.Equal(AudioClipState.Ready, backend.GetClipState(clip));
    }

    private static void Retire(Func<bool> cleanup)
    {
        Assert.True(SpinWait.SpinUntil(() =>
        {
            try
            {
                _ = cleanup();
                return true;
            }
            catch (RetirementPendingException)
            {
                return false;
            }
        }, TimeSpan.FromSeconds(5)));
    }

    private static byte[] CreateWave()
    {
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output);
        writer.Write("RIFF"u8);
        writer.Write(36 + 9600);
        writer.Write("WAVEfmt "u8);
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)1);
        writer.Write(48000);
        writer.Write(96000);
        writer.Write((short)2);
        writer.Write((short)16);
        writer.Write("data"u8);
        writer.Write(9600);
        writer.Write(new byte[9600]);
        return output.ToArray();
    }

    private sealed class GatedSource : IAudioClipSource, IDisposable
    {
        private readonly byte[] m_bytes;
        private readonly TaskCompletionSource m_release = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal readonly ManualResetEventSlim entered = new();
        internal int opens;
        internal int closures;
        internal int openThread;

        internal GatedSource(byte[] bytes)
        {
            m_bytes = bytes;
            contentHash = Convert.ToHexString(SHA256.HashData(bytes));
        }

        public string contentHash { get; init; }
        public long length => m_bytes.Length;

        public Stream OpenRead()
        {
            Interlocked.Increment(ref opens);
            openThread = Environment.CurrentManagedThreadId;
            entered.Set();
            return new GatedStream(this);
        }

        public void Dispose()
        {
            Release();
            entered.Dispose();
        }

        internal void Release() => m_release.TrySetResult();

        private sealed class GatedStream : MemoryStream
        {
            private readonly GatedSource m_owner;
            private bool m_closed;

            internal GatedStream(GatedSource owner) : base(owner.m_bytes, writable: false) => m_owner = owner;

            public override async ValueTask<int> ReadAsync(
                Memory<byte> buffer,
                CancellationToken cancellationToken = default
            ) {
                await m_owner.m_release.Task.WaitAsync(cancellationToken).ConfigureAwait(false);
                return await base.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
            }

            protected override void Dispose(bool disposing)
            {
                if (!m_closed)
                {
                    m_closed = true;
                    Interlocked.Increment(ref m_owner.closures);
                }
                base.Dispose(disposing);
            }
        }
    }
}
