/* BGCS compiler verification of parsed native storage facts. */

#include <stddef.h>
#include <type_traits>
#include "TextRuntime.hpp"


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
static_assert(sizeof(float) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(float) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(sizeof(Direction) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Direction) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_same<typename std::underlying_type<Direction>::type, unsigned int>::value, "BGCS native enum underlying type differs from parsed facts");
static_assert(sizeof(Inno::Text::FontAdapter::Runtime) == 8, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::Text::FontAdapter::Runtime) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::Text::FontAdapter::Runtime>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::Text::FontAdapter::Runtime>::value == 0, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::Text::FontAdapter::Runtime>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(sizeof(unsigned long long) == 8, "BGCS native storage size differs from parsed facts");
static_assert(alignof(unsigned long long) == 8, "BGCS native storage alignment differs from parsed facts");
static_assert((((unsigned long long)-1) < ((unsigned long long)1)) == 0, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(char) == 1, "BGCS native storage size differs from parsed facts");
static_assert(alignof(char) == 1, "BGCS native storage alignment differs from parsed facts");
static_assert((((char)-1) < ((char)1)) == 1, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(Inno::Text::FontAdapter::Metrics) == 20, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::Text::FontAdapter::Metrics) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::Text::FontAdapter::Metrics>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::Text::FontAdapter::Metrics>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::Text::FontAdapter::Metrics>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Metrics, ascender) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Metrics, descender) == 4, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Metrics, line_height) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Metrics, underline_position) == 12, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Metrics, underline_thickness) == 16, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(unsigned char) == 1, "BGCS native storage size differs from parsed facts");
static_assert(alignof(unsigned char) == 1, "BGCS native storage alignment differs from parsed facts");
static_assert((((unsigned char)-1) < ((unsigned char)1)) == 0, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(unsigned int) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(unsigned int) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert((((unsigned int)-1) < ((unsigned int)1)) == 0, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(int) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(int) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert((((int)-1) < ((int)1)) == 1, "BGCS native integral signedness differs from parsed facts");
static_assert(sizeof(Result) == 4, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Result) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_same<typename std::underlying_type<Result>::type, unsigned int>::value, "BGCS native enum underlying type differs from parsed facts");
static_assert(sizeof(Inno::Text::FontAdapter::Bitmap) == 24, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::Text::FontAdapter::Bitmap) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::Text::FontAdapter::Bitmap>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::Text::FontAdapter::Bitmap>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::Text::FontAdapter::Bitmap>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Bitmap, width) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Bitmap, height) == 4, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Bitmap, bearing_x) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Bitmap, bearing_y) == 12, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Bitmap, advance_x) == 16, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Bitmap, byte_length) == 20, "BGCS native field offset differs from parsed facts");
static_assert(sizeof(Inno::Text::FontAdapter::Glyph) == 24, "BGCS native storage size differs from parsed facts");
static_assert(alignof(Inno::Text::FontAdapter::Glyph) == 4, "BGCS native storage alignment differs from parsed facts");
static_assert(std::is_standard_layout<Inno::Text::FontAdapter::Glyph>::value == 1, "BGCS native standard-layout trait differs from parsed facts");
static_assert(std::is_trivially_copyable<Inno::Text::FontAdapter::Glyph>::value == 1, "BGCS native trivial-copy trait differs from parsed facts");
static_assert(std::is_default_constructible<Inno::Text::FontAdapter::Glyph>::value == 1, "BGCS native default-construction trait differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Glyph, glyph_id) == 0, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Glyph, cluster) == 4, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Glyph, advance_x) == 8, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Glyph, advance_y) == 12, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Glyph, offset_x) == 16, "BGCS native field offset differs from parsed facts");
static_assert(BGCS_NATIVE_FIELD_OFFSET(Inno::Text::FontAdapter::Glyph, offset_y) == 20, "BGCS native field offset differs from parsed facts");
#if defined(_MSC_VER)
#define BGCS_VERIFY_CC_Win64 __cdecl
#elif defined(__GNUC__) || defined(__clang__)
#define BGCS_VERIFY_CC_Win64 __attribute__((ms_abi))
#else
#error BGCS native compiler cannot independently verify callable conventions
#endif
typedef Result (BGCS_VERIFY_CC_Win64 Inno::Text::FontAdapter::Runtime::*bgcs_verified_callable_0)(unsigned long long, unsigned int, float, unsigned char *, unsigned int, Inno::Text::FontAdapter::Bitmap *);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_0>(&Inno::Text::FontAdapter::Runtime::RasterizeGlyph)), bgcs_verified_callable_0>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_Win64 Inno::Text::FontAdapter::Runtime::*bgcs_verified_callable_1)(unsigned long long);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_1>(&Inno::Text::FontAdapter::Runtime::ReleaseFont)), bgcs_verified_callable_1>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_Win64 Inno::Text::FontAdapter::Runtime::*bgcs_verified_callable_2)(const unsigned char *, unsigned int, int, unsigned long long *);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_2>(&Inno::Text::FontAdapter::Runtime::LoadFont)), bgcs_verified_callable_2>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_Win64 Inno::Text::FontAdapter::Runtime::*bgcs_verified_callable_3)(unsigned long long, const char *, unsigned int, float, Direction, const char *, const char *, Inno::Text::FontAdapter::Glyph *, unsigned int, unsigned int *);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_3>(&Inno::Text::FontAdapter::Runtime::ShapeUtf8)), bgcs_verified_callable_3>::value, "BGCS native callable signature differs from parsed facts");
typedef Result (BGCS_VERIFY_CC_Win64 Inno::Text::FontAdapter::Runtime::*bgcs_verified_callable_4)(unsigned long long, float, Inno::Text::FontAdapter::Metrics *);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_4>(&Inno::Text::FontAdapter::Runtime::GetMetrics)), bgcs_verified_callable_4>::value, "BGCS native callable signature differs from parsed facts");
typedef const char * (BGCS_VERIFY_CC_Win64 *bgcs_verified_callable_5)(Result);
static_assert(std::is_same<decltype(static_cast<bgcs_verified_callable_5>(&Inno::Text::FontAdapter::Runtime::ResultMessage)), bgcs_verified_callable_5>::value, "BGCS native callable signature differs from parsed facts");
typedef float bgcs_verified_field_0;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Metrics*)nullptr)->ascender), bgcs_verified_field_0>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_1;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Metrics*)nullptr)->descender), bgcs_verified_field_1>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_2;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Metrics*)nullptr)->line_height), bgcs_verified_field_2>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_3;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Metrics*)nullptr)->underline_position), bgcs_verified_field_3>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_4;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Metrics*)nullptr)->underline_thickness), bgcs_verified_field_4>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_5;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Bitmap*)nullptr)->width), bgcs_verified_field_5>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_6;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Bitmap*)nullptr)->height), bgcs_verified_field_6>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_7;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Bitmap*)nullptr)->bearing_x), bgcs_verified_field_7>::value, "BGCS native field type differs from parsed facts");
typedef int bgcs_verified_field_8;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Bitmap*)nullptr)->bearing_y), bgcs_verified_field_8>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_9;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Bitmap*)nullptr)->advance_x), bgcs_verified_field_9>::value, "BGCS native field type differs from parsed facts");
typedef unsigned int bgcs_verified_field_10;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Bitmap*)nullptr)->byte_length), bgcs_verified_field_10>::value, "BGCS native field type differs from parsed facts");
typedef unsigned int bgcs_verified_field_11;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Glyph*)nullptr)->glyph_id), bgcs_verified_field_11>::value, "BGCS native field type differs from parsed facts");
typedef unsigned int bgcs_verified_field_12;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Glyph*)nullptr)->cluster), bgcs_verified_field_12>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_13;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Glyph*)nullptr)->advance_x), bgcs_verified_field_13>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_14;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Glyph*)nullptr)->advance_y), bgcs_verified_field_14>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_15;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Glyph*)nullptr)->offset_x), bgcs_verified_field_15>::value, "BGCS native field type differs from parsed facts");
typedef float bgcs_verified_field_16;
static_assert(std::is_same<decltype(((Inno::Text::FontAdapter::Glyph*)nullptr)->offset_y), bgcs_verified_field_16>::value, "BGCS native field type differs from parsed facts");
