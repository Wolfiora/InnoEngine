#include "RmlUiRuntime.hpp"

#include "RmlUiEventListener.hpp"
#include "RmlUiControlValue.hpp"
#include "RmlUiProcessHost.hpp"
#include "RmlUiRenderInterface.hpp"

#include <RmlUi/Core.h>
#include <RmlUi/Core/Element.h>
#include <RmlUi/Core/ElementDocument.h>
#include <RmlUi/Core/Elements/ElementFormControlInput.h>
#include <RmlUi/Core/Elements/ElementFormControlTextArea.h>
#include <RmlUi/Core/Event.h>
#include <RmlUi/Core/ElementUtilities.h>
#include <RmlUi/Core/RenderManager.h>
#include <RmlUi/Core/Input.h>

#include <algorithm>
#include <cmath>
#include <chrono>
#include <cstdint>
#include <cstring>
#include <memory>
#include <stdexcept>
#include <string>
#include <unordered_map>
#include <utility>
#include <vector>

namespace Inno::UI::RmlUiAdapter
{
namespace
{


constexpr const char* C_CONTEXT_EVENTS[] = {"click", "change", "submit", "focus", "blur", "mouseover", "mouseout"};

struct QueuedEvent
{
    EventType type;
    std::uint64_t document;
    std::string target_id;
    RmlUiControlValue value;
    EditPhase phase;
    EventOrigin origin;
};

void RequireEditableFont(Rml::Element* element)
{
    while (element)
    {
        bool editable = dynamic_cast<Rml::ElementFormControlTextArea*>(element) != nullptr;
        if (dynamic_cast<Rml::ElementFormControlInput*>(element))
        {
            const auto type = element->GetAttribute<Rml::String>("type", "text");
            editable = type == "text" || type == "password";
        }
        if (editable)
        {
            if (element->GetFontFaceHandle() == 0)
                throw std::runtime_error("The RmlUi text control '" + element->GetId() +
                    "' has no resolved font face. Register its declared font and refresh layout before editing.");
            return;
        }
        element = element->GetParentNode();
    }
}

EventType MapEvent(const Rml::String& type) noexcept
{
    if (type == "change")
        return EventType::Change;
    if (type == "submit")
        return EventType::Submit;
    if (type == "focus")
        return EventType::Focus;
    if (type == "blur")
        return EventType::Blur;
    if (type == "mouseover")
        return EventType::MouseEnter;
    if (type == "mouseout")
        return EventType::MouseLeave;
    return EventType::Click;
}

Rml::Input::KeyIdentifier MapKey(int key) noexcept
{
    using namespace Rml::Input;
    if (key >= 65 && key <= 90)
        return static_cast<KeyIdentifier>(KI_A + key - 65);
    if (key >= 48 && key <= 57)
        return static_cast<KeyIdentifier>(KI_0 + key - 48);
    if (key >= 96 && key <= 105)
        return static_cast<KeyIdentifier>(KI_NUMPAD0 + key - 96);
    if (key >= 112 && key <= 123)
        return static_cast<KeyIdentifier>(KI_F1 + key - 112);
    switch (key)
    {
    case 8:
        return KI_BACK;
    case 9:
        return KI_TAB;
    case 13:
        return KI_RETURN;
    case 20:
        return KI_CAPITAL;
    case 27:
        return KI_ESCAPE;
    case 32:
        return KI_SPACE;
    case 33:
        return KI_PRIOR;
    case 34:
        return KI_NEXT;
    case 35:
        return KI_END;
    case 36:
        return KI_HOME;
    case 37:
        return KI_LEFT;
    case 38:
        return KI_UP;
    case 39:
        return KI_RIGHT;
    case 40:
        return KI_DOWN;
    case 45:
        return KI_INSERT;
    case 46:
        return KI_DELETE;
    case 91:
        return KI_LWIN;
    case 92:
        return KI_RWIN;
    case 144:
        return KI_NUMLOCK;
    case 145:
        return KI_SCROLL;
    case 160:
        return KI_LSHIFT;
    case 161:
        return KI_RSHIFT;
    case 162:
        return KI_LCONTROL;
    case 163:
        return KI_RCONTROL;
    case 164:
        return KI_LMENU;
    case 165:
        return KI_RMENU;
    case 186:
        return KI_OEM_1;
    case 187:
        return KI_OEM_PLUS;
    case 188:
        return KI_OEM_COMMA;
    case 189:
        return KI_OEM_MINUS;
    case 190:
        return KI_OEM_PERIOD;
    case 191:
        return KI_OEM_2;
    case 192:
        return KI_OEM_3;
    case 219:
        return KI_OEM_4;
    case 220:
        return KI_OEM_5;
    case 221:
        return KI_OEM_6;
    case 222:
        return KI_OEM_7;
    default:
        return KI_UNKNOWN;
    }
}

int MapModifiers(int modifiers) noexcept
{
    int result = 0;
    if ((modifiers & 2) != 0)
        result |= Rml::Input::KM_CTRL;
    if ((modifiers & 4) != 0)
        result |= Rml::Input::KM_SHIFT;
    if ((modifiers & 1) != 0)
        result |= Rml::Input::KM_ALT;
    if ((modifiers & 8) != 0)
        result |= Rml::Input::KM_META;
    return result;
}

}

struct RmlUiRuntimeState
{
    struct ContextState final : Inno::UI::RmlUiAdapter::RmlUiEventSink
    {
        std::uint64_t id = 0;
        std::string name;
        Rml::Context* context = nullptr;
        std::unique_ptr<Inno::UI::RmlUiAdapter::RmlUiRenderInterface> renderer;
        std::unique_ptr<Inno::UI::RmlUiAdapter::RmlUiEventListener> listener;
        std::unordered_map<std::uint64_t, Rml::ElementDocument*> documents;
        std::unordered_map<Rml::ElementDocument*, std::uint64_t> document_ids;
        std::vector<QueuedEvent> events;
        bool frame_prepared = false;
        std::uint64_t layout_revision = 0;
        double elapsed = 0;
        bool paused = false;
        std::chrono::steady_clock::time_point last_update = std::chrono::steady_clock::now();
        EventOrigin event_origin = EventOrigin::Backend;
        bool suppress_value_events = false;

