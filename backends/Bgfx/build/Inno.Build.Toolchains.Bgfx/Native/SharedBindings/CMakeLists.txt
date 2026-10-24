cmake_minimum_required(VERSION 3.20)
project(InnoBgfxBinding C CXX)

# The common manifest hook adds independently compiled translation units.
add_library(inno_bgfx_binding STATIC)
set_target_properties(inno_bgfx_binding PROPERTIES PREFIX "")
