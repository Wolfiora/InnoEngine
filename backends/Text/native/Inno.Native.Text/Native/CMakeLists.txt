cmake_minimum_required(VERSION 3.20)
if(NOT IS_DIRECTORY "${INNO_ROOT}/extern")
    message(FATAL_ERROR "INNO_ROOT must identify the explicitly selected checkout")
endif()
project(inno_text LANGUAGES C CXX)
file(TO_CMAKE_PATH "${INNO_TEXT_BRIDGE_ROOT}" INNO_TEXT_BRIDGE_ROOT)
if(NOT INNO_TEXT_BRIDGE_ROOT OR NOT EXISTS "${INNO_TEXT_BRIDGE_ROOT}/src/Classes.cpp")
    message(FATAL_ERROR "An explicit complete Text binding generation is required")
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

if(NOT TARGET freetype)
    add_subdirectory("${INNO_ROOT}/extern/freetype" freetype EXCLUDE_FROM_ALL)
endif()
if(NOT TARGET harfbuzz)
    add_subdirectory("${INNO_ROOT}/extern/harfbuzz" harfbuzz EXCLUDE_FROM_ALL)
endif()

if(NOT INNO_LIBRARY_KIND MATCHES "^(STATIC|SHARED)$")
    message(FATAL_ERROR "INNO_LIBRARY_KIND must explicitly select STATIC or SHARED")
endif()

add_library(inno-text ${INNO_LIBRARY_KIND}
    "${INNO_TEXT_BRIDGE_ROOT}/src/Classes.cpp" src/TextRuntime.cpp)
target_compile_features(inno-text PRIVATE cxx_std_17)
target_include_directories(inno-text PRIVATE
    "${INNO_TEXT_BRIDGE_ROOT}/include" "${CMAKE_CURRENT_LIST_DIR}/include")
target_link_libraries(inno-text PRIVATE freetype harfbuzz)
set_target_properties(inno-text PROPERTIES
    CXX_VISIBILITY_PRESET hidden
    VISIBILITY_INLINES_HIDDEN YES
    OUTPUT_NAME "inno-text")