        void ProcessRmlUiEvent(Rml::Event& event) override
        {
            Rml::Element* target = event.GetTargetElement();
            if (!target)
                return;
            Rml::ElementDocument* document = target->GetOwnerDocument();
            auto iterator = document_ids.find(document);
            if (iterator == document_ids.end())
                return;
            const auto type = MapEvent(event.GetType());
            if (suppress_value_events && (type == EventType::Change || type == EventType::Submit))
                return;
            auto value = RmlUiControlValue::Read(target);
            EditPhase phase = EditPhase::None;
            if (type == EventType::Change)
            {
                const bool continuous = value.info.kind == ControlValueKind::Text || value.info.kind == ControlValueKind::Number;
                phase = continuous && !event.GetParameter<bool>("linebreak", false) ? EditPhase::Editing : EditPhase::Committed;
            }
            else if (type == EventType::Submit || (type == EventType::Blur && value.info.kind != ControlValueKind::None))
                phase = EditPhase::Committed;
            events.push_back({type, iterator->second, target->GetId(), std::move(value), phase, event_origin});
        }
    };

    struct EventScope
    {
        ContextState& state;
        EventOrigin previous_origin;
        bool previous_suppression;

        EventScope(
            ContextState& state,
            EventOrigin origin,
            bool suppress_values = false
        ) : state(state), previous_origin(state.event_origin), previous_suppression(state.suppress_value_events) {
            state.event_origin = origin;
            state.suppress_value_events = suppress_values;
        }
        ~EventScope()
        {
            state.event_origin = previous_origin;
            state.suppress_value_events = previous_suppression;
        }
    };

    std::unordered_map<std::uint64_t, std::unique_ptr<ContextState>> contexts;

    ContextState* Find(std::uint64_t context) noexcept
    {
        auto iterator = contexts.find(context);
        return iterator == contexts.end() ? nullptr : iterator->second.get();
    }

    static Rml::ElementDocument* FindDocument(ContextState* state, std::uint64_t document) noexcept
    {
        if (!state || document == 0)
            return nullptr;
        auto iterator = state->documents.find(document);
        return iterator == state->documents.end() ? nullptr : iterator->second;
    }

