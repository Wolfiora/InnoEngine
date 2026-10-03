#include <stdint.h>

// .NET 9's Emscripten 3.1.56 runtime archives omit the setjmp symbols emitted by its LLVM compiler.
// Match that SDK's wasm SjLj jump-buffer layout at the static link boundary. Other targets never link this shim.

struct InnoWasmLongjmpArgs
{
    void* env;
    int value;
};

struct InnoWasmJmpBuf
{
    void* invocation;
    uint32_t label;
    struct InnoWasmLongjmpArgs args;
};

void __wasm_setjmp(void* environment, uint32_t label, void* invocation)
{
    struct InnoWasmJmpBuf* buffer = environment;
    buffer->invocation = invocation;
    buffer->label = label;
}

uint32_t __wasm_setjmp_test(void* environment, void* invocation)
{
    struct InnoWasmJmpBuf* buffer = environment;
    return buffer->invocation == invocation ? buffer->label : 0;
}
