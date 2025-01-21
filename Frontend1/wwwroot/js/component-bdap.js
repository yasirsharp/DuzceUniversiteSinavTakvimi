document.addEventListener('DOMContentLoaded', function () {
    // Durum değişkenleri
    const state = {
        selectedIds: {
            BolumId: null,
            DersId: null,
            AkademikPersonelId: null
        },
        selectedBDAP: null
    };

    // DOM elementleri
    const elements = {
        saveButton: document.getElementById('saveButton'),
        singleRow: document.querySelector('#singleRow'),
        bdapTable: document.getElementById('bdapTable')
    };

    // Event Listeners
    initializeTableListeners();
    initializeSaveButton();

    function initializeTableListeners() {
        // Sol tablolar için click listener'ları
        TiklanabilenTablo('bolumTable', 'Bölüm', 'BolumId');
        TiklanabilenTablo('dersTable', 'Ders', 'DersId');
        TiklanabilenTablo('personelTable', 'Akademik Personel', 'AkademikPersonelId');

        // Sağ tablo için click listener
        $('#bdapTable tbody tr').click(function(e) {
            if (e.target.tagName.toLowerCase() === 'button') return; // Butonlara tıklamayı engelle
            
            $('#bdapTable tbody tr').removeClass('selected-row');
            $(this).addClass('selected-row');
            
            const id = $(this).data('id');
            if (id) getBDAPDetail(id);
        });
    }

    function TiklanabilenTablo(tableId, columnType, idType) {
        const table = document.getElementById(tableId);
        const cells = {
            'Bölüm': elements.singleRow.children[0],
            'Ders': elements.singleRow.children[1],
            'Akademik Personel': elements.singleRow.children[2]
        };

        table.addEventListener('click', function (e) {
            const row = e.target.closest('tr');
            if (!row || row.parentNode.tagName !== 'TBODY') return;

            const id = row.children[0].textContent.trim();
            const name = row.children[1].textContent.trim();
            const additionalInfo = row.children[2]?.textContent.trim() || '';

            updateSelectedRow(row, table, idType, id);
            updateTargetCell(columnType, name, additionalInfo);
            
            state.selectedIds[idType] = parseInt(id);
            updateSaveButtonState();
        });
    }

    function updateSelectedRow(newRow, table, idType, id) {
        const selectedRow = table.querySelector('.selected-row');
        if (selectedRow) selectedRow.classList.remove('selected-row');
        newRow.classList.add('selected-row');
    }

    function updateTargetCell(columnType, name, additionalInfo) {
        const cells = {
            'Bölüm': elements.singleRow.children[0],
            'Ders': elements.singleRow.children[1],
            'Akademik Personel': elements.singleRow.children[2]
        };

        cells[columnType].textContent = columnType === 'Akademik Personel' 
            ? `${name} (${additionalInfo})` 
            : name;

        elements.singleRow.style.display = '';
    }

    function updateSaveButtonState() {
        elements.saveButton.disabled = !Object.values(state.selectedIds)
            .every(id => id !== null);
    }

    function initializeSaveButton() {
        elements.saveButton.addEventListener('click', handleSave);
    }

    function handleSave() {
        if (!Object.values(state.selectedIds).every(id => id !== null)) {
            showAlert('warning', 'Lütfen her bir tablodan bir seçim yapınız.');
            return;
        }

        fetch('/BolumDersAkademikPersonel/Add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(state.selectedIds)
        })
        .then(response => response.json())
        .then(handleSaveResponse)
        .catch(error => showAlert('error', 'Bir hata oluştu: ' + error));
    }

    function handleSaveResponse(result) {
        if (result.success) {
            showAlert('success', result.message).then(() => {
                location.reload();
            });
            resetSelections();
        } else {
            showAlert('error', result.message || 'Kayıt sırasında bir hata oluştu');
        }
    }

    function resetSelections() {
        state.selectedIds = { BolumId: null, DersId: null, AkademikPersonelId: null };
        elements.saveButton.disabled = true;
        elements.singleRow.style.display = 'none';
        $('.table tbody tr').removeClass('selected-row');
    }

    function getBDAPDetail(id) {
        fetch(`/BolumDersAkademikPersonel/GetDetail/${id}`, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                updateSelectedRows(data.data);
            } else {
                showAlert('error', 'Detay bilgileri alınamadı');
            }
        })
        .catch(error => showAlert('error', 'Bir hata oluştu: ' + error));
    }

    function updateSelectedRows(detailData) {
        $('.table tbody tr').removeClass('selected-row');
        
        $(`#bolumTable tbody tr[data-id="${detailData.bolumId}"]`).addClass('selected-row');
        $(`#dersTable tbody tr[data-id="${detailData.dersId}"]`).addClass('selected-row');
        $(`#personelTable tbody tr[data-id="${detailData.akademikPersonelId}"]`).addClass('selected-row');
    }

    
});
function handleDelete(item) {
    event.stopPropagation(); // Tıklamanın satır seçimine etki etmemesi için
    
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu eşleşmeyi silmek istediğinize emin misiniz?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteRecord(item);
        } else {
            console.log(item);
            console.log("a");
            showAlert('warning', 'Silme işlemi iptal edildi');
        }
    });
}

function deleteRecord(item) {
    console.log(item);
    fetch(`/BolumDersAkademikPersonel/Delete/`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
    .then(response => response.json())
    .then(result => {
        console.log(result);
        if (result.success) {
            console.log("b");
            showAlert('success', 'Kayıt başarıyla silindi').then(() => {
                location.reload();
            });
        } else {
            showAlert('error', result.message);
        }
    })
    .catch(error => {
        console.log(error);
        showAlert('error', 'Silme işlemi sırasında bir hata oluştu: ' + error);
    });
}

function showAlert(type, message) {
    return new Promise((resolve) => {
        Swal.fire({
            icon: type === 'success' ? 'success' : type === 'danger' ? 'error' : 'warning',
            title: type === 'success' ? 'Başarılı!' : type === 'danger' ? 'Hata!' : 'Uyarı!',
            text: message,
            timer: type === 'success' ? 700 : 5000,
            showConfirmButton: false,
            position: 'top-end',
            toast: true
        }).then(() => {
            resolve();
        });
    });
}