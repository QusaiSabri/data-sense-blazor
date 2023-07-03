import { defineConfig } from 'vite'
import tsconfigPaths from 'vite-tsconfig-paths'

export default defineConfig({
    plugins: [tsconfigPaths()],
    build: {
        lib: {
            entry: 'Scripts/main.ts',
            name: 'MyLibrary',
            formats: ['iife'],
            fileName: 'bundle'
        },
        outDir: 'wwwroot/js',
    }
});