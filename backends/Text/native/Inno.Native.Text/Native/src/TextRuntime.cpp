#include "TextRuntime.hpp"

#include <algorithm>
#include <cmath>
#include <cstring>
#include <limits>
#include <memory>
#include <new>
#include <stdexcept>
#include <unordered_map>
#include <vector>

#include <ft2build.h>
#include FT_FREETYPE_H
#include <hb-ft.h>
#include <hb.h>

namespace Inno::Text::FontAdapter
{

struct Face
{
    std::vector<uint8_t> bytes;
    FT_Face face = nullptr;
    hb_font_t* font = nullptr;

    ~Face()
    {
        if (font)
            hb_font_destroy(font);
        if (face)
            FT_Done_Face(face);
    }
};

struct Runtime::Impl
{
    FT_Library library = nullptr;
    uint64_t next_handle = 1;
    std::unordered_map<uint64_t, std::unique_ptr<Face>> faces;

    ~Impl()
    {
        faces.clear();
        if (library)
            FT_Done_FreeType(library);
    }

    Face* FindFace(uint64_t handle)
    {
        const auto iterator = faces.find(handle);
        return iterator == faces.end() ? nullptr : iterator->second.get();
    }
};

static bool set_size(Face* face, float font_size)
{
    if (!face || !std::isfinite(font_size) || font_size <= 0.f)
        return false;
    const double scaled_size = static_cast<double>(font_size) * 64.;
    if (scaled_size >= static_cast<double>(std::numeric_limits<FT_F26Dot6>::max()))
        return false;
    const auto size_26_6 = static_cast<FT_F26Dot6>(std::llround(scaled_size));
    if (FT_Set_Char_Size(face->face, 0, size_26_6, 72, 72) != 0)
        return false;
    hb_ft_font_changed(face->font);
    return true;
}

static hb_direction_t resolve_direction(Direction direction)
{
    switch (direction)
    {
    case Direction::LeftToRight:
        return HB_DIRECTION_LTR;
    case Direction::RightToLeft:
        return HB_DIRECTION_RTL;
    case Direction::TopToBottom:
        return HB_DIRECTION_TTB;
    case Direction::BottomToTop:
        return HB_DIRECTION_BTT;
    default:
        return HB_DIRECTION_INVALID;
    }
}

Runtime::Runtime() : m_impl(std::make_unique<Impl>())
{
    if (FT_Init_FreeType(&m_impl->library) != 0)
        throw std::runtime_error("The text adapter could not initialize its font library.");
}

Runtime::~Runtime() = default;

Result Runtime::LoadFont(const uint8_t* data, uint32_t length, int32_t face_index,
                                     uint64_t* font_handle)
{
    if (!data || length == 0 || length > static_cast<uint64_t>(std::numeric_limits<FT_Long>::max())
        || face_index < 0 || !font_handle)
        return Result::InvalidArgument;
    *font_handle = 0;
    std::unique_ptr<Face> value(new (std::nothrow) Face());
    if (!value)
        return Result::OutOfMemory;
    try
    {
        value->bytes.assign(data, data + length);
    }
    catch (...)
    {
        return Result::OutOfMemory;
    }
    if (FT_New_Memory_Face(m_impl->library, value->bytes.data(), static_cast<FT_Long>(value->bytes.size()), face_index, &value->face) != 0)
        return Result::InvalidFont;
    value->font = hb_ft_font_create_referenced(value->face);
    if (!value->font)
        return Result::BackendError;
    if (m_impl->next_handle == 0)
        return Result::BackendError;
    const uint64_t handle = m_impl->next_handle++;
    try
    {
        m_impl->faces.emplace(handle, std::move(value));
    }
    catch (...)
    {
        return Result::OutOfMemory;
    }
    *font_handle = handle;
    return Result::Success;
}

Result Runtime::ReleaseFont(uint64_t font_handle)
{
    if (font_handle == 0)
        return Result::InvalidArgument;
    return m_impl->faces.erase(font_handle) == 1 ? Result::Success : Result::InvalidHandle;
}

Result Runtime::GetMetrics(uint64_t font_handle, float font_size, Metrics* metrics)
{
    if (!metrics)
        return Result::InvalidArgument;
    Face* face = m_impl->FindFace(font_handle);
    if (!face)
        return Result::InvalidHandle;
    if (!set_size(face, font_size))
        return Result::InvalidArgument;
    const FT_Size_Metrics& value = face->face->size->metrics;
    metrics->ascender = value.ascender / 64.f;
    metrics->descender = value.descender / 64.f;
    metrics->line_height = value.height / 64.f;
    const float scale = face->face->units_per_EM == 0 ? 0.f : font_size / static_cast<float>(face->face->units_per_EM);
    metrics->underline_position = face->face->underline_position * scale;
    metrics->underline_thickness = std::max(1.f, face->face->underline_thickness * scale);
    return Result::Success;
}

Result Runtime::ShapeUtf8(uint64_t font_handle, const char* text, uint32_t text_length,
                                      float font_size, Direction direction, const char* language, const char* script,
                                      Glyph* glyphs, uint32_t glyph_capacity, uint32_t* glyph_count)
{
    if (!text || text_length > static_cast<uint32_t>(INT32_MAX) || !glyph_count || (glyph_capacity > 0 && !glyphs))
        return Result::InvalidArgument;
    Face* face = m_impl->FindFace(font_handle);
    if (!face)
        return Result::InvalidHandle;
    if (!set_size(face, font_size))
        return Result::InvalidArgument;

    std::unique_ptr<hb_buffer_t, decltype(&hb_buffer_destroy)> buffer(hb_buffer_create(), hb_buffer_destroy);
    if (!buffer)
        return Result::OutOfMemory;
    hb_buffer_add_utf8(buffer.get(), text, static_cast<int>(text_length), 0, static_cast<int>(text_length));
    hb_direction_t resolved = resolve_direction(direction);
    if (resolved != HB_DIRECTION_INVALID)
        hb_buffer_set_direction(buffer.get(), resolved);
    if (language && language[0] != '\0')
        hb_buffer_set_language(buffer.get(), hb_language_from_string(language, -1));
    if (script && script[0] != '\0')
        hb_buffer_set_script(buffer.get(), hb_script_from_string(script, -1));
    hb_buffer_guess_segment_properties(buffer.get());
    hb_shape(face->font, buffer.get(), nullptr, 0);

    unsigned int count = 0;
    hb_glyph_info_t* infos = hb_buffer_get_glyph_infos(buffer.get(), &count);
    hb_glyph_position_t* positions = hb_buffer_get_glyph_positions(buffer.get(), &count);
    *glyph_count = count;
    if (glyph_capacity < count)
    {
        return glyphs ? Result::BufferTooSmall : Result::Success;
    }
    for (unsigned int index = 0; index < count; ++index)
    {
        glyphs[index] = {
            infos[index].codepoint,
            infos[index].cluster,
            positions[index].x_advance / 64.f,
            positions[index].y_advance / 64.f,
            positions[index].x_offset / 64.f,
            positions[index].y_offset / 64.f
        };
    }
    return Result::Success;
}

Result Runtime::RasterizeGlyph(uint64_t font_handle, uint32_t glyph_id, float font_size,
                                           uint8_t* pixels, uint32_t pixel_capacity, Bitmap* bitmap)
{
    if (!bitmap || (pixel_capacity > 0 && !pixels))
        return Result::InvalidArgument;
    Face* face = m_impl->FindFace(font_handle);
    if (!face)
        return Result::InvalidHandle;
    if (!set_size(face, font_size))
        return Result::InvalidArgument;
    if (FT_Load_Glyph(face->face, glyph_id, FT_LOAD_DEFAULT) != 0 || FT_Render_Glyph(face->face->glyph, FT_RENDER_MODE_NORMAL) != 0)
        return Result::BackendError;

    const FT_GlyphSlot slot = face->face->glyph;
    const FT_Bitmap& source = slot->bitmap;
    const uint64_t length = static_cast<uint64_t>(source.width) * source.rows;
    if (length > UINT32_MAX || source.width > INT32_MAX || source.rows > INT32_MAX)
        return Result::BufferTooSmall;
    if (length > 0 && source.pixel_mode != FT_PIXEL_MODE_GRAY && source.pixel_mode != FT_PIXEL_MODE_MONO)
        return Result::BackendError;
    bitmap->width = static_cast<int32_t>(source.width);
    bitmap->height = static_cast<int32_t>(source.rows);
    bitmap->bearing_x = slot->bitmap_left;
    bitmap->bearing_y = slot->bitmap_top;
    bitmap->advance_x = slot->advance.x / 64.f;
    bitmap->byte_length = static_cast<uint32_t>(length);
    if (!pixels)
        return Result::Success;
    if (pixel_capacity < length)
        return Result::BufferTooSmall;

    for (unsigned int row = 0; row < source.rows; ++row)
    {
        const uint8_t* source_row = source.pitch >= 0
            ? source.buffer + static_cast<size_t>(row) * source.pitch
            : source.buffer + static_cast<size_t>(source.rows - row - 1) * static_cast<size_t>(-source.pitch);
        uint8_t* target_row = pixels + static_cast<size_t>(row) * source.width;
        if (source.pixel_mode == FT_PIXEL_MODE_GRAY)
        {
            std::memcpy(target_row, source_row, source.width);
        }
        else if (source.pixel_mode == FT_PIXEL_MODE_MONO)
        {
            for (unsigned int column = 0; column < source.width; ++column)
                target_row[column] = (source_row[column >> 3] & (0x80 >> (column & 7))) ? 255 : 0;
        }
    }
    return Result::Success;
}

const char* Runtime::ResultMessage(Result result)
{
    switch (result)
    {
    case Result::Success:
        return "success";
    case Result::InvalidArgument:
        return "invalid argument";
    case Result::OutOfMemory:
        return "out of memory";
    case Result::InvalidFont:
        return "invalid font";
    case Result::InvalidHandle:
        return "invalid handle";
    case Result::BackendError:
        return "backend error";
    case Result::BufferTooSmall:
        return "buffer too small";
    default:
        return "unknown error";
    }
}

}
