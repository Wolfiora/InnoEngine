#pragma once

#include <cimgui.h>

// Scalar callbacks keep register classification inside the native ImGui ABI.
typedef void (*InnoImGuiWindowCallback)(
    ImGuiViewport* viewport,
    float x,
    float y
);

// Requires the current context's owner thread and unassigned BackendPlatformUserData.
// A null result retains nothing. The handle owns the registration until destruction.
CIMGUI_API void* igInnoCreatePlatformWindowCallbacks(
    ImGuiContext* context,
    InnoImGuiWindowCallback set_position,
    InnoImGuiWindowCallback set_size
);

// Checks owner thread, current context, ownership and native callback-frame quiescence.
CIMGUI_API int igInnoCanDestroyPlatformWindowCallbacks(void* registration);

// Requires a successful quiescence check on the same owner thread without intervening calls.
CIMGUI_API void igInnoDestroyPlatformWindowCallbacks(void* registration);
