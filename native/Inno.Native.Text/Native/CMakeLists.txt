cmake_minimum_required(VERSION 3.20)
project(inno_text LANGUAGES C CXX)
set(INNO_TEXT_BRIDGE_ROOT "${CMAKE_CURRENT_LIST_DIR}/Generated" CACHE PATH "Generated text bridge source directory")

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
    add_subdirectory("${CMAKE_CURRENT_LIST_DIR}/../../../extern/freetype" freetype EXCLUDE_FROM_ALL)
endif()
if(NOT TARGET harfbuzz)
    add_subdirectory("${CMAKE_CURRENT_LIST_DIR}/../../../extern/harfbuzz" harfbuzz EXCLUDE_FROM_ALL)
endif()

if(EMSCRIPTEN)
    set(INNO_TEXT_LIBRARY_TYPE STATIC)
else()
    set(INNO_TEXT_LIBRARY_TYPE SHARED)
endif()

add_library(inno-text ${INNO_TEXT_LIBRARY_TYPE}
    "${INNO_TEXT_BRIDGE_ROOT}/src/Classes.cpp" src/TextRuntime.cpp)
target_compile_features(inno-text PRIVATE cxx_std_17)
target_include_directories(inno-text PRIVATE
    "${INNO_TEXT_BRIDGE_ROOT}/include" "${CMAKE_CURRENT_LIST_DIR}/include")
target_link_libraries(inno-text PRIVATE freetype harfbuzz)
set_target_properties(inno-text PROPERTIES
    CXX_VISIBILITY_PRESET hidden
    VISIBILITY_INLINES_HIDDEN YES
    OUTPUT_NAME "inno-text")
