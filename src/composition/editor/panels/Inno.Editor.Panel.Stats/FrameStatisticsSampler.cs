using System;

namespace Inno.Editor.Panel.Stats;

internal sealed class FrameStatisticsSampler
{
    private const int C_SAMPLE_CAPACITY = 120;
    private const float C_SAMPLE_INTERVAL_SECONDS = 0.5f;
    private const float C_SAMPLE_RESET_SECONDS = 1f;
    private const float C_SAMPLE_WINDOW_SECONDS = 60f;
    private readonly float[] m_frameTimes = new float[C_SAMPLE_CAPACITY];
    private readonly float[] m_frameRates = new float[C_SAMPLE_CAPACITY];
    private readonly float[] m_sampleTimes = new float[C_SAMPLE_CAPACITY];
    private float m_lastFrameTime = float.NaN;
    private float m_nextSampleTime;
    private double m_duration;
    private int m_frames;
    private int m_count;

    internal float deltaTime { get; private set; }
    internal float framesPerSecond { get; private set; }
    internal ReadOnlySpan<float> frameTimes => m_frameTimes.AsSpan(0, m_count);
    internal ReadOnlySpan<float> frameRates => m_frameRates.AsSpan(0, m_count);

    internal bool Update(
        float totalTime,
        float frameDeltaTime
    ) {
        if (!float.IsFinite(totalTime) || !float.IsFinite(frameDeltaTime) || frameDeltaTime <= 0f)
            return false;
        if (float.IsFinite(m_lastFrameTime)
            && (totalTime < m_lastFrameTime || totalTime - m_lastFrameTime > C_SAMPLE_RESET_SECONDS
                || totalTime - m_lastFrameTime > frameDeltaTime * 1.5f + 0.0001f))
            Reset();
        m_lastFrameTime = totalTime;
        int expired = 0;
        while (expired < m_count && m_sampleTimes[expired] < totalTime - C_SAMPLE_WINDOW_SECONDS)
            expired++;
        RemoveOldest(expired);
        m_duration += frameDeltaTime;
        m_frames++;
        if (m_count > 0 && totalTime < m_nextSampleTime)
            return false;
        deltaTime = (float)(m_duration / m_frames);
        framesPerSecond = (float)(m_frames / m_duration);
        if (m_count == C_SAMPLE_CAPACITY)
            RemoveOldest(1);
        m_frameTimes[m_count] = deltaTime * 1000f;
        m_frameRates[m_count] = framesPerSecond;
        m_sampleTimes[m_count++] = totalTime;
        m_nextSampleTime = totalTime + C_SAMPLE_INTERVAL_SECONDS;
        m_frames = 0;
        m_duration = 0d;
        return true;
    }

    internal void Reset()
    {
        m_count = 0;
        m_lastFrameTime = float.NaN;
        m_frames = 0;
        m_duration = 0d;
        deltaTime = 0f;
        framesPerSecond = 0f;
    }

    private void RemoveOldest(int count)
    {
        if (count == 0)
            return;
        m_count -= count;
        Array.Copy(m_frameTimes, count, m_frameTimes, 0, m_count);
        Array.Copy(m_frameRates, count, m_frameRates, 0, m_count);
        Array.Copy(m_sampleTimes, count, m_sampleTimes, 0, m_count);
    }
}
