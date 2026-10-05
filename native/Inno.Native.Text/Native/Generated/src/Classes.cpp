#ifndef inno_text_BUILD_SHARED
#define inno_text_BUILD_SHARED 1
#endif
#include "Classes.h"
#include <algorithm>
#include <exception>
#include <iterator>
#include <stdexcept>
#include <string>
#include <utility>
#include "TextRuntime.hpp"

static thread_local std::string inno_text_last_error;

inno_text_API_INTERNAL(const char*) inno_text_GetLastError(void)
{
	return inno_text_last_error.c_str();
}
inno_text_API_INTERNAL(void) inno_text_ClearLastError(void)
{
	inno_text_last_error.clear();
}
inno_text_API_INTERNAL(inno_text_Runtime*) inno_text_RuntimeCreate(void)
{
	try
	{
		inno_text_ClearLastError();
		return reinterpret_cast<inno_text_Runtime*>(new Inno::Text::FontAdapter::Runtime());
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return nullptr;
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return nullptr;
	}
}
inno_text_API_INTERNAL(void) inno_text_RuntimeDestroy(inno_text_Runtime* self)
{
	try
	{
		inno_text_ClearLastError();
		auto* ptr = reinterpret_cast<Inno::Text::FontAdapter::Runtime*>(self);
		delete ptr;
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return;
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return;
	}
}
inno_text_API_INTERNAL(inno_text_Result) inno_text_Runtime_LoadFont(inno_text_Runtime* self, const unsigned char* data, unsigned int length, int face_index, unsigned long long* font_handle)
{
	try
	{
		inno_text_ClearLastError();
		auto* ptr = reinterpret_cast<Inno::Text::FontAdapter::Runtime*>(self);
		return static_cast<inno_text_Result>(ptr->LoadFont(reinterpret_cast<const unsigned char*>(data), length, face_index, reinterpret_cast<unsigned long long*>(font_handle)));
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return {};
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return {};
	}
}
inno_text_API_INTERNAL(inno_text_Result) inno_text_Runtime_ReleaseFont(inno_text_Runtime* self, unsigned long long font_handle)
{
	try
	{
		inno_text_ClearLastError();
		auto* ptr = reinterpret_cast<Inno::Text::FontAdapter::Runtime*>(self);
		return static_cast<inno_text_Result>(ptr->ReleaseFont(font_handle));
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return {};
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return {};
	}
}
inno_text_API_INTERNAL(inno_text_Result) inno_text_Runtime_GetMetrics(inno_text_Runtime* self, unsigned long long font_handle, float font_size, inno_text_Metrics* metrics)
{
	try
	{
		inno_text_ClearLastError();
		auto* ptr = reinterpret_cast<Inno::Text::FontAdapter::Runtime*>(self);
		return static_cast<inno_text_Result>(ptr->GetMetrics(font_handle, font_size, reinterpret_cast<Inno::Text::FontAdapter::Metrics*>(metrics)));
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return {};
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return {};
	}
}
inno_text_API_INTERNAL(inno_text_Result) inno_text_Runtime_ShapeUtf8(inno_text_Runtime* self, unsigned long long font_handle, const char* text, unsigned int text_length, float font_size, inno_text_Direction direction, const char* language, const char* script, inno_text_Glyph* glyphs, unsigned int glyph_capacity, unsigned int* glyph_count)
{
	try
	{
		inno_text_ClearLastError();
		auto* ptr = reinterpret_cast<Inno::Text::FontAdapter::Runtime*>(self);
		return static_cast<inno_text_Result>(ptr->ShapeUtf8(font_handle, reinterpret_cast<const char*>(text), text_length, font_size, static_cast<Direction>(direction), reinterpret_cast<const char*>(language), reinterpret_cast<const char*>(script), reinterpret_cast<Inno::Text::FontAdapter::Glyph*>(glyphs), glyph_capacity, reinterpret_cast<unsigned int*>(glyph_count)));
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return {};
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return {};
	}
}
inno_text_API_INTERNAL(inno_text_Result) inno_text_Runtime_RasterizeGlyph(inno_text_Runtime* self, unsigned long long font_handle, unsigned int glyph_id, float font_size, unsigned char* pixels, unsigned int pixel_capacity, inno_text_Bitmap* bitmap)
{
	try
	{
		inno_text_ClearLastError();
		auto* ptr = reinterpret_cast<Inno::Text::FontAdapter::Runtime*>(self);
		return static_cast<inno_text_Result>(ptr->RasterizeGlyph(font_handle, glyph_id, font_size, reinterpret_cast<unsigned char*>(pixels), pixel_capacity, reinterpret_cast<Inno::Text::FontAdapter::Bitmap*>(bitmap)));
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return {};
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return {};
	}
}
inno_text_API_INTERNAL(const char*) inno_text_Runtime_ResultMessage(inno_text_Result result)
{
	try
	{
		inno_text_ClearLastError();
		return reinterpret_cast<const char*>(Inno::Text::FontAdapter::Runtime::ResultMessage(static_cast<Result>(result)));
	}
	catch (const std::exception& exception)
	{
		inno_text_last_error = exception.what();
		return nullptr;
	}
	catch (...)
	{
		inno_text_last_error = "Unknown C++ exception";
		return nullptr;
	}
}
