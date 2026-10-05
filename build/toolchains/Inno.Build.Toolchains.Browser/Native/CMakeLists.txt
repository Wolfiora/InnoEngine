cmake_minimum_required(VERSION 3.20)
project(inno_browser_native LANGUAGES C CXX)
if(NOT EMSCRIPTEN)
    message(FATAL_ERROR "This target requires the .NET Emscripten toolchain.")
endif()
get_filename_component(INNO_ROOT "${CMAKE_CURRENT_LIST_DIR}/../../../.." ABSOLUTE)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/artifacts")
set(BUILD_SHARED_LIBS OFF CACHE BOOL "" FORCE)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -sSUPPORT_LONGJMP=wasm -fwasm-exceptions")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -sSUPPORT_LONGJMP=wasm -fwasm-exceptions")

# Use the same semantic bridges and pinned dependencies as desktop builds.
set(SDL_SHARED OFF CACHE BOOL "" FORCE)
set(SDL_STATIC ON CACHE BOOL "" FORCE)
set(SDL_TESTS OFF CACHE BOOL "" FORCE)
set(SDL_INSTALL OFF CACHE BOOL "" FORCE)
add_subdirectory("${INNO_ROOT}/extern/SDL" sdl)
set(MINIAUDIO_INSTALL OFF CACHE BOOL "" FORCE)
set(MINIAUDIO_NO_EXTRA_NODES ON CACHE BOOL "" FORCE)
add_subdirectory("${INNO_ROOT}/extern/miniaudio" miniaudio)
if(NOT INNO_UI_BRIDGE_ROOT OR NOT EXISTS "${INNO_UI_BRIDGE_ROOT}/src/Classes.cpp")
    message(FATAL_ERROR "A complete request-selected UI bridge is required.")
endif()
if(NOT INNO_TEXT_BRIDGE_ROOT OR NOT EXISTS "${INNO_TEXT_BRIDGE_ROOT}/src/Classes.cpp")
    message(FATAL_ERROR "A complete request-selected text bridge is required.")
endif()
add_subdirectory("${INNO_ROOT}/native/Inno.Native.UI/Native" ui)
add_subdirectory("${INNO_ROOT}/native/Inno.Native.Text/Native" text)

file(GLOB BGFX_SOURCES CONFIGURE_DEPENDS "${INNO_ROOT}/extern/bgfx/src/*.cpp")
list(FILTER BGFX_SOURCES EXCLUDE REGEX "/amalgamated\\.cpp$")
add_library(bgfx STATIC ${BGFX_SOURCES})
# WebGL 2 exposes the OpenGL ES 3.0 feature set.
target_compile_definitions(bgfx PRIVATE BGFX_CONFIG_RENDERER_OPENGLES=30)
add_library(bx STATIC "${INNO_ROOT}/extern/bx/src/amalgamated.cpp")
file(GLOB ASTC_SOURCES CONFIGURE_DEPENDS "${INNO_ROOT}/extern/bimg/3rdparty/astc-encoder/source/*.cpp")
list(FILTER ASTC_SOURCES EXCLUDE REGEX "astcenccli")
add_library(bimg STATIC "${INNO_ROOT}/extern/bimg/src/image.cpp" "${INNO_ROOT}/extern/bimg/src/image_gnf.cpp" ${ASTC_SOURCES})
add_library(bimg_decode STATIC "${INNO_ROOT}/extern/bimg/src/image_decode.cpp")
foreach(target bgfx bx bimg bimg_decode)
    target_compile_features(${target} PRIVATE cxx_std_20)
    target_compile_definitions(${target} PRIVATE BX_CONFIG_DEBUG=0 __STDC_LIMIT_MACROS __STDC_FORMAT_MACROS __STDC_CONSTANT_MACROS)
    target_compile_definitions(${target} PRIVATE
        __EMSCRIPTEN_MAJOR__=__EMSCRIPTEN_major__
        __EMSCRIPTEN_MINOR__=__EMSCRIPTEN_minor__
        __EMSCRIPTEN_TINY__=__EMSCRIPTEN_tiny__)
    target_compile_options(${target} PRIVATE -fno-rtti -fno-exceptions -ffast-math -include emscripten/version.h)
    target_include_directories(${target} PRIVATE
        "${INNO_ROOT}/extern/bgfx/include" "${INNO_ROOT}/extern/bgfx/3rdparty" "${INNO_ROOT}/extern/bgfx/3rdparty/khronos"
        "${INNO_ROOT}/extern/bx/include" "${INNO_ROOT}/extern/bx/3rdparty"
        "${INNO_ROOT}/extern/bimg/include" "${INNO_ROOT}/extern/bimg/3rdparty"
        "${INNO_ROOT}/extern/bimg/3rdparty/astc-encoder/include" "${INNO_ROOT}/extern/bimg/3rdparty/tinyexr/deps")
    set_target_properties(${target} PROPERTIES PREFIX "" OUTPUT_NAME "${target}Release")
endforeach()
install(TARGETS bgfx bx bimg bimg_decode ARCHIVE COMPONENT Inno DESTINATION bgfx/browser-wasm)
install(TARGETS SDL3-static ARCHIVE COMPONENT Inno DESTINATION sdl3/browser-wasm)
install(TARGETS miniaudio ARCHIVE COMPONENT Inno DESTINATION miniaudio/browser-wasm)
install(TARGETS inno-text freetype harfbuzz ARCHIVE COMPONENT Inno DESTINATION text/browser-wasm)
install(TARGETS inno-ui rmlui_core ARCHIVE COMPONENT Inno DESTINATION ui/browser-wasm)
