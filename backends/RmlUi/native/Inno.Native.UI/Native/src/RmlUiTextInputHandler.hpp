#pragma once

#include "RmlUiRuntime.hpp"
#include <RmlUi/Core/Types.h>
#include <RmlUi/Core/TextInputHandler.h>
#include <cstdint>

namespace Rml { class Context; class Element; }

namespace Inno::UI::RmlUiAdapter
{
// Owns only the focused native borrow, never a managed callback or a document lifetime.
class RmlUiTextInputHandler final : public Rml::TextInputHandler
{
  public:
    void Attach(Rml::Context* context);
    void OnActivate(Rml::TextInputContext* context) override;
    void OnDeactivate(Rml::TextInputContext* context) override;
    void OnDestroy(Rml::TextInputContext* context) override;
    Result Process(
        std::uint64_t operation,
        std::uint32_t phase,
        const char* text,
        int selection_start,
        int selection_length
    );
    void Cancel();
    void CancelFor(Rml::Element* element);
    void Synchronize(std::uint64_t operation);
    bool GetArea(
        float& x,
        float& y,
        float& width,
        float& height
    ) const;

  private:
    Rml::Context* m_context = nullptr;
    Rml::TextInputContext* m_input = nullptr;
    Rml::Element* m_element = nullptr;
    std::uint64_t m_operation = 0;
    std::uint64_t m_last_operation = 0;
    int m_start = 0;
    int m_original_end = 0;
    int m_end = 0;
    Rml::String m_original;

    void Restore();
};
}
