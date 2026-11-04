#include "RmlUiTextInputHandler.hpp"
#include "RmlUiControlValue.hpp"

#include <RmlUi/Core/Context.h>
#include <RmlUi/Core/TextInputContext.h>
#include <RmlUi/Core/Element.h>
#include <RmlUi/Core/ElementDocument.h>
#include <RmlUi/Core/Rectangle.h>
#include <RmlUi/Core/StringUtilities.h>
#include <algorithm>
#include <cstring>
#include <cmath>

namespace Inno::UI::RmlUiAdapter
{
void RmlUiTextInputHandler::Attach(Rml::Context* context)
{
    m_context = context;
}

void RmlUiTextInputHandler::OnActivate(Rml::TextInputContext* context)
{
    if (m_input != context)
        Cancel();
    m_input = context;
    m_element = nullptr;
}

void RmlUiTextInputHandler::OnDeactivate(Rml::TextInputContext* context)
{
    if (m_input != context)
        return;
    Cancel();
    m_input = nullptr;
    m_element = nullptr;
}

void RmlUiTextInputHandler::OnDestroy(Rml::TextInputContext* context)
{
    if (m_input != context)
        return;
    // Destruction ends the borrow; calling its virtual text methods is no longer valid.
    m_input = nullptr;
    m_element = nullptr;
    m_operation = 0;
    m_original.clear();
}

void RmlUiTextInputHandler::Restore()
{
    m_input->SetCompositionRange(0, 0);
    m_input->SetText(m_original, m_start, m_end);
    m_input->SetSelectionRange(m_start, m_original_end);
}

void RmlUiTextInputHandler::Cancel()
{
    if (m_operation == 0)
        return;
    Restore();
    m_operation = 0;
    m_original.clear();
}

void RmlUiTextInputHandler::CancelFor(Rml::Element* element)
{
    if (element == m_element)
        Cancel();
}

void RmlUiTextInputHandler::Synchronize(std::uint64_t operation)
{
    if (m_operation != operation)
        Cancel();
}

bool RmlUiTextInputHandler::GetArea(
    float& x,
    float& y,
    float& width,
    float& height
) const {
    auto* element = m_context ? m_context->GetFocusElement() : nullptr;
    auto* document = element ? element->GetOwnerDocument() : nullptr;
    if (!m_input || !document || !document->IsVisible() || !element->IsVisible())
        return false;
    Rml::Rectanglef bounds;
    if (!m_input->GetBoundingBox(bounds) || !bounds.Valid())
        return false;
    x = bounds.Left();
    y = bounds.Top();
    width = bounds.Width();
    height = bounds.Height();
    return std::isfinite(x) && std::isfinite(y) && std::isfinite(width) &&
        std::isfinite(height) && width > 0 && height > 0;
}

Result RmlUiTextInputHandler::Process(
    std::uint64_t operation,
    std::uint32_t phase,
    const char* text,
    int selection_start,
    int selection_length
) {
    if (operation == 0 || phase > 3 || !text || selection_start < -1 || selection_length < 0)
        return Result::InvalidArgument;
    const Rml::StringView text_view(text, text + std::strlen(text));
    const auto length = Rml::StringUtilities::LengthUTF8(text_view);
    if ((selection_start == -1 && selection_length != 0) ||
        (selection_start >= 0 && (static_cast<std::size_t>(selection_start) > length ||
            static_cast<std::size_t>(selection_length) > length - selection_start)))
        return Result::InvalidArgument;
    if (phase == 0)
    {
        if (operation <= m_last_operation)
            return Result::Success;
        Cancel();
        m_last_operation = operation;
        if (!m_input)
            return Result::Success;
        // Focus callbacks run before Context publishes its new focus pointer. Capture
        // the element only when an input operation begins after focus dispatch returns.
        m_element = m_context->GetFocusElement();
        const auto value = RmlUiControlValue::Read(m_element);
        if (value.info.kind != ControlValueKind::Text)
            return Result::Success;
        RmlUiControlValue::RequireEditableFont(m_element);
        m_input->GetSelectionRange(m_start, m_original_end);
        if (m_original_end < m_start)
            std::swap(m_original_end, m_start);
        const int start_byte = Rml::StringUtilities::ConvertCharacterOffsetToByteOffset(value.text, m_start);
        const int end_byte = Rml::StringUtilities::ConvertCharacterOffsetToByteOffset(value.text, m_original_end);
        m_original = value.text.substr(start_byte, end_byte - start_byte);
        m_end = m_original_end;
        m_operation = operation;
    }
    if (!m_input || m_operation != operation)
        return Result::Success;
    if (phase != 3)
        RmlUiControlValue::RequireEditableFont(m_element);
    if (phase == 2)
    {
        // Restore the selection before ordinary insertion so the control applies its own
        // maxlength, line-break and change-event rules exactly once, including empty ranges.
        Restore();
        m_operation = 0;
        m_original.clear();
        if (*text != '\0')
            m_context->ProcessTextInput(text);
        return Result::Success;
    }
    if (phase == 3)
    {
        Cancel();
        return Result::Success;
    }
    m_input->SetText(text_view, m_start, m_end);
    m_end = m_start + static_cast<int>(length);
    m_input->SetCompositionRange(m_start, m_end);
    const int selection = selection_start < 0 ? m_end : m_start + selection_start;
    m_input->SetSelectionRange(selection, selection + selection_length);
    return Result::Success;
}
}
