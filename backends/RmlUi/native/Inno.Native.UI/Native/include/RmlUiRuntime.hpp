#pragma once

#include <cstdint>


/// Reports the result of an operation performed by the native RmlUi adapter.
enum class Result : std::uint32_t
{
    UnknownError = 0,
    Success = 1,
    InvalidArgument = 2,
    OutOfMemory = 3,
    InvalidHandle = 4,
    BackendError = 5,
    BufferTooSmall = 6,
    NotFound = 7
};

/// Identifies an input event emitted by a retained UI document.
enum class EventType : std::uint32_t
{
    Click = 0,
    Change = 1,
    Submit = 2,
    Focus = 3,
    Blur = 4,
    MouseEnter = 5,
    MouseLeave = 6
};

/// Describes an explicit document focus request independently of transport failures.
enum class FocusOutcome : std::uint32_t
{
    Focused = 0,
    Unchanged = 1,
    NotFound = 2,
    NotFocusable = 3
};

/// Identifies the typed value of a form control.
enum class ControlValueKind : std::uint32_t { None, Text, Boolean, Number, Selection };
/// Distinguishes value editing from a completed interaction.
enum class EditPhase : std::uint32_t { None, Editing, Committed };
/// Identifies the operation that produced a document event.
enum class EventOrigin : std::uint32_t { Backend, UserInput, Programmatic };

namespace Inno::UI::RmlUiAdapter
{

/// Stores one premultiplied UI vertex in backend-neutral transfer form.
struct Vertex
{
    float x;
    float y;
    float u;
    float v;
    std::uint32_t color;
};

/// Describes one retained mesh draw submitted during a UI frame.
struct DrawCommand
{
    std::uint64_t mesh_id;
    std::uint64_t texture_id;
    std::uint8_t scissor_enabled;
    std::int32_t scissor_x;
    std::int32_t scissor_y;
    std::int32_t scissor_width;
    std::int32_t scissor_height;
};

/// Summarizes the incremental resources and commands available for one frame.
struct FrameInfo
{
    std::uint64_t mesh_update_count;
    std::uint64_t released_mesh_count;
    std::uint64_t command_count;
    std::uint64_t texture_update_count;
    std::uint64_t released_texture_count;
};

/// Captures one element layout using context pixels and a column-vector transform.
struct ElementLayoutInfo
{
    std::uint32_t status;
    std::uint64_t revision;
    float content_x;
    float content_y;
    float content_width;
    float content_height;
    float border_x;
    float border_y;
    float border_width;
    float border_height;
    float clip_x;
    float clip_y;
    float clip_width;
    float clip_height;
    float m11;
    float m12;
    float m13;
    float m14;
    float m21;
    float m22;
    float m23;
    float m24;
    float m31;
    float m32;
    float m33;
    float m34;
    float m41;
    float m42;
    float m43;
    float m44;
};

/// Describes one retained mesh update.
struct MeshInfo
{
    std::uint64_t id;
    std::uint64_t revision;
    std::uint64_t vertex_count;
    std::uint64_t index_count;
};

/// Describes one retained RGBA texture update.
struct TextureInfo
{
    std::uint64_t id;
    std::uint64_t revision;
    std::int32_t width;
    std::int32_t height;
    std::uint64_t byte_length;
    std::uint64_t image_id;
};

/// Describes a typed control value without retaining a document or text pointer.
struct ControlValueInfo
{
    ControlValueKind kind;
    double number;
    std::uint8_t checked;
    std::uint64_t text_length;
};

/// Describes one queued UI event without exposing third-party event objects.
struct EventInfo
{
    EventType type;
    std::uint64_t document;
    std::uint64_t target_id_length;
    ControlValueInfo value;
    EditPhase phase;
    EventOrigin origin;
};

/// Owns one isolated RmlUi adapter session and its retained contexts.
class Runtime final
{
  public:
    /// Initializes an adapter session on the current thread.
    Runtime();

