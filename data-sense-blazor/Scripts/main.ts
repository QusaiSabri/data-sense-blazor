import Split from 'split.js';

export function setupSplit(horizontalPaneSelectors: string[], verticalPaneSelectors: string[]): void {
    const horizontalSelectors = horizontalPaneSelectors.map(id => '#' + id);
    const verticalSelectors = verticalPaneSelectors.map(id => '#' + id);

    Split(horizontalSelectors, {
        gutterSize: 8,
        cursor: 'col-resize',
        sizes: [25, 75],
        minSize: [200, 400],
        onDragEnd: () => updateMudTableHeight()
    });

    Split(verticalSelectors, {
        gutterSize: 8,
        direction: 'vertical',
        onDrag: () => updateMudTableHeight()
    });
}

export function updateMudTableHeight(): void {
    const pane = document.getElementById('dataSenseBottomRightPane');
    const tableContainer = pane?.querySelector('.mud-table-container');

    if (tableContainer) {
        const paneRect = pane?.getBoundingClientRect();
        const tableContainerRect = tableContainer.getBoundingClientRect();
        const newHeight = paneRect.height - (tableContainerRect.top - paneRect.top);
        tableContainer.style.height = `${newHeight - 2}px`;
    }
}

(window as any).setupSplit = setupSplit;
(window as any).updateMudTableHeight = updateMudTableHeight;
