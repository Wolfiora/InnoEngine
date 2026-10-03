import { dotnet } from './_framework/dotnet.js';

const canvas = document.getElementById('canvas');
const status = document.getElementById('status');
const error = document.getElementById('error');

function showError(message) {
    error.textContent = message;
    error.style.display = 'block';
    status.textContent = '';
}

try {
    const probe = document.createElement('canvas');
    if (!probe.getContext('webgl2')) {
        throw new Error('This game requires a browser with WebGL 2 enabled.');
    }

    canvas.addEventListener('pointerdown', () => canvas.focus());
    const { setModuleImports, runMain } = await dotnet.create();
    setModuleImports('browser-player.js', {
        host: {
            baseUrl: () => new URL('./', document.baseURI).href,
            nextFrame: () => new Promise(resolve => requestAnimationFrame(resolve)),
            status: message => { status.textContent = message; },
            error: showError
        },
        storage: {
            get: key => localStorage.getItem(key),
            set: (key, value) => localStorage.setItem(key, value),
            remove: key => localStorage.removeItem(key),
            count: () => localStorage.length,
            keyAt: index => localStorage.key(index)
        }
    });
    await runMain();
} catch (failure) {
    showError(String(failure));
}
