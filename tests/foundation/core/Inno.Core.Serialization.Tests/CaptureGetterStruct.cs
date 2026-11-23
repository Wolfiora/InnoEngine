namespace Inno.Core.Serialization.Tests;

internal struct CaptureGetterStruct
{
    internal static int calls;

    public int value
    {
        get
        {
            calls++;
            return 1;
        }
        set {}
    }
}
