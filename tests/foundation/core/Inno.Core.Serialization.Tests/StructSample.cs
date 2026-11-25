namespace Inno.Core.Serialization.Tests;

internal struct StructSample
{
    public int x;
    public int y { get; set; }
    public readonly int readOnly;

    [SerializableProperty]
    private int m_hidden;

    public readonly int computed => x + m_hidden;
    public readonly int hidden => m_hidden;

    private StructSample(
        int x,
        int y,
        int hidden
    ) {
        this.x = x;
        this.y = y;
        this.readOnly = 99;
        m_hidden = hidden;
    }

    internal static StructSample Create(
        int x,
        int y,
        int hidden
    ) => new(x, y, hidden);
}
