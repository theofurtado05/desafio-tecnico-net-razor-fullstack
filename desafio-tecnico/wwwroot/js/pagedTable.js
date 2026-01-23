function loadPage(tableId, page) {
    const event = new CustomEvent('pagedTable:loadPage', {
        detail: { tableId, page }
    });
    window.dispatchEvent(event);
}

function renderTableData(tableId, data, renderRowCallback) {
    const tbody = document.getElementById(tableId + 'Body');
    if (!tbody) return;

    tbody.innerHTML = '';

    if (!data || data.length === 0) {
        const columns = document.querySelector(`#${tableId} thead tr`).children.length;
        tbody.innerHTML = `
            <tr>
                <td colspan="${columns}" class="text-center text-muted py-4">
                    Nenhum registro encontrado
                </td>
            </tr>
        `;
        return;
    }

    data.forEach((item, index) => {
        const row = document.createElement('tr');
        row.setAttribute('data-item-index', index);
        row.innerHTML = renderRowCallback(item, index);
        tbody.appendChild(row);
    });
}