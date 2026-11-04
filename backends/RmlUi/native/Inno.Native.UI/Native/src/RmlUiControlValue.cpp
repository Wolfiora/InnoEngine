#include "RmlUiControlValue.hpp"

#include <RmlUi/Core/Element.h>
#include <RmlUi/Core/Elements/ElementFormControl.h>
#include <RmlUi/Core/Elements/ElementFormControlInput.h>
#include <RmlUi/Core/Elements/ElementFormControlSelect.h>
#include <RmlUi/Core/Elements/ElementFormControlTextArea.h>
#include <charconv>
#include <cmath>
#include <cstring>
#include <stdexcept>

namespace Inno::UI::RmlUiAdapter
{
void RmlUiControlValue::RequireEditableFont(Rml::Element* element)
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

RmlUiControlValue RmlUiControlValue::Read(Rml::Element* element)
{
    RmlUiControlValue value;
    auto* control = dynamic_cast<Rml::ElementFormControl*>(element);
    if (!control)
        return value;
    if (dynamic_cast<Rml::ElementFormControlSelect*>(control))
        value.info.kind = ControlValueKind::Selection;
    else if (dynamic_cast<Rml::ElementFormControlTextArea*>(control))
        value.info.kind = ControlValueKind::Text;
    else if (dynamic_cast<Rml::ElementFormControlInput*>(control))
    {
        const auto type = control->GetAttribute<Rml::String>("type", "text");
        if (type == "checkbox" || type == "radio")
        {
            value.info.kind = ControlValueKind::Boolean;
            value.info.checked = control->HasAttribute("checked") ? 1 : 0;
            return value;
        }
        if (type == "range")
        {
            const std::string text = control->GetValue();
            const auto result = std::from_chars(text.data(), text.data() + text.size(), value.info.number);
            if (result.ec != std::errc{} || result.ptr != text.data() + text.size() || !std::isfinite(value.info.number))
                return {};
            value.info.kind = ControlValueKind::Number;
            return value;
        }
        if (type != "text" && type != "password")
            return value;
        value.info.kind = ControlValueKind::Text;
    }
    else
        return value;
    value.text = control->GetValue();
    value.info.text_length = value.text.size();
    return value;
}

Result RmlUiControlValue::Write(
    Rml::Element* element,
    ControlValueKind kind,
    const char* text,
    double number,
    bool checked,
    bool& accepted
) {
    accepted = false;
    const auto previous = Read(element);
    if (previous.info.kind == ControlValueKind::None)
        return Result::Success;
    if (previous.info.kind != kind)
        return Result::InvalidArgument;
    auto* control = static_cast<Rml::ElementFormControl*>(element);
    switch (kind)
    {
    case ControlValueKind::Text:
        if (!text)
            return Result::InvalidArgument;
        control->SetValue(text);
        break;
    case ControlValueKind::Selection:
    {
        if (!text)
            return Result::InvalidArgument;
        auto* select = static_cast<Rml::ElementFormControlSelect*>(control);
        for (int index = 0; index < select->GetNumOptions(); ++index)
        {
            auto* option = select->GetOption(index);
            if (option && !option->HasAttribute("disabled") && option->GetAttribute<Rml::String>("value", "") == text)
            {
                select->SetSelection(index);
                accepted = true;
                return Result::Success;
            }
        }
        return Result::Success;
    }
    case ControlValueKind::Boolean:
        if (checked)
            control->SetAttribute("checked", "");
        else
            control->RemoveAttribute("checked");
        break;
    case ControlValueKind::Number:
    {
        if (!std::isfinite(number))
            return Result::InvalidArgument;
        char buffer[64];
        const auto result = std::to_chars(buffer, buffer + sizeof(buffer), number);
        if (result.ec != std::errc{})
            return Result::InvalidArgument;
        control->SetValue(std::string(buffer, result.ptr));
        break;
    }
    default:
        return Result::InvalidArgument;
    }
    accepted = true;
    return Result::Success;
}

Result RmlUiControlValue::CopyText(
    char* destination,
    std::uint64_t capacity
) const {
    if (capacity < text.size())
        return Result::BufferTooSmall;
    if (text.empty())
        return Result::Success;
    if (!destination)
        return Result::InvalidArgument;
    std::memcpy(destination, text.data(), text.size());
    return Result::Success;
}
}
