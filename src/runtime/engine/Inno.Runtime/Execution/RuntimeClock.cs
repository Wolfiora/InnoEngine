using System;
using Inno.Core.Execution;
using Inno.Runtime.Contracts;

namespace Inno.Runtime;

internal sealed class RuntimeClock
{
    private static readonly ExecutionSlot<RuntimeClock> CURRENT_SCOPE = new("clock");

    internal static RuntimeClock current => CURRENT_SCOPE.current;

    internal float time { get; private set; }

    internal float deltaTime { get; private set; }

    internal float unscaledTime { get; private set; }

    internal float unscaledDeltaTime { get; private set; }

    internal float fixedTime { get; private set; }

    internal float fixedDeltaTime { get; private set; }

    internal float timeScale { get; private set; } = 1f;

    internal bool isPaused { get; private set; }

    internal long frameCount { get; private set; }

    internal long fixedStepCount { get; private set; }

    internal IDisposable EnterScope() => CURRENT_SCOPE.Enter(this);

    internal RuntimeFrame Update(float unscaledDeltaTime)
    {
        this.unscaledDeltaTime = unscaledDeltaTime;
        unscaledTime += unscaledDeltaTime;
        deltaTime = isPaused ? 0f : unscaledDeltaTime * timeScale;
        time += deltaTime;
        var frame = new RuntimeFrame(frameCount, time, unscaledTime, deltaTime, unscaledDeltaTime, timeScale, isPaused);
        frameCount++;
        return frame;
    }

    internal RuntimeFixedFrame BeginFixedStep(float fixedDeltaTime)
    {
        this.fixedDeltaTime = fixedDeltaTime;
        fixedTime += fixedDeltaTime;
        var frame = new RuntimeFixedFrame(fixedStepCount, fixedTime, fixedDeltaTime);
        fixedStepCount++;
        return frame;
    }

    internal void SetTimeScale(float value)
    {
        if (!float.IsFinite(value) || value < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Time scale must be finite and non-negative.");
        }
        timeScale = value;
    }

    internal void SetPaused(bool value) => isPaused = value;
}
