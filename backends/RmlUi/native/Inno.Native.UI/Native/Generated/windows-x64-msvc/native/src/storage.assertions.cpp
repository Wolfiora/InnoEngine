/* BGCS compiler verification of parsed native storage facts. */

#include <stddef.h>
#include <type_traits>
#include "RmlUiRuntime.hpp"

#if defined(sizeof)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(unsigned)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(signed)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(long)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(short)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(int)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(char)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(float)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(double)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(void)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(const)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(volatile)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(struct)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(union)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(enum)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(__builtin_offsetof)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(BGCS_NATIVE_FIELD_OFFSET)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(static_assert)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(alignof)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(decltype)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(static_cast)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(typename)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(using)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(nullptr)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(bool)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(std)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(is_same)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(is_signed)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(is_lvalue_reference)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(is_rvalue_reference)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(remove_cv)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(underlying_type)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(type)
#error BGCS native storage proof token is shadowed by a macro
#endif
#if defined(_MSC_VER) && !defined(__clang__) && !defined(__cplusplus)
#define BGCS_NATIVE_FIELD_OFFSET(s,m) ((unsigned long long)&(((s*)0)->m))
#else
#define BGCS_NATIVE_FIELD_OFFSET(s,m) __builtin_offsetof(s,m)
#endif
#if defined(__BYTE_ORDER__) && defined(__ORDER_LITTLE_ENDIAN__) && defined(__ORDER_BIG_ENDIAN__)
static_assert(__BYTE_ORDER__ == __ORDER_LITTLE_ENDIAN__, "BGCS native byte order differs from parsed facts");
#elif defined(_MSC_VER) && (defined(_M_IX86) || defined(_M_X64) || defined(_M_AMD64) || defined(_M_ARM) || defined(_M_ARM64))
#else
#error BGCS native compiler did not independently establish target byte order
#endif
static_assert(sizeof(void*) == 8, "BGCS native pointer width differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::EventInfo) == 24, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::EventInfo) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::EventInfo>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::EventInfo>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::EventInfo>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::EventInfo, type) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::EventInfo, document) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::EventInfo, target_id_length) == 16, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::Runtime) == 8, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::Runtime) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::Runtime>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::Runtime>::value == 0, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::Runtime>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(sizeof(float) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(float) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(sizeof(EventType) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(EventType) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_same<typename std::underlying_type<EventType>::type, unsigned int>::value, "BGCS native enum underlying type differs from parsed facts");
static_assert(sizeof(unsigned long long) == 8, "BGCS native storage size differs from parsed facts");
static_assert(alignof(unsigned long long) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert((((unsigned long long)-1) < ((unsigned long long)1)) == 0, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(char) == 1, "BGCS native storage size differs from parsed facts");
static_assert(alignof(char) == 1, "BGCS native storage alignment differs from parsed facts");
static_assert((((char)-1) < ((char)1)) == 1, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(unsigned char) == 1, "BGCS native storage size differs from parsed facts");
static_assert(alignof(unsigned char) == 1, "BGCS native storage alignment differs from parsed facts");
static_assert((((unsigned char)-1) < ((unsigned char)1)) == 0, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::TextureInfo) == 32, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::TextureInfo) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::TextureInfo>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::TextureInfo>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::TextureInfo>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::TextureInfo, id) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::TextureInfo, revision) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::TextureInfo, width) == 16, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::TextureInfo, height) == 20, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::TextureInfo, byte_length) == 24, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(unsigned int) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(unsigned int) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert((((unsigned int)-1) < ((unsigned int)1)) == 0, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(int) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(int) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert((((int)-1) < ((int)1)) == 1, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::Vertex) == 20, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::Vertex) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::Vertex>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::Vertex>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::Vertex>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::Vertex, x) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::Vertex, y) == 4, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::Vertex, u) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::Vertex, v) == 12, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::Vertex, color) == 16, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::MeshInfo) == 32, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::MeshInfo) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::MeshInfo>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::MeshInfo>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::MeshInfo>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::MeshInfo, id) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::MeshInfo, revision) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::MeshInfo, vertex_count) == 16, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::MeshInfo, index_count) == 24, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(Result) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Result) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_same<typename std::underlying_type<Result>::type, unsigned int>::value, "BGCS native enum underlying type differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::FrameInfo) == 40, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::FrameInfo) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::FrameInfo>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::FrameInfo>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::FrameInfo>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::FrameInfo, mesh_update_count) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::FrameInfo, released_mesh_count) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::FrameInfo, command_count) == 16, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::FrameInfo, texture_update_count) == 24, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::FrameInfo, released_texture_count) == 32, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(Inno::UI::RmlUiAdapter::DrawCommand) == 40, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::UI::RmlUiAdapter::DrawCommand) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::UI::RmlUiAdapter::DrawCommand>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::UI::RmlUiAdapter::DrawCommand>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::UI::RmlUiAdapter::DrawCommand>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, mesh_id) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, texture_id) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, scissor_enabled) == 16, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, scissor_x) == 20, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, scissor_y) == 24, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, scissor_width) == 28, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::UI::RmlUiAdapter::DrawCommand, scissor_height) == 32, "BGCS native field offset differs from parsed facts");
#if defined(_MSC_VER)
#define BGCS_VERIFY_CC_C __cdecl
#elif defined(__GNUC__) || defined(__clang__)
#if defined(__i386__)
#define BGCS_VERIFY_CC_C __attribute__((cdecl))
#elif defined(__x86_64__)
#define BGCS_VERIFY_CC_C __attribute__((ms_abi))
#else
#define BGCS_VERIFY_CC_C
#endif
#else
#error BGCS native compiler cannot independently verify callable conventions
#endif
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_0)(unsigned long long, unsigned long long, const char *, const char *, unsigned char, unsigned char &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_0>(&Inno::UI::RmlUiAdapter::Runtime::SetClass)), bgcs_verified_callable_0>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_1)(unsigned long long, Inno::UI::RmlUiAdapter::DrawCommand *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_1>(&Inno::UI::RmlUiAdapter::Runtime::CopyCommands)), bgcs_verified_callable_1>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_2)(const char *, int, int, float, unsigned long long &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_2>(&Inno::UI::RmlUiAdapter::Runtime::CreateContext)), bgcs_verified_callable_2>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_3)(unsigned long long, int, int, int);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_3>(&Inno::UI::RmlUiAdapter::Runtime::ProcessMouseMove)), bgcs_verified_callable_3>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_4)(unsigned long long, Inno::UI::RmlUiAdapter::FrameInfo &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_4>(&Inno::UI::RmlUiAdapter::Runtime::Render)), bgcs_verified_callable_4>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_5)(unsigned long long, float, float, int);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_5>(&Inno::UI::RmlUiAdapter::Runtime::ProcessMouseWheel)), bgcs_verified_callable_5>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_6)(unsigned long long, unsigned long long, unsigned char *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_6>(&Inno::UI::RmlUiAdapter::Runtime::CopyTexturePixels)), bgcs_verified_callable_6>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_7)(unsigned long long, unsigned long long &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_7>(&Inno::UI::RmlUiAdapter::Runtime::GetEventCount)), bgcs_verified_callable_7>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_8)(unsigned long long, int, int, float);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_8>(&Inno::UI::RmlUiAdapter::Runtime::SetViewport)), bgcs_verified_callable_8>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_9)(unsigned long long, const char *, int, int, unsigned char *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_9>(&Inno::UI::RmlUiAdapter::Runtime::RegisterTexture)), bgcs_verified_callable_9>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_10)(unsigned long long, unsigned long long *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_10>(&Inno::UI::RmlUiAdapter::Runtime::CopyReleasedMeshes)), bgcs_verified_callable_10>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_11)(unsigned long long, int, unsigned char, int);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_11>(&Inno::UI::RmlUiAdapter::Runtime::ProcessMouseButton)), bgcs_verified_callable_11>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_12)(unsigned long long, unsigned long long, Inno::UI::RmlUiAdapter::TextureInfo &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_12>(&Inno::UI::RmlUiAdapter::Runtime::GetTextureInfo)), bgcs_verified_callable_12>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_13)(unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_13>(&Inno::UI::RmlUiAdapter::Runtime::ClearEvents)), bgcs_verified_callable_13>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_14)(unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_14>(&Inno::UI::RmlUiAdapter::Runtime::DestroyContext)), bgcs_verified_callable_14>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_15)(unsigned long long, unsigned long long *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_15>(&Inno::UI::RmlUiAdapter::Runtime::CopyReleasedTextures)), bgcs_verified_callable_15>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_16)(unsigned long long, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_16>(&Inno::UI::RmlUiAdapter::Runtime::CloseDocument)), bgcs_verified_callable_16>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_17)(unsigned long long, unsigned long long, const char *, const char *, const char *, unsigned char &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_17>(&Inno::UI::RmlUiAdapter::Runtime::SetAttribute)), bgcs_verified_callable_17>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_18)(unsigned long long, unsigned long long, unsigned int *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_18>(&Inno::UI::RmlUiAdapter::Runtime::CopyMeshIndices)), bgcs_verified_callable_18>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_19)(unsigned long long, int, int, unsigned char &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_19>(&Inno::UI::RmlUiAdapter::Runtime::HasElementAtPoint)), bgcs_verified_callable_19>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_20)(unsigned long long, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_20>(&Inno::UI::RmlUiAdapter::Runtime::HideDocument)), bgcs_verified_callable_20>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_21)(unsigned long long, unsigned long long, Inno::UI::RmlUiAdapter::MeshInfo &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_21>(&Inno::UI::RmlUiAdapter::Runtime::GetMeshInfo)), bgcs_verified_callable_21>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_22)(unsigned long long, int, unsigned char, int);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_22>(&Inno::UI::RmlUiAdapter::Runtime::ProcessKey)), bgcs_verified_callable_22>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_23)(unsigned long long, const char *);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_23>(&Inno::UI::RmlUiAdapter::Runtime::ProcessText)), bgcs_verified_callable_23>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_24)(unsigned long long, unsigned long long, Inno::UI::RmlUiAdapter::Vertex *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_24>(&Inno::UI::RmlUiAdapter::Runtime::CopyMeshVertices)), bgcs_verified_callable_24>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_25)(unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_25>(&Inno::UI::RmlUiAdapter::Runtime::Update)), bgcs_verified_callable_25>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_26)(unsigned long long, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_26>(&Inno::UI::RmlUiAdapter::Runtime::ShowDocument)), bgcs_verified_callable_26>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_27)(unsigned long long, unsigned long long, char *, unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_27>(&Inno::UI::RmlUiAdapter::Runtime::CopyEventTargetId)), bgcs_verified_callable_27>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_28)(unsigned long long, const char *, const char *, unsigned long long &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_28>(&Inno::UI::RmlUiAdapter::Runtime::LoadDocument)), bgcs_verified_callable_28>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_29)(unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_29>(&Inno::UI::RmlUiAdapter::Runtime::FinishFrame)), bgcs_verified_callable_29>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_30)(unsigned long long, unsigned long long, const char *, const char *, unsigned char &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_30>(&Inno::UI::RmlUiAdapter::Runtime::SetInnerMarkup)), bgcs_verified_callable_30>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_31)(const unsigned char *, unsigned long long, const char *, int, int, unsigned char);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_31>(&Inno::UI::RmlUiAdapter::Runtime::LoadFont)), bgcs_verified_callable_31>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_C Inno::UI::RmlUiAdapter::Runtime::*bgcs_verified_callable_32)(unsigned long long, unsigned long long, Inno::UI::RmlUiAdapter::EventInfo &);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_32>(&Inno::UI::RmlUiAdapter::Runtime::GetEventInfo)), bgcs_verified_callable_32>::value, "BGCS native callable signature differs from parsed facts");
typedef EventType bgcs_verified_field_0;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::EventInfo*)nullptr)->type), bgcs_verified_field_0>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_1;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::EventInfo*)nullptr)->document), bgcs_verified_field_1>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_2;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::EventInfo*)nullptr)->target_id_length), bgcs_verified_field_2>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_3;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::TextureInfo*)nullptr)->id), bgcs_verified_field_3>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_4;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::TextureInfo*)nullptr)->revision), bgcs_verified_field_4>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_5;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::TextureInfo*)nullptr)->width), bgcs_verified_field_5>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_6;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::TextureInfo*)nullptr)->height), bgcs_verified_field_6>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_7;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::TextureInfo*)nullptr)->byte_length), bgcs_verified_field_7>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_8;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::Vertex*)nullptr)->x), bgcs_verified_field_8>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_9;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::Vertex*)nullptr)->y), bgcs_verified_field_9>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_10;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::Vertex*)nullptr)->u), bgcs_verified_field_10>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_11;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::Vertex*)nullptr)->v), bgcs_verified_field_11>::value, "BGCS native field type differs from parsed facts");
typedef unsigned int bgcs_verified_field_12;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::Vertex*)nullptr)->color), bgcs_verified_field_12>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_13;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::MeshInfo*)nullptr)->id), bgcs_verified_field_13>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_14;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::MeshInfo*)nullptr)->revision), bgcs_verified_field_14>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_15;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::MeshInfo*)nullptr)->vertex_count), bgcs_verified_field_15>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_16;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::MeshInfo*)nullptr)->index_count), bgcs_verified_field_16>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_17;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::FrameInfo*)nullptr)->mesh_update_count), bgcs_verified_field_17>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_18;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::FrameInfo*)nullptr)->released_mesh_count), bgcs_verified_field_18>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_19;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::FrameInfo*)nullptr)->command_count), bgcs_verified_field_19>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_20;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::FrameInfo*)nullptr)->texture_update_count), bgcs_verified_field_20>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_21;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::FrameInfo*)nullptr)->released_texture_count), bgcs_verified_field_21>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_22;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->mesh_id), bgcs_verified_field_22>::value, "BGCS native field type differs from parsed facts");
typedef unsigned long long bgcs_verified_field_23;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->texture_id), bgcs_verified_field_23>::value, "BGCS native field type differs from parsed facts");
typedef unsigned char bgcs_verified_field_24;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->scissor_enabled), bgcs_verified_field_24>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_25;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->scissor_x), bgcs_verified_field_25>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_26;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->scissor_y), bgcs_verified_field_26>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_27;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->scissor_width), bgcs_verified_field_27>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_28;
static_assert(std::is_same<decltype(((Inno::UI::RmlUiAdapter::DrawCommand*)nullptr)->scissor_height), bgcs_verified_field_28>::value, "BGCS native field type differs from parsed facts");