    static Rml::Element* FindElement(ContextState* state, std::uint64_t document, const char* id) noexcept
    {
        Rml::ElementDocument* value = FindDocument(state, document);
        return value && id ? value->GetElementById(id) : nullptr;
    }
};

RmlUiRuntimeState* GetRuntimeState(void* state) noexcept
{
    return static_cast<RmlUiRuntimeState*>(state);
}

Runtime::Runtime() : m_state(nullptr)
{
    std::unique_ptr<RmlUiRuntimeState> implementation = std::make_unique<RmlUiRuntimeState>();
    if (!Inno::UI::RmlUiAdapter::AcquireProcessHost())
        throw std::runtime_error("The RmlUi process host is already owned by another thread or failed to initialize.");
    m_state = implementation.release();
}

Runtime::~Runtime() noexcept
{
    if (!m_state)
        return;
    auto* state = static_cast<RmlUiRuntimeState*>(m_state);
    while (!state->contexts.empty())
        DestroyContext(state->contexts.begin()->first);
    delete state;
    m_state = nullptr;
    Inno::UI::RmlUiAdapter::ReleaseProcessHost();
}

Result Runtime::CreateContext(
    const char* name,
    int width,
    int height,
    float density,
    std::uint64_t& context
) {
    context = 0;
    auto* implementation = static_cast<RmlUiRuntimeState*>(m_state);
    if (!implementation || !name || name[0] == '\0' || width <= 0 || height <= 0 || !std::isfinite(density) || density <= 0.f)
    {
        return Result::InvalidArgument;
    }

    auto state = std::make_unique<RmlUiRuntimeState::ContextState>();
    state->renderer = std::make_unique<Inno::UI::RmlUiAdapter::RmlUiRenderInterface>();
    state->id = Inno::UI::RmlUiAdapter::AllocateContextId();
    if (state->id == 0)
        return Result::BackendError;
    state->name =
        std::string("inno-ui-") + std::to_string(reinterpret_cast<std::uintptr_t>(this)) + "-" + std::to_string(state->id) + "-" + name;
    state->context = Rml::CreateContext(state->name, {width, height}, state->renderer.get());
    if (!state->context)
        return Result::BackendError;
    state->context->SetDensityIndependentPixelRatio(density);
    // Initialize the context root before documents use independent layout refreshes.
    // No document, input owner or event listener exists at this construction boundary.
    {
        RmlUiTimeScope time_scope(state->elapsed);
        if (!state->context->Update())
        {
            Rml::RemoveContext(state->name);
            return Result::BackendError;
        }
    }
    state->listener = std::make_unique<Inno::UI::RmlUiAdapter::RmlUiEventListener>(*state);
    for (const char* event_name : C_CONTEXT_EVENTS)
        state->context->AddEventListener(event_name, state->listener.get(), true);

    const std::uint64_t id = state->id;
    implementation->contexts.emplace(id, std::move(state));
    context = id;
    return Result::Success;
}

Result Runtime::DestroyContext(std::uint64_t context)
{
    auto* implementation = static_cast<RmlUiRuntimeState*>(m_state);
    if (!implementation || context == 0)
        return Result::InvalidArgument;
    auto iterator = implementation->contexts.find(context);
    if (iterator == implementation->contexts.end())
        return Result::InvalidHandle;

    RmlUiRuntimeState::ContextState& state = *iterator->second;
    RmlUiTimeScope time_scope(state.elapsed);
    for (const char* event_name : C_CONTEXT_EVENTS)
        state.context->RemoveEventListener(event_name, state.listener.get(), true);
    Rml::RemoveContext(state.name);
    Rml::ReleaseTextures(state.renderer.get());
    Rml::ReleaseCompiledGeometry(state.renderer.get());
    state.renderer->Retire();
    Inno::UI::RmlUiAdapter::RetainRenderInterface(std::move(state.renderer));
    implementation->contexts.erase(iterator);
    return Result::Success;
}

Result Runtime::SetViewport(
    std::uint64_t context,
    int width,
    int height,
    float density
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    if (width <= 0 || height <= 0 || !std::isfinite(density) || density <= 0.f)
        return Result::InvalidArgument;
    state->context->SetDimensions({width, height});
    state->context->SetDensityIndependentPixelRatio(density);
    return Result::Success;
}

Result Runtime::LoadDocument(
    std::uint64_t context,
    const char* markup,
    const char* source_url,
    std::uint64_t& document
) {
    document = 0;
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    if (!markup || !source_url)
        return Result::InvalidArgument;
    Rml::ElementDocument* value = state->context->LoadDocumentFromMemory(markup, source_url);
    if (!value)
        return Result::BackendError;
    const auto& style = value->GetComputedValues();
    if ((style.width().type == Rml::Style::Width::Auto && !value->SetProperty("width", "100%")) ||
        (style.height().type == Rml::Style::Height::Auto && !value->SetProperty("height", "100%")))
    {
        value->Close();
        return Result::BackendError;
    }
    const std::uint64_t id = Inno::UI::RmlUiAdapter::AllocateDocumentId();
    if (id == 0)
    {
        value->Close();
        return Result::BackendError;
    }
    state->documents.emplace(id, value);
    state->document_ids.emplace(value, id);
    document = id;
    return Result::Success;
}

Result Runtime::ShowDocument(
    std::uint64_t context,
    std::uint64_t document
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    Rml::ElementDocument* value = RmlUiRuntimeState::FindDocument(state, document);
    if (!value)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
    value->Show();
    return Result::Success;
}

Result Runtime::HideDocument(
    std::uint64_t context,
    std::uint64_t document
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    Rml::ElementDocument* value = RmlUiRuntimeState::FindDocument(state, document);
    if (!value)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
    value->Hide();
    return Result::Success;
}

Result Runtime::CloseDocument(
    std::uint64_t context,
    std::uint64_t document
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    Rml::ElementDocument* value = RmlUiRuntimeState::FindDocument(state, document);
    if (!value)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    state->document_ids.erase(value);
    state->documents.erase(document);
    state->events.erase(std::remove_if(state->events.begin(), state->events.end(), [document](const QueuedEvent& event) {
        return event.document == document;
    }), state->events.end());
    value->Close();
    return Result::Success;
}

Result Runtime::FocusElement(
    std::uint64_t context,
    std::uint64_t document,
    const char* element_id,
    FocusOutcome& outcome
) {
    outcome = FocusOutcome::NotFound;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    auto* owner = RmlUiRuntimeState::FindDocument(state, document);
    if (!owner)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
    if (!element_id || !*element_id)
        return Result::InvalidArgument;
    auto* element = owner->GetElementById(element_id);
    if (!element)
        return Result::Success;
    if (!owner->IsVisible() || !element->IsVisible(true) || element->IsPseudoClassSet("disabled") || element->HasAttribute("disabled"))
    {
        outcome = FocusOutcome::NotFocusable;
        return Result::Success;
    }
    if (state->context->GetFocusElement() == element)
    {
        outcome = FocusOutcome::Unchanged;
        return Result::Success;
    }
    outcome = element->Focus(true) ? FocusOutcome::Focused : FocusOutcome::NotFocusable;
    return Result::Success;
}

Result Runtime::GetFocusedElement(
    std::uint64_t context,
    std::uint64_t& document,
    char* element_id,
    std::uint64_t capacity,
    std::uint64_t& length
) {
    document = 0;
    length = 0;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    auto* element = state->context->GetFocusElement();
    if (!element)
        return Result::Success;
    auto* owner = element->GetOwnerDocument();
    auto iterator = state->document_ids.find(owner);
    if (iterator == state->document_ids.end() || element == owner || !owner->IsVisible())
        return Result::Success;
    document = iterator->second;
    const auto& id = element->GetId();
    length = id.size();
    if (capacity < length)
        return Result::BufferTooSmall;
    if (length != 0)
    {
        if (!element_id)
            return Result::InvalidArgument;
        std::memcpy(element_id, id.data(), static_cast<std::size_t>(length));
    }
    return Result::Success;
}

Result Runtime::ClearFocus(
    std::uint64_t context,
    std::uint8_t& cleared
) {
    cleared = 0;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
    auto* element = state->context->GetFocusElement();
    auto* document = element ? element->GetOwnerDocument() : nullptr;
    cleared = !document || element == document || document->Focus() ? 1 : 0;
    return Result::Success;
}

Result Runtime::PrepareLayout(
    std::uint64_t context,
    std::uint64_t& revision
) {
    revision = 0;
    const Result refreshed = RefreshLayout(context);
    if (refreshed != Result::Success)
        return refreshed;
    FrameInfo frame{};
    const Result rendered = Render(context, frame);
    if (rendered == Result::Success)
        revision = GetRuntimeState(m_state)->Find(context)->layout_revision;
    return rendered;
}

Result Runtime::ReadLayout(
    std::uint64_t context,
    std::uint64_t document,
    const char* element_id,
    std::uint64_t revision,
    ElementLayoutInfo& layout
) {
    layout = {};
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state || !RmlUiRuntimeState::FindDocument(state, document))
        return Result::InvalidHandle;
    if (!state->frame_prepared || revision != state->layout_revision)
        return Result::InvalidArgument;
    if (!element_id)
        return Result::InvalidArgument;
    layout.revision = revision;
    auto* element = RmlUiRuntimeState::FindElement(state, document, element_id);
    if (!element)
        return Result::Success;
    layout.status = 1;
    if (!element->IsVisible(true))
        return Result::Success;
    Rml::Rectanglef border;
    Rml::Rectanglef content;
    if (!Rml::ElementUtilities::GetBoundingBox(border, element, Rml::BoxArea::Border) ||
        !Rml::ElementUtilities::GetBoundingBox(content, element, Rml::BoxArea::Content))
        return Result::BackendError;
    layout.status = 2;
    const auto content_position = content.Position();
    const auto content_size = content.Size();
    const auto border_position = border.Position();
    const auto border_size = border.Size();
    layout.content_x = content_position.x; layout.content_y = content_position.y;
    layout.content_width = content_size.x; layout.content_height = content_size.y;
    layout.border_x = border_position.x; layout.border_y = border_position.y;
    layout.border_width = border_size.x; layout.border_height = border_size.y;
    const auto dimensions = state->context->GetDimensions();
    Rml::Rectanglei clip = Rml::Rectanglei::FromPositionSize({0, 0}, dimensions);
    Rml::Rectanglei element_clip;
    Rml::ClipMaskGeometryList clip_masks;
    if (Rml::ElementUtilities::GetClippingRegion(element, element_clip, &clip_masks))
        clip = clip.Intersect(element_clip);
    layout.clip_x = static_cast<float>(clip.Position().x); layout.clip_y = static_cast<float>(clip.Position().y);
    layout.clip_width = static_cast<float>(clip.Size().x); layout.clip_height = static_cast<float>(clip.Size().y);
    auto& manager = state->context->GetRenderManager();
    const Rml::Matrix4f previous = manager.GetState().transform;
    if (!Rml::ElementUtilities::ApplyTransform(*element))
        return Result::BackendError;
    const Rml::Matrix4f transform = manager.GetState().transform;
    manager.SetTransform(&previous);
    const auto offset = element->GetAbsoluteOffset(Rml::BoxArea::Border);
    // The public accumulated transform acts on context coordinates. Include the local border origin.
    const auto translated = transform * Rml::Vector4f(offset.x, offset.y, 0.f, 1.f);
    layout.m11 = transform.GetRow(0)[0];
    layout.m12 = transform.GetRow(0)[1];
    layout.m13 = transform.GetRow(0)[2];
    layout.m14 = translated.x;
    layout.m21 = transform.GetRow(1)[0];
    layout.m22 = transform.GetRow(1)[1];
    layout.m23 = transform.GetRow(1)[2];
    layout.m24 = translated.y;
    layout.m31 = transform.GetRow(2)[0];
    layout.m32 = transform.GetRow(2)[1];
    layout.m33 = transform.GetRow(2)[2];
    layout.m34 = translated.z;
    layout.m41 = transform.GetRow(3)[0];
    layout.m42 = transform.GetRow(3)[1];
    layout.m43 = transform.GetRow(3)[2];
    layout.m44 = translated.w;
    return Result::Success;
}