    /// Releases every retained context and the process-wide RmlUi reference.
    ~Runtime() noexcept;
    Result CreateContext(
        const char* name,
        std::int32_t width,
        std::int32_t height,
        float density,
        std::uint64_t& context
    );
    Result DestroyContext(std::uint64_t context);
    Result SetViewport(
        std::uint64_t context,
        std::int32_t width,
        std::int32_t height,
        float density
    );
    Result LoadDocument(
        std::uint64_t context,
        const char* markup,
        const char* source_url,
        std::uint64_t& document
    );
    Result ShowDocument(
        std::uint64_t context,
        std::uint64_t document
    );
    Result HideDocument(
        std::uint64_t context,
        std::uint64_t document
    );
    Result CloseDocument(
        std::uint64_t context,
        std::uint64_t document
    );
    Result FocusElement(
        std::uint64_t context,
        std::uint64_t document,
        const char* element_id,
        FocusOutcome& outcome
    );
    Result GetFocusedElement(
        std::uint64_t context,
        std::uint64_t& document,
        char* element_id,
        std::uint64_t capacity,
        std::uint64_t& length
    );
    Result ClearFocus(
        std::uint64_t context,
        std::uint8_t& cleared
    );
    /// Reads the active text control bounds in context pixels without refreshing layout.
    Result GetTextInputArea(
        std::uint64_t context,
        std::uint8_t& available,
        float& x,
        float& y,
        float& width,
        float& height
    );
    Result PrepareLayout(
        std::uint64_t context,
        std::uint64_t& revision
    );
    Result ReadLayout(
        std::uint64_t context,
        std::uint64_t document,
        const char* element_id,
        std::uint64_t revision,
        ElementLayoutInfo& layout
    );
    Result GetValue(
        std::uint64_t context,
        std::uint64_t document,
        const char* element_id,
        ControlValueInfo& value,
        char* text,
        std::uint64_t capacity
    );
    Result SetValue(
        std::uint64_t context,
        std::uint64_t document,
        const char* element_id,
        ControlValueKind kind,
        const char* text,
        double number,
        std::uint8_t checked,
        std::uint8_t& accepted
    );
    Result CopyEventValue(
        std::uint64_t context,
        std::uint64_t index,
        char* text,
        std::uint64_t capacity
    );
    Result SetInnerMarkup(
        std::uint64_t context,
        std::uint64_t document,
        const char* element_id,
        const char* markup,
        std::uint8_t& changed
    );
    Result SetAttribute(std::uint64_t context, std::uint64_t document, const char* element_id, const char* name, const char* value,
                        std::uint8_t& changed);
    Result SetClass(std::uint64_t context, std::uint64_t document, const char* element_id, const char* class_name, std::uint8_t active,
                    std::uint8_t& changed);
    Result LoadFont(
        const std::uint8_t* data,
        std::uint64_t data_length,
        const char* family,
        std::int32_t style,
        std::int32_t weight,
        std::uint8_t fallback
    );
    /// Registers a context-local image identity and dimensions without pixel data.
    Result RegisterImage(
        std::uint64_t context,
        const char* source,
        std::uint64_t image,
        int width,
        int height
    );
    /// Retires only the named context-local image and invalidates its cached native texture.
    Result UnregisterImage(
        std::uint64_t context,
        const char* source
    );

    Result RegisterTexture(
        std::uint64_t context,
        const char* source,
        std::int32_t width,
        std::int32_t height,
        std::uint8_t* pixels,
        std::uint64_t pixel_capacity
    );
    Result ProcessMouseMove(
        std::uint64_t context,
        std::int32_t x,
        std::int32_t y,
        std::int32_t modifiers
    );
    Result HasElementAtPoint(
        std::uint64_t context,
        std::int32_t x,
        std::int32_t y,
        std::uint8_t& hit
    );
    Result ProcessMouseButton(
        std::uint64_t context,
        std::int32_t button,
        std::uint8_t down,
        std::int32_t modifiers
    );
    Result ProcessMouseWheel(
        std::uint64_t context,
        float x,
        float y,
        std::int32_t modifiers
    );
    Result ProcessKey(
        std::uint64_t context,
        std::int32_t key,
        std::uint8_t down,
        std::int32_t modifiers
    );
    Result ProcessText(
        std::uint64_t context,
        const char* text
    );
    /// Applies one ordered composition transition to the currently focused text control.
    Result ProcessComposition(
        std::uint64_t context,
        std::uint64_t operation,
        std::uint32_t phase,
        const char* text,
        std::int32_t selection_start,
        std::int32_t selection_length
    );
    /// Retires provisional text when the input route no longer owns its composition.
    Result SynchronizeComposition(
        std::uint64_t context,
        std::uint64_t operation
    );

    Result GetTime(
        std::uint64_t context,
        double& seconds,
        std::uint8_t& paused
    );
    Result SetTimePaused(
        std::uint64_t context,
        std::uint8_t paused
    );
    Result AdvanceTime(
        std::uint64_t context,
        double seconds
    );
    Result RefreshLayout(std::uint64_t context);
    Result Update(std::uint64_t context);
    Result Render(
        std::uint64_t context,
        FrameInfo& frame
    );
    Result GetMeshInfo(
        std::uint64_t context,
        std::uint64_t index,
        MeshInfo& mesh
    );
    Result CopyMeshVertices(
        std::uint64_t context,
        std::uint64_t mesh,
        Vertex* vertices,
        std::uint64_t vertex_capacity
    );
    Result CopyMeshIndices(
        std::uint64_t context,
        std::uint64_t mesh,
        std::uint32_t* indices,
        std::uint64_t index_capacity
    );
    Result CopyCommands(
        std::uint64_t context,
        DrawCommand* commands,
        std::uint64_t command_capacity
    );
    Result GetTextureInfo(
        std::uint64_t context,
        std::uint64_t index,
        TextureInfo& texture
    );
    Result CopyTexturePixels(
        std::uint64_t context,
        std::uint64_t texture,
        std::uint8_t* pixels,
        std::uint64_t pixel_capacity
    );
    Result CopyReleasedTextures(
        std::uint64_t context,
        std::uint64_t* textures,
        std::uint64_t texture_capacity
    );
    Result CopyReleasedMeshes(
        std::uint64_t context,
        std::uint64_t* meshes,
        std::uint64_t mesh_capacity
    );
    Result FinishFrame(std::uint64_t context);
    Result GetEventCount(
        std::uint64_t context,
        std::uint64_t& count
    );
    Result GetEventInfo(
        std::uint64_t context,
        std::uint64_t index,
        EventInfo& event_info
    );
    Result CopyEventTargetId(
        std::uint64_t context,
        std::uint64_t index,
        char* target_id,
        std::uint64_t target_capacity
    );
    Result ClearEvents(std::uint64_t context);

  private:
    void* m_state;
};

}
