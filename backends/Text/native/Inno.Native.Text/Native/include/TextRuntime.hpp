#pragma once

#include <cstdint>
#include <memory>

/// Reports a text operation; zero denotes an unexpected bridge failure.
enum class Result : std::uint32_t
{
    UnknownError = 0,
    Success = 1,
    InvalidArgument = 2,
    OutOfMemory = 3,
    InvalidFont = 4,
    InvalidHandle = 5,
    BackendError = 6,
    BufferTooSmall = 7
};

/// Supplies the caller's shaping direction without exposing a library-specific enum.
enum class Direction : std::uint32_t
{
    Automatic = 0,
    LeftToRight = 1,
    RightToLeft = 2,
    TopToBottom = 3,
    BottomToTop = 4
};

namespace Inno::Text::FontAdapter
{

/// Transfers a shaped glyph and its position in the caller's UTF-8 input.
struct Glyph
{
    std::uint32_t glyph_id;
    std::uint32_t cluster;
    float advance_x;
    float advance_y;
    float offset_x;
    float offset_y;
};

/// Transfers logical font measurements for the requested size.
struct Metrics
{
    float ascender;
    float descender;
    float line_height;
    float underline_position;
    float underline_thickness;
};

/// Describes a caller-owned, tightly packed 8-bit coverage buffer.
struct Bitmap
{
    std::int32_t width;
    std::int32_t height;
    std::int32_t bearing_x;
    std::int32_t bearing_y;
    float advance_x;
    std::uint32_t byte_length;
};

/// Owns one font library and all faces loaded into this session.
class Runtime
{
public:
    /// Initializes an isolated session; initialization failure throws before ownership is returned.
    Runtime();
    /// Releases faces before the underlying font library.
    ~Runtime();

    Runtime(const Runtime&) = delete;
    Runtime& operator=(const Runtime&) = delete;

    /// Copies font bytes and returns a session-local face handle on success.
    Result LoadFont(const std::uint8_t* data, std::uint32_t length, std::int32_t face_index,
        std::uint64_t* font_handle);
    /// Releases one face; a handle from another session is invalid.
    Result ReleaseFont(std::uint64_t font_handle);
    /// Writes scaled metrics into caller-owned storage.
    Result GetMetrics(std::uint64_t font_handle, float font_size, Metrics* metrics);
    /// Queries the glyph count when glyphs is null, or writes up to the supplied capacity.
    Result ShapeUtf8(std::uint64_t font_handle, const char* text, std::uint32_t text_length,
        float font_size, Direction direction, const char* language, const char* script,
        Glyph* glyphs, std::uint32_t glyph_capacity, std::uint32_t* glyph_count);
    /// Queries bitmap size when pixels is null, or writes coverage into caller-owned storage.
    Result RasterizeGlyph(std::uint64_t font_handle, std::uint32_t glyph_id, float font_size,
        std::uint8_t* pixels, std::uint32_t pixel_capacity, Bitmap* bitmap);
    /// Returns a borrowed, process-lifetime UTF-8 result description.
    static const char* ResultMessage(Result result);

private:
    struct Impl;
    std::unique_ptr<Impl> m_impl;
};

}