Result Runtime::GetValue(
    std::uint64_t context,
    std::uint64_t document,
    const char* element_id,
    ControlValueInfo& value,
    char* text,
    std::uint64_t capacity
) {
    value = {};
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    auto* owner = RmlUiRuntimeState::FindDocument(state, document);
    if (!owner)
        return Result::InvalidHandle;
    if (!element_id || !*element_id)
        return Result::InvalidArgument;
    const auto captured = RmlUiControlValue::Read(owner->GetElementById(element_id));
    value = captured.info;
    return captured.CopyText(text, capacity);
}

Result Runtime::SetValue(
    std::uint64_t context,
    std::uint64_t document,
    const char* element_id,
    ControlValueKind kind,
    const char* text,
    double number,
    std::uint8_t checked,
    std::uint8_t& accepted
) {
    accepted = 0;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    auto* owner = RmlUiRuntimeState::FindDocument(state, document);
    if (!owner)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    if (!element_id || !*element_id)
        return Result::InvalidArgument;
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic, true);
    bool result = false;
    const auto status = RmlUiControlValue::Write(owner->GetElementById(element_id), kind, text, number, checked != 0, result);
    accepted = result ? 1 : 0;
    return status;
}

Result Runtime::CopyEventValue(
    std::uint64_t context,
    std::uint64_t index,
    char* text,
    std::uint64_t capacity
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    if (index >= state->events.size())
        return Result::NotFound;
    return state->events[static_cast<std::size_t>(index)].value.CopyText(text, capacity);
}

