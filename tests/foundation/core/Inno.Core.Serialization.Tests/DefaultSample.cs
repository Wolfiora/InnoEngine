using System;
using System.Collections.Generic;

namespace Inno.Core.Serialization.Tests;

internal sealed class DefaultSample : ISerializable
{
    [SerializableProperty]
    public bool enabled { get; set; }

    [SerializableProperty]
    public int count { get; set; }

    [SerializableProperty]
    public float ratio { get; set; }

    [SerializableProperty]
    public decimal price { get; set; }

    [SerializableProperty]
    public string name { get; set; } = string.Empty;

    [SerializableProperty]
    public Guid identity { get; set; }

    [SerializableProperty]
    public SampleMode mode { get; set; }

    [SerializableProperty]
    public int? optional { get; set; }

    [SerializableProperty]
    public byte[] bytes { get; set; } = [];

    [SerializableProperty]
    public int[] numbers { get; set; } = [];

    [SerializableProperty]
    public List<int> values { get; set; } = [];

    [SerializableProperty]
    public Dictionary<int, string> map { get; set; } = [];

    [SerializableProperty]
    public StructSample data { get; set; }
}
