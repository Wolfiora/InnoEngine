
#ifndef inno_text_COMMON_H
#define inno_text_COMMON_H

#include <stdint.h>
#include <stddef.h>
/* Calling convention */

#if defined(_WIN32) || defined(_WIN64)
#define inno_text_CALL __cdecl
#else
#define inno_text_CALL
#endif

/* API export/import */
#if defined(_WIN32) || defined(_WIN64)
#define inno_text_EXPORT_INTERNAL __declspec(dllexport)
#ifdef inno_text_BUILD_SHARED
#define inno_text_EXPORT __declspec(dllexport)
#else
#define inno_text_EXPORT __declspec(dllimport)
#endif
#elif defined(__GNUC__) || defined(__clang__)
#define inno_text_EXPORT_INTERNAL __attribute__((visibility("default")))
#ifdef inno_text_BUILD_SHARED
#define inno_text_EXPORT __attribute__((visibility("default")))
#else
#define inno_text_EXPORT
#endif
#else
#define inno_text_EXPORT_INTERNAL
#define inno_text_EXPORT
#endif

#if defined __cplusplus
#define inno_text_EXTERN extern "C"
#else
#include <stdarg.h>
#include <stdbool.h>
#define inno_text_EXTERN extern
#endif

#define inno_text_API(type) inno_text_EXTERN inno_text_EXPORT type inno_text_CALL
#define inno_text_API_INTERNAL(type) inno_text_EXTERN inno_text_EXPORT_INTERNAL type inno_text_CALL

inno_text_API(const char*) inno_text_GetLastError(void);
inno_text_API(void) inno_text_ClearLastError(void);

#endif