Result Runtime::SetInnerMarkup(std::uint64_t context, std::uint64_t document, const char* element_id, const char* markup,
                               std::uint8_t& changed)
{
    changed = 0;
    if (!markup)
        return Result::InvalidArgument;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    Rml::Element* element = RmlUiRuntimeState::FindElement(state, document, element_id);
    if (element)
    {
        state->frame_prepared = false;
        RmlUiTimeScope time_scope(state->elapsed);
        RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
        element->SetInnerRML(markup);
        changed = 1;
    }
    return Result::Success;
}

Result Runtime::SetAttribute(std::uint64_t context, std::uint64_t document, const char* element_id, const char* name, const char* value,
                             std::uint8_t& changed)
{
    changed = 0;
    if (!name || !value)
        return Result::InvalidArgument;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    Rml::Element* element = RmlUiRuntimeState::FindElement(state, document, element_id);
    if (element)
    {
        state->frame_prepared = false;
        RmlUiTimeScope time_scope(state->elapsed);
        RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
        element->SetAttribute(name, Rml::String(value));
        changed = 1;
    }
    return Result::Success;
}

Result Runtime::SetClass(std::uint64_t context, std::uint64_t document, const char* element_id, const char* class_name, std::uint8_t active,
                         std::uint8_t& changed)
{
    changed = 0;
    if (!class_name)
        return Result::InvalidArgument;
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    Rml::Element* element = RmlUiRuntimeState::FindElement(state, document, element_id);
    if (element)
    {
        state->frame_prepared = false;
        RmlUiTimeScope time_scope(state->elapsed);
        RmlUiRuntimeState::EventScope events(*state, EventOrigin::Programmatic);
        element->SetClass(class_name, active != 0);
        changed = 1;
    }
    return Result::Success;
}

