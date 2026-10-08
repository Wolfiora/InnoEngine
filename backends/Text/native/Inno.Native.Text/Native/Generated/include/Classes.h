#include "common.h"
#include "enums.h"

typedef struct inno_text_Runtime inno_text_Runtime;

typedef struct
{
	unsigned int glyph_id;
	unsigned int cluster;
	float advance_x;
	float advance_y;
	float offset_x;
	float offset_y;
} inno_text_Glyph;
typedef struct
{
	float ascender;
	float descender;
	float line_height;
	float underline_position;
	float underline_thickness;
} inno_text_Metrics;
typedef struct
{
	int width;
	int height;
	int bearing_x;
	int bearing_y;
	float advance_x;
	unsigned int byte_length;
} inno_text_Bitmap;
inno_text_API(inno_text_Runtime*) inno_text_RuntimeCreate(void);
inno_text_API(void) inno_text_RuntimeDestroy(inno_text_Runtime* self);
inno_text_API(inno_text_Result) inno_text_Runtime_LoadFont(inno_text_Runtime* self, const unsigned char* data, unsigned int length, int face_index, unsigned long long* font_handle);
inno_text_API(inno_text_Result) inno_text_Runtime_ReleaseFont(inno_text_Runtime* self, unsigned long long font_handle);
inno_text_API(inno_text_Result) inno_text_Runtime_GetMetrics(inno_text_Runtime* self, unsigned long long font_handle, float font_size, inno_text_Metrics* metrics);
inno_text_API(inno_text_Result) inno_text_Runtime_ShapeUtf8(inno_text_Runtime* self, unsigned long long font_handle, const char* text, unsigned int text_length, float font_size, inno_text_Direction direction, const char* language, const char* script, inno_text_Glyph* glyphs, unsigned int glyph_capacity, unsigned int* glyph_count);
inno_text_API(inno_text_Result) inno_text_Runtime_RasterizeGlyph(inno_text_Runtime* self, unsigned long long font_handle, unsigned int glyph_id, float font_size, unsigned char* pixels, unsigned int pixel_capacity, inno_text_Bitmap* bitmap);
inno_text_API(const char*) inno_text_Runtime_ResultMessage(inno_text_Result result);
