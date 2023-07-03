import Split from 'split.js';

export function setupSplit(horizontalPaneSelectors: string[], verticalPaneSelectors: string[]): void {
    const selectors = horizontalPaneSelectors.map(id => '#' + id);
    Split(selectors, {
        gutterSize: 8,
        cursor: 'col-resize',
        sizes: [25, 75],
        minSize: [200, 400],
    });

    const verticalSelectors = verticalPaneSelectors.map(id => '#' + id);
    Split(verticalSelectors, {
        gutterSize: 8,
        //cursor: 'col-resize',
        //sizes: [50, 50],
        //minSize: [200, 200],
        direction: 'vertical',
    });
}

(window as any).setupSplit = setupSplit;
