import { dotnet } from './_framework/dotnet.js';

const canvas = document.getElementById('canvas');
const status = document.getElementById('status');
const error = document.getElementById('error');
let pendingFrame;

function showError(message) {
    error.textContent = message;
    error.style.display = 'block';
    status.textContent = '';
}

function nextFrame() {
    if (pendingFrame) {
        throw new Error('A presentation opportunity is already pending.');
    }
    return new Promise(resolve => {
        const complete = () => {
            cancelAnimationFrame(frame);
            document.removeEventListener('visibilitychange', complete);
            pendingFrame = undefined;
            resolve();
        };
        const frame = requestAnimationFrame(complete);
        pendingFrame = complete;
        document.addEventListener('visibilitychange', complete, { once: true });
    });
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
            nextFrame,
            cancelFrame: () => pendingFrame?.(),
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
