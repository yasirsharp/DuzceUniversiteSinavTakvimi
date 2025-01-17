document.addEventListener('DOMContentLoaded', function () {
    let selectedIds = {
        BolumId: null,
        DersId: null,
        AkademikPersonelId: null
    };

    TiklanabilenTablo('bolumTable', 'Bölüm', 'BolumId');
    TiklanabilenTablo('dersTable', 'Ders', 'DersId');
    TiklanabilenTablo('personelTable', 'Akademik Personel', 'AkademikPersonelId');

    // Kaydet butonuna tıklama işlemi
    document.getElementById('saveButton').addEventListener('click', function () {
        if (selectedIds.BolumId && selectedIds.DersId && selectedIds.AkademikPersonelId) {
            const data = {
                BolumId: selectedIds.BolumId,
                DersId: selectedIds.DersId,
                AkademikPersonelId: selectedIds.AkademikPersonelId
            };

            fetch('/BolumDersAkademikPersonel/Add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => response.json())
                .then(result => {
                    if (result.success) {
                        showToast('success',result.message);

                        // Yeni satırı sağdaki tabloya ekle
                        const tableBody = document.querySelector('table tbody');
                        const newRow = document.createElement('tr');
                        newRow.innerHTML = `<td>${data.BolumId}</td><td>${data.DersId}</td><td>${data.AkademikPersonelId}</td>`;
                        tableBody.appendChild(newRow);

                        // Kaydet butonunu pasif yap ve seçili değerleri sıfırla
                        document.getElementById('saveButton').disabled = true;
                        selectedIds = { BolumId: null, DersId: null, AkademikPersonelId: null };
                    } else {
                        showToast('danger', result.message);
                    }
                })
                .catch(error => showToast('danger', error));
        } else {
            showToast('danger', 'Lütfen her bir tablodan bir seçim yapınız.');
        }
    });

    function TiklanabilenTablo(tableId, columnType, idType) {
        const table = document.getElementById(tableId);
        const targetRow = document.querySelector('#singleRow');
        const saveButton = document.getElementById('saveButton');
        const cells = {
            'Bölüm': targetRow.children[0],
            'Ders': targetRow.children[1],
            'Akademik Personel': targetRow.children[2]
        };

        table.addEventListener('click', function (e) {
            const row = e.target.closest('tr'); // Tıklanan elemanın satırını al
            if (row && row.parentNode.tagName === 'TBODY') {
                const id = row.children[0].textContent.trim(); // ID sütunu
                const name = row.children[1].textContent.trim(); // İsim
                const additionalInfo = row.children[2]?.textContent.trim() || ''; // Ek bilgi (unvan)

                // Seçili ID'yi kaydet
                selectedIds[idType] = parseInt(id);

                // İlgili hücreye yaz
                if (columnType === 'Akademik Personel') {
                    cells[columnType].textContent = `${name} (${additionalInfo})`;
                } else {
                    cells[columnType].textContent = name;
                }

                // Satırı görünür yap
                targetRow.style.display = '';
                saveButton.disabled = false;

                // Önceki seçili satırın stilini temizle
                const selectedRow = table.querySelector('.selected-row');
                if (selectedRow) {
                    selectedRow.classList.remove('selected-row');
                }

                // Yeni seçili satıra stil uygula
                row.classList.add('selected-row');
            }
        });
    }
});
// Toast bildirim fonksiyonu
function showToast(type, message) {
    const toastContainer = document.getElementById('toast-container');

    const toast = document.createElement('div');
    toast.classList.add('toast', 'fade', 'show', `bg-${type}`, 'text-white');
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    toast.innerHTML = `<div class="toast-body">${message}</div>`;

    toastContainer.appendChild(toast);

    setTimeout(() => {
        toast.classList.remove('show');
        toast.classList.add('hide');
        setTimeout(() => toast.remove(), 300);
    }, 5000);
}