Result Runtime::LoadFont(
    const std::uint8_t* data,
    std::uint64_t data_length,
    const char* family,
    int style,
    int weight,
    std::uint8_t fallback
) {
    if (!m_state)
        return Result::InvalidHandle;
    return Inno::UI::RmlUiAdapter::LoadFont(data, data_length,
                                            family, style, weight, fallback != 0);
}

Result Runtime::RegisterImage(
    std::uint64_t context,
    const char* source,
    std::uint64_t image,
    int width,
    int height
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    Result result = state->renderer->RegisterImage(source, image, width, height);
    if (result == Result::Success)
        state->frame_prepared = false;
    return result;
}

Result Runtime::UnregisterImage(
    std::uint64_t context,
    const char* source
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    Result result = state->renderer->UnregisterImage(source);
    if (result == Result::Success)
        state->frame_prepared = false;
    return result;
}

Result Runtime::RegisterTexture(
    std::uint64_t context,
    const char* source,
    int width,
    int height,
    std::uint8_t* pixels,
    std::uint64_t pixel_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->RegisterTexture(source, width, height, pixels,
                                                    pixel_capacity)
                 : Result::InvalidHandle;
}

Result Runtime::ProcessMouseMove(
    std::uint64_t context,
    int x,
    int y,
    int modifiers
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::UserInput);
    state->context->ProcessMouseMove(x, y, MapModifiers(modifiers));
    return Result::Success;
}

