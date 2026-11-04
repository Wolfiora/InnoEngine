#pragma once

#include "RmlUiRuntime.hpp"
#include <string>

namespace Rml { class Element; }

namespace Inno::UI::RmlUiAdapter
{
/// Owns the neutral value captured synchronously from a form control.
struct RmlUiControlValue
{
    ControlValueInfo info{};
    std::string text;

    static RmlUiControlValue Read(Rml::Element* element);
    static Result Write(
        Rml::Element* element,
        ControlValueKind kind,
        const char* text,
        double number,
        bool checked,
        bool& accepted
    );
    Result CopyText(
        char* destination,
        std::uint64_t capacity
    ) const;
};
}
