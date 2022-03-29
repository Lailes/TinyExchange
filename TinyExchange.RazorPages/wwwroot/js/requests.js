function deleteRowWithId(rowId, tableId) {
    const table = document.getElementById(tableId)
    for(let i = 0; i < table.rows.length; i++)
        if (table.rows[i].id === rowId){
            table.deleteRow(i)
            break;
        }
}

function cancelTransfer(transferID, cancelerID, url) {
    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            transferId: transferID,
            cancelerId: cancelerID
        })
    }).then(response => {
        if (response.status === 200){
            deleteRowWithId('row' + transferID, 'tableBody')
            alert('Cancel is successful')
        } else if (response.status === 400) alert('Transfer not found') 
        else if (response.status === 405) alert('Status change is not allowed')
    })
}