Result Runtime::HasElementAtPoint(
    std::uint64_t context,
    int x,
    int y,
    std::uint8_t& hit
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    Rml::Element* element = state->context->GetElementAtPoint({float(x), float(y)});
    hit = element && element->GetTagName() != "#root" && element->GetTagName() != "body" && element->GetTagName() != "rml" &&
                  dynamic_cast<Rml::ElementDocument*>(element) == nullptr
              ? 1
              : 0;
    return Result::Success;
}

Result Runtime::ProcessMouseButton(
    std::uint64_t context,
    int button,
    std::uint8_t down,
    int modifiers
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::UserInput);
    if (button < 0 || button > 4)
        return Result::InvalidArgument;
    if (down != 0)
    {
        RequireEditableFont(state->context->GetHoverElement());
        state->context->ProcessMouseButtonDown(button, MapModifiers(modifiers));
    }
    else
        state->context->ProcessMouseButtonUp(button, MapModifiers(modifiers));
    return Result::Success;
}

Result Runtime::ProcessMouseWheel(
    std::uint64_t context,
    float x,
    float y,
    int modifiers
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::UserInput);
    state->context->ProcessMouseWheel({x, y}, MapModifiers(modifiers));
    return Result::Success;
}

Result Runtime::ProcessKey(
    std::uint64_t context,
    int key,
    std::uint8_t down,
    int modifiers
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::UserInput);
    const Rml::Input::KeyIdentifier identifier = MapKey(key);
    if (down != 0)
    {
        if (identifier != Rml::Input::KI_TAB)
            RequireEditableFont(state->context->GetFocusElement());
        state->context->ProcessKeyDown(identifier, MapModifiers(modifiers));
    }
    else
        state->context->ProcessKeyUp(identifier, MapModifiers(modifiers));
    return Result::Success;
}

Result Runtime::ProcessText(
    std::uint64_t context,
    const char* text
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    RmlUiRuntimeState::EventScope events(*state, EventOrigin::UserInput);
    if (!text)
        return Result::InvalidArgument;
    RequireEditableFont(state->context->GetFocusElement());
    state->context->ProcessTextInput(text);
    return Result::Success;
}

Result Runtime::GetTime(
    std::uint64_t context,
    double& seconds,
    std::uint8_t& paused
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    seconds = state->elapsed;
    paused = state->paused ? 1 : 0;
    return Result::Success;
}

Result Runtime::SetTimePaused(
    std::uint64_t context,
    std::uint8_t paused
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->paused = paused != 0;
    state->last_update = std::chrono::steady_clock::now();
    return Result::Success;
}

Result Runtime::AdvanceTime(
    std::uint64_t context,
    double seconds
) {
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    if (!std::isfinite(seconds) || seconds < 0 || !std::isfinite(state->elapsed + seconds))
        return Result::InvalidArgument;
    state->elapsed += seconds;
    state->last_update = std::chrono::steady_clock::now();
    return Result::Success;
}

Result Runtime::RefreshLayout(std::uint64_t context)
{
    auto* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    for (const auto& document : state->documents)
        document.second->UpdateDocument();
    return Result::Success;
}

Result Runtime::Update(std::uint64_t context)
{
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    const auto now = std::chrono::steady_clock::now();
    if (!state->paused)
        state->elapsed += std::chrono::duration<double>(now - state->last_update).count();
    state->last_update = now;
    state->frame_prepared = false;
    RmlUiTimeScope time_scope(state->elapsed);
    return state->context->Update() ? Result::Success : Result::BackendError;
}

