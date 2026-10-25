#include <imgui/imgui.h>
#include <imgui/imgui_internal.h>
#include "ImGuiPlatformCallbacks.h"

#include <new>
#include <thread>

namespace
{
    struct PlatformCallbacks
    {
        ImGuiContext* context;
        ImGuiIO* io;
        ImGuiPlatformIO* platform;
        std::thread::id thread;
        InnoImGuiWindowCallback set_position;
        InnoImGuiWindowCallback set_size;
        unsigned active_frames;
    };

    struct CallbackFrame
    {
        PlatformCallbacks& callbacks;

        explicit CallbackFrame(PlatformCallbacks& callbacks) : callbacks(callbacks)
        {
            ++callbacks.active_frames;
        }

        ~CallbackFrame()
        {
            --callbacks.active_frames;
        }
    };

    PlatformCallbacks& CurrentCallbacks()
    {
        auto* callbacks = static_cast<PlatformCallbacks*>(ImGui::GetIO().BackendPlatformUserData);
        IM_ASSERT(callbacks != nullptr && callbacks->context == ImGui::GetCurrentContext());
        IM_ASSERT(callbacks->thread == std::this_thread::get_id());
        return *callbacks;
    }

    void SetWindowPosition(
        ImGuiViewport* viewport,
        ImVec2 position
    ) {
        PlatformCallbacks& callbacks = CurrentCallbacks();
        CallbackFrame frame(callbacks);
        callbacks.set_position(viewport, position.x, position.y);
    }

    void SetWindowSize(
        ImGuiViewport* viewport,
        ImVec2 size
    ) {
        PlatformCallbacks& callbacks = CurrentCallbacks();
        CallbackFrame frame(callbacks);
        callbacks.set_size(viewport, size.x, size.y);
    }
}

void* igInnoCreatePlatformWindowCallbacks(
    ImGuiContext* context,
    InnoImGuiWindowCallback set_position,
    InnoImGuiWindowCallback set_size
) {
    if (context == nullptr || context != ImGui::GetCurrentContext()
        || set_position == nullptr || set_size == nullptr)
        return nullptr;
    ImGuiIO& io = ImGui::GetIO();
    if (io.BackendPlatformUserData != nullptr)
        return nullptr;
    auto* callbacks = new (std::nothrow) PlatformCallbacks{
        context, &io, &ImGui::GetPlatformIO(), std::this_thread::get_id(), set_position, set_size, 0};
    if (callbacks == nullptr)
        return nullptr;
    io.BackendPlatformUserData = callbacks;
    callbacks->platform->Platform_SetWindowPos = SetWindowPosition;
    callbacks->platform->Platform_SetWindowSize = SetWindowSize;
    return callbacks;
}

int igInnoCanDestroyPlatformWindowCallbacks(void* registration)
{
    if (registration == nullptr)
        return 1;
    auto& callbacks = *static_cast<PlatformCallbacks*>(registration);
    return callbacks.thread == std::this_thread::get_id()
        && callbacks.context == ImGui::GetCurrentContext()
        && callbacks.io->BackendPlatformUserData == registration
        && callbacks.active_frames == 0;
}

void igInnoDestroyPlatformWindowCallbacks(void* registration)
{
    IM_ASSERT(igInnoCanDestroyPlatformWindowCallbacks(registration) != 0);
    if (registration == nullptr)
        return;
    auto* callbacks = static_cast<PlatformCallbacks*>(registration);
    callbacks->platform->Platform_SetWindowPos = nullptr;
    callbacks->platform->Platform_SetWindowSize = nullptr;
    callbacks->io->BackendPlatformUserData = nullptr;
    delete callbacks;
}
