cmake_minimum_required(VERSION 3.20)
file(TO_CMAKE_PATH "${INNO_ROOT}" INNO_ROOT)
if(NOT IS_DIRECTORY "${INNO_ROOT}/extern")
    message(FATAL_ERROR "INNO_ROOT must identify the explicitly selected checkout")
endif()
project(inno_ui LANGUAGES C CXX)
file(TO_CMAKE_PATH "${INNO_UI_BRIDGE_ROOT}" INNO_UI_BRIDGE_ROOT)
if(NOT INNO_UI_BRIDGE_ROOT OR NOT EXISTS "${INNO_UI_BRIDGE_ROOT}/src/Classes.cpp")
    message(FATAL_ERROR "An explicit complete UI binding generation is required")
endif()

set(BUILD_SHARED_LIBS OFF CACHE BOOL "" FORCE)
set(FT_DISABLE_BROTLI ON CACHE BOOL "" FORCE)
set(FT_DISABLE_BZIP2 ON CACHE BOOL "" FORCE)
set(FT_DISABLE_HARFBUZZ ON CACHE BOOL "" FORCE)
set(FT_DISABLE_PNG ON CACHE BOOL "" FORCE)
set(FT_DISABLE_ZLIB ON CACHE BOOL "" FORCE)
set(HB_BUILD_UTILS OFF CACHE BOOL "" FORCE)
set(HB_BUILD_SUBSET OFF CACHE BOOL "" FORCE)
set(HB_BUILD_RASTER OFF CACHE BOOL "" FORCE)
set(HB_BUILD_VECTOR OFF CACHE BOOL "" FORCE)
set(HB_BUILD_GPU OFF CACHE BOOL "" FORCE)
set(HB_HAVE_CORETEXT OFF CACHE BOOL "" FORCE)
set(HB_HAVE_FREETYPE ON CACHE BOOL "" FORCE)
set(RMLUI_FONT_ENGINE none CACHE STRING "" FORCE)
set(RMLUI_SAMPLES OFF CACHE BOOL "" FORCE)
set(RMLUI_TESTS OFF CACHE BOOL "" FORCE)
set(RMLUI_LUA_BINDINGS OFF CACHE BOOL "" FORCE)
set(RMLUI_LOTTIE_PLUGIN OFF CACHE BOOL "" FORCE)
set(RMLUI_SVG_PLUGIN OFF CACHE BOOL "" FORCE)
set(RMLUI_PRECOMPILED_HEADERS OFF CACHE BOOL "" FORCE)
set(RMLUI_COMPILER_OPTIONS OFF CACHE BOOL "" FORCE)


if(NOT TARGET freetype)
    add_subdirectory("${INNO_ROOT}/extern/freetype" freetype EXCLUDE_FROM_ALL)
endif()
if(NOT TARGET Freetype::Freetype)
    add_library(Freetype::Freetype ALIAS freetype-interface)
endif()
if(NOT TARGET harfbuzz)
    add_subdirectory("${INNO_ROOT}/extern/harfbuzz" harfbuzz EXCLUDE_FROM_ALL)
endif()
add_subdirectory("${INNO_ROOT}/extern/RmlUi" rmlui EXCLUDE_FROM_ALL)

set(RMLUI_HARFBUZZ_DIR "${INNO_ROOT}/extern/RmlUi/Samples/basic/harfbuzz/src")
set(RMLUI_HARFBUZZ_SOURCES
    ${INNO_ROOT}/extern/RmlUi/Source/Core/FontEngineDefault/FreeTypeInterface.cpp
    ${RMLUI_HARFBUZZ_DIR}/FontEngineInterfaceHarfBuzz.cpp
    ${RMLUI_HARFBUZZ_DIR}/FontFace.cpp
    ${RMLUI_HARFBUZZ_DIR}/FontFaceHandleHarfBuzz.cpp
    ${RMLUI_HARFBUZZ_DIR}/FontFaceLayer.cpp
    ${RMLUI_HARFBUZZ_DIR}/FontFamily.cpp
    ${RMLUI_HARFBUZZ_DIR}/FontProvider.cpp
    ${RMLUI_HARFBUZZ_DIR}/FreeTypeInterface.cpp
    ${RMLUI_HARFBUZZ_DIR}/TextureLayout.cpp
    ${RMLUI_HARFBUZZ_DIR}/TextureLayoutRectangle.cpp
    ${RMLUI_HARFBUZZ_DIR}/TextureLayoutRow.cpp
    ${RMLUI_HARFBUZZ_DIR}/TextureLayoutTexture.cpp)

if(NOT INNO_LIBRARY_KIND MATCHES "^(STATIC|SHARED)$")
    message(FATAL_ERROR "INNO_LIBRARY_KIND must explicitly select STATIC or SHARED")
endif()

add_library(inno-ui ${INNO_LIBRARY_KIND}
    "${INNO_UI_BRIDGE_ROOT}/src/Classes.cpp"
    src/RmlUiEventListener.cpp
    src/RmlUiProcessHost.cpp
    src/RmlUiRenderInterface.cpp
    src/RmlUiRuntime.cpp
    ${RMLUI_HARFBUZZ_SOURCES})
target_compile_features(inno-ui PRIVATE cxx_std_20)
target_include_directories(inno-ui PRIVATE
    "${INNO_UI_BRIDGE_ROOT}/include"
    "${CMAKE_CURRENT_LIST_DIR}/include"
    "${CMAKE_CURRENT_LIST_DIR}/src"
    "${RMLUI_HARFBUZZ_DIR}"
    "${INNO_ROOT}/extern/RmlUi/Source/Core")
target_link_libraries(inno-ui PRIVATE rmlui_core freetype harfbuzz)
set_target_properties(inno-ui PROPERTIES
    CXX_VISIBILITY_PRESET hidden
    VISIBILITY_INLINES_HIDDEN YES
    OUTPUT_NAME "inno-ui")