Result Runtime::Render(
    std::uint64_t context,
    FrameInfo& frame
) {
    frame = {};
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    RmlUiTimeScope time_scope(state->elapsed);
    if (!state->frame_prepared)
    {
        state->renderer->BeginRender();
        const bool rendered = state->context->Render();
        state->renderer->EndRender();
        if (!rendered)
            return Result::BackendError;
        ++state->layout_revision;
        state->frame_prepared = true;
    }
    frame = {state->renderer->MeshUpdateCount(), state->renderer->ReleasedMeshCount(), state->renderer->CommandCount(),
             state->renderer->TextureUpdateCount(), state->renderer->ReleasedTextureCount()};
    return Result::Success;
}

Result Runtime::GetMeshInfo(
    std::uint64_t context,
    std::uint64_t index,
    MeshInfo& mesh
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->GetMeshInfo(index, mesh) : Result::InvalidHandle;
}

Result Runtime::CopyMeshVertices(
    std::uint64_t context,
    std::uint64_t mesh,
    Vertex* vertices,
    std::uint64_t vertex_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->CopyMeshVertices(mesh, vertices, vertex_capacity)
                 : Result::InvalidHandle;
}

Result Runtime::CopyMeshIndices(
    std::uint64_t context,
    std::uint64_t mesh,
    std::uint32_t* indices,
    std::uint64_t index_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->CopyMeshIndices(mesh, indices,
                                                    index_capacity)
                 : Result::InvalidHandle;
}

Result Runtime::CopyCommands(
    std::uint64_t context,
    DrawCommand* commands,
    std::uint64_t command_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->CopyCommands(commands, command_capacity) : Result::InvalidHandle;
}

Result Runtime::GetTextureInfo(
    std::uint64_t context,
    std::uint64_t index,
    TextureInfo& texture
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->GetTextureInfo(index, texture) : Result::InvalidHandle;
}

Result Runtime::CopyTexturePixels(
    std::uint64_t context,
    std::uint64_t texture,
    std::uint8_t* pixels,
    std::uint64_t pixel_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->CopyTexturePixels(texture, pixels,
                                                      pixel_capacity)
                 : Result::InvalidHandle;
}

Result Runtime::CopyReleasedTextures(
    std::uint64_t context,
    std::uint64_t* textures,
    std::uint64_t texture_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->CopyReleasedTextures(textures,
                                                         texture_capacity)
                 : Result::InvalidHandle;
}

Result Runtime::CopyReleasedMeshes(
    std::uint64_t context,
    std::uint64_t* meshes,
    std::uint64_t mesh_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    return state ? state->renderer->CopyReleasedMeshes(meshes,
                                                       mesh_capacity)
                 : Result::InvalidHandle;
}

Result Runtime::FinishFrame(std::uint64_t context)
{
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->renderer->FinishFrame();
    state->frame_prepared = false;
    return Result::Success;
}

Result Runtime::GetEventCount(
    std::uint64_t context,
    std::uint64_t& count
) {
    count = 0;
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    count = static_cast<std::uint64_t>(state->events.size());
    return Result::Success;
}

Result Runtime::GetEventInfo(
    std::uint64_t context,
    std::uint64_t index,
    EventInfo& event_info
) {
    event_info = {};
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    if (index >= static_cast<std::uint64_t>(state->events.size()))
        return Result::NotFound;
    const QueuedEvent& value = state->events[static_cast<std::size_t>(index)];
    event_info = {value.type, value.document, static_cast<std::uint64_t>(value.target_id.size()), value.value.info, value.phase, value.origin};
    return Result::Success;
}

Result Runtime::CopyEventTargetId(
    std::uint64_t context,
    std::uint64_t index,
    char* target_id,
    std::uint64_t target_capacity
) {
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    if (index >= static_cast<std::uint64_t>(state->events.size()))
        return Result::NotFound;
    const std::string& value = state->events[static_cast<std::size_t>(index)].target_id;
    if (target_capacity < value.size() + 1)
        return Result::BufferTooSmall;
    if (target_id == nullptr)
        return Result::InvalidArgument;
    std::memcpy(target_id, value.data(), value.size());
    target_id[value.size()] = '\0';
    return Result::Success;
}

Result Runtime::ClearEvents(std::uint64_t context)
{
    RmlUiRuntimeState::ContextState* state = GetRuntimeState(m_state) ? GetRuntimeState(m_state)->Find(context) : nullptr;
    if (!state)
        return Result::InvalidHandle;
    state->events.clear();
    return Result::Success;
}

}
