import { defineConfig } from 'vite';
import { viteStaticCopy } from 'vite-plugin-static-copy';

export default defineConfig({
    build: {
        outDir: '../wwwroot/dist',
        emptyOutDir: true,
        assetsDir: 'assets',
    },
    assetsInclude: ['**/*.wasm'],
    publicDir: 'public',
    plugins: [
        viteStaticCopy({
            targets: [
                {
                    src: 'node_modules/web-ifc/web-ifc.wasm',
                    dest: 'assets/lib/web-ifc'
                }
            ]
        })
    ]
});