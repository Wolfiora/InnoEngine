using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Inno.Native.UI;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Owns context-local imported image registrations without acquiring texture pixels or GPU resources.
/// </summary>
public sealed unsafe partial class RmlUiBackend
{
    private static readonly Regex S_IMAGE_BINDING_ELEMENT = new(
        """(?<comment><!--[\s\S]*?-->)|<img\b[^>]*>""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex S_IMAGE_BINDING_SHEET = new(
        """(?<comment>/\*[\s\S]*?\*/)|@spritesheet\s+[^{}]+\{[^{}]*\}""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex S_IMAGE_BINDING_ATTRIBUTE = new(
        """(?<prefix>\bsrc\s*=\s*)(?:"(?<double>[^"]*)"|'(?<single>[^']*)'|(?<bare>[^\s>]+))""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex S_IMAGE_BINDING_PROPERTY = new(
        """(?<prefix>\bsrc\s*:\s*)(?<value>[^;]+);""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private readonly Dictionary<ulong, ImageRegistration> m_images = [];
    private ulong m_nextImage;

    /// <inheritdoc />
    public IUiImageRegistration RegisterImage(
        UiContextHandle context,
        UiImageSource image
    ) {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(image);
        ulong id = checked(++m_nextImage);
        string source = "inno-image://" + context.value.ToString(CultureInfo.InvariantCulture)
            + "/" + id.ToString(CultureInfo.InvariantCulture);
        var registration = new ImageRegistration(this, context, id, source, image);
        m_images.Add(id, registration);
        try
        {
            using var nativeSource = new RmlUiUtf8String(source);
            Result result = default;
            BgcsInnoRmluiError* error = null;
            RmlUiNativeErrors.CheckBridge(UiNative.Raw.RegisterImage(
                m_runtime, Require(context), (sbyte*)nativeSource.pointer, id, image.width, image.height,
                &result, &error), error);
            ThrowIfFailed(result, "register imported UI image");
            return registration;
        }
        catch
        {
            m_images.Remove(id);
            throw;
        }
    }

    private static string RewriteImageBindings(
        string source,
        IReadOnlyDictionary<string, string>? bindings
    ) {
        if (bindings is null || bindings.Count == 0)
            return source;
        string text = S_IMAGE_BINDING_ELEMENT.Replace(source, element =>
            element.Groups["comment"].Success ? element.Value
            : S_IMAGE_BINDING_ATTRIBUTE.Replace(element.Value, match =>
        {
            string value = match.Groups["double"].Success ? match.Groups["double"].Value
                : match.Groups["single"].Success ? match.Groups["single"].Value : match.Groups["bare"].Value;
            return bindings.TryGetValue(value, out string? mapped)
                ? match.Groups["prefix"].Value + "\"" + mapped + "\"" : match.Value;
        }));
        return S_IMAGE_BINDING_SHEET.Replace(text, sheet =>
            sheet.Groups["comment"].Success ? sheet.Value
            : S_IMAGE_BINDING_PROPERTY.Replace(sheet.Value, match =>
        {
            string value = match.Groups["value"].Value.Trim().Trim('\'', '"');
            return bindings.TryGetValue(value, out string? mapped)
                ? match.Groups["prefix"].Value + mapped + ";" : match.Value;
        }));
    }

    private void RetireImage(ImageRegistration registration)
    {
        if (Environment.CurrentManagedThreadId != m_ownerThread)
            throw new InvalidOperationException("A UI image must be retired on its owner thread.");
        if (registration.retired)
            return;
        EnsureActive();
        using var source = new RmlUiUtf8String(registration.source);
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.UnregisterImage(
            m_runtime, Require(registration.context), (sbyte*)source.pointer, &result, &error), error);
        ThrowIfFailed(result, "retire imported UI image");
        registration.retired = true;
        m_images.Remove(registration.id);
    }

    private void RetireContextImages(UiContextHandle context)
    {
        List<ulong>? retired = null;
        foreach ((ulong id, ImageRegistration registration) in m_images)
        {
            if (registration.context != context)
                continue;
            registration.retired = true;
            (retired ??= []).Add(id);
        }
        if (retired is not null)
            foreach (ulong id in retired)
                m_images.Remove(id);
    }

    private sealed class ImageRegistration : IUiImageRegistration
    {
        private readonly RmlUiBackend m_owner;

        internal ImageRegistration(
            RmlUiBackend owner,
            UiContextHandle context,
            ulong id,
            string source,
            UiImageSource image
        ) {
            m_owner = owner;
            this.context = context;
            this.id = id;
            this.source = source;
            this.image = image;
        }

        /// <inheritdoc />
        public string source { get; }
        /// <inheritdoc />
        public void Dispose() => m_owner.RetireImage(this);

        internal UiContextHandle context { get; }
        internal ulong id { get; }
        internal UiImageSource image { get; }
        internal bool retired { get; set; }
    }
}
