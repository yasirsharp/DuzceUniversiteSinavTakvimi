// Satıra dokununca nesne silme
function initializeDeleteButtons(entityName) {
    const deleteButtons = document.querySelectorAll('.delete-btn');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function () {
            const itemId = this.getAttribute('data-id');
            const itemName = this.getAttribute('data-name');
            if (confirm(`Bu ${itemName} silmek istediğinizden emin misiniz?`)) {
                fetch(`/${entityName}/GetById/${itemId}`)
                    .then(response => response.json())
                    .then(item => {
                        deleteItem(item, entityName);  // Entity parametresi ile işlem yapıyoruz
                    })
                    .catch(error => {
                        console.error(`${entityName} bilgisi alınırken hata oluştu:`, error);
                    });
            }
        });
    });
}

function deleteItem(item, entityName) {
    fetch(`/${entityName}/Delete`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    }).then(response => {
        if (response.ok) {
            showToast('success', `${entityName} başarıyla silindi!`);
            location.reload(); // Sayfayı yeniden yükle
        } else {
            showToast('danger', `${entityName} silme işlemi başarısız oldu!`);
        }
    }).catch(error => {
        console.error(`${entityName} silme sırasında hata:`, error);
        showToast('danger', 'Bir hata oluştu!');
    });
}

// Ekleme işlemi
function initializeAddButton(entityName) {
    document.getElementById("addRowBtn").addEventListener("click", function () {
        // Input satırını göster
        document.getElementById("inputRow").style.display = "table-row-group";
    });

    document.getElementById("submitBtn").addEventListener("click", function () {
        var ad = document.getElementById("ad").value;
        if (entityName === 'AkademikPersonel') var unvan = document.getElementById("unvan").value;
        if (entityName === 'Derslik') var kapasite = document.getElementById("kapasite").value;

        if (ad) {
            var entity;
            // Entity türünde bir nesne oluşturuyoruz
            if (entityName === 'AkademikPersonel') {
                entity = {
                    ad: ad,
                    unvan: unvan
                };
            } else if (entityName==='Derslik') {
                entity = {
                    ad: ad,
                    kapasite: kapasite
                };
            } else {
                entity = {
                    ad: ad
                };
            }

            // Fetch ile entity nesnesini backend'e gönder
            fetch(`/${entityName}/Add`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(entity) // JSON olarak gönderiyoruz
            })
                .then(response => {
                    if (response.ok) {
                        showToast('success', `${entityName} başarıyla eklendi!`);
                        location.reload(); // Sayfayı yenile
                    } else {
                        showToast('danger', `${entityName} eklenirken bir hata oluştu!`);
                    }
                })
                .catch(error => {
                    console.error("Hata:", error);
                    showToast('danger', `Bir hata oluştu: ${error.message}`);
                });
        }
        else {
            showToast('danger', 'Lütfen bir isim girin!');
        }
    });
}


// Düzenleme işlemi
function initializeEditButtons(entityName) {
    document.querySelector('#dataTable').addEventListener('click', function (event) {
        if (event.target.classList.contains('edit-btn')) {
            const button = event.target;
            const itemId = button.getAttribute('data-id');
            const itemName = button.getAttribute('data-name');
            const row = button.closest('tr');
            const itemNameCell = row.querySelector('td:nth-child(2)');

            // Burada itemId ve itemName'den başka, tüm nesneye erişim sağlamak için
            // itemId'yi kullanarak gerekli verileri çekiyoruz.
            fetch(`/${entityName}/GetById/${itemId}`)
                .then(response => response.json())
                .then(item => {
                    console.log(item);
                    const input = document.createElement('input');
                    input.type = 'text';
                    input.classList.add('form-control');
                    input.value = item.ad; // item.Name, örneğin 'Bolum' veya 'Ders' nesnesi içinde

                    itemNameCell.innerHTML = '';
                    itemNameCell.appendChild(input);

                    button.style.display = 'none';

                    const saveButton = document.createElement('button');
                    saveButton.classList.add('btn', 'btn-success', 'btn-sm');
                    saveButton.textContent = 'Kaydet';

                    const cancelButton = document.createElement('button');
                    cancelButton.classList.add('btn', 'btn-danger', 'btn-sm');
                    cancelButton.textContent = 'İptal';

                    const actionCell = row.querySelector('td:nth-child(3)');
                    actionCell.innerHTML = '';
                    actionCell.appendChild(saveButton);
                    actionCell.appendChild(cancelButton);

                    saveButton.addEventListener('click', function () {
                        const newName = input.value;

                        // Burada item nesnesini güncelliyoruz
                        item.Name = newName;  // item'ı yeni değerle güncelliyoruz
                        updateItem(item, entityName);  // item nesnesini gönderiyoruz

                        // Tabloyu güncelliyoruz
                        itemNameCell.innerHTML = newName;

                        const newEditButton = document.createElement('button');
                        newEditButton.classList.add('btn', 'btn-warning', 'btn-sm', 'edit-btn');
                        newEditButton.textContent = 'Düzenle';
                        newEditButton.setAttribute('data-id', item.Id);
                        newEditButton.setAttribute('data-name', newName);

                        const newDeleteButton = document.createElement('button');
                        newDeleteButton.classList.add('btn', 'btn-danger', 'btn-sm', 'delete-btn');
                        newDeleteButton.textContent = 'Sil';
                        newDeleteButton.setAttribute('data-id', item.Id);

                        actionCell.innerHTML = '';
                        actionCell.appendChild(newEditButton);
                        actionCell.appendChild(newDeleteButton);

                        showToast('success', `${entityName} başarıyla güncellendi!`);
                    });

                    cancelButton.addEventListener('click', function () {
                        itemNameCell.innerHTML = item.Name;

                        const newEditButton = document.createElement('button');
                        newEditButton.classList.add('btn', 'btn-warning', 'btn-sm', 'edit-btn');
                        newEditButton.textContent = 'Düzenle';
                        newEditButton.setAttribute('data-id', item.Id);
                        newEditButton.setAttribute('data-name', item.Name);

                        const actionCell = row.querySelector('td:nth-child(3)');
                        actionCell.innerHTML = '';
                        actionCell.appendChild(newEditButton);

                        showToast('danger', 'Düzenleme iptal edildi!');
                    });
                })
                .catch(error => {
                    console.error('Nesne getirilirken hata oluştu:', error);
                    showToast('danger', 'Bir hata oluştu!');
                });
        }
    });
}

function updateItem(item, entityName) {
    fetch(`/${entityName}/Update`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)  // Burada tüm item nesnesini JSON olarak gönderiyoruz
    }).then(response => {
        if (response.ok) {
            showToast('success', `${entityName} başarıyla güncellendi!`);
        } else {
            showToast('danger', `${entityName} güncelleme işlemi başarısız oldu!`);
        }
    }).catch(error => {
        console.error(`${entityName} güncelleme sırasında hata:`, error);
        showToast('danger', 'Bir hata oluştu!');
    });
}


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
