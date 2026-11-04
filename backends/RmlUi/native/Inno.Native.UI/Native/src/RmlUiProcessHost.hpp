#pragma once

#include "RmlUiRuntime.hpp"
#include <memory>

namespace Rml
{
class RenderInterface;
}

namespace Inno::UI::RmlUiAdapter
{

/// Binds one context clock for the duration of a synchronous native operation.
class RmlUiTimeScope final
{
  public:
    explicit RmlUiTimeScope(double time);
    ~RmlUiTimeScope() noexcept;
    RmlUiTimeScope(const RmlUiTimeScope&) = delete;
    RmlUiTimeScope& operator=(const RmlUiTimeScope&) = delete;
  private:
    double m_previous;
};

bool AcquireProcessHost();
void ReleaseProcessHost() noexcept;
void RetainRenderInterface(std::unique_ptr<Rml::RenderInterface> renderer);
std::uint64_t AllocateContextId() noexcept;
std::uint64_t AllocateDocumentId() noexcept;
Result LoadFont(const std::uint8_t* data, std::uint64_t length, const char* family, int style, int weight, bool fallback);

}
