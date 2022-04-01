function deleteRowWithId(rowId, tableId) {
    const table = document.getElementById(tableId)
    for(let i = 0; i < table.rows.length; i++)
        if (table.rows[i].id === rowId){
            table.deleteRow(i)
            break
        }
}

async function cancelTransfer(transferID, cancelerID, url) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            transferId: transferID,
            userId: cancelerID
        })
    })
    
    if (response.status === 200){
        deleteRowWithId('row' + transferID, 'tableBody')
        alert('Operation is successful')
    } 
    else if (response.status === 400) alert('Transfer not found') 
    else if (response.status === 405) alert('Status change is not allowed')
}

async function fetchKyc(kycId, url) {
    const response = await fetch(url + '/' + kycId, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            requestId: kycId,
        })
    })
    if (response.status === 200){
        deleteRowWithId('row' + kycId, 'tableBody')
        alert('Operation is successful')
    }
    else if (response.status === 400) alert('KYC request not found')
}