function initializeDeleteButtons(entityName) {
    const deleteButtons = document.querySelectorAll('.delete-btn');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function () {
            const itemId = this.getAttribute('data-id');
            if (confirm(`Bu kaydı silmek istediğinizden emin misiniz?`)) {
                fetch(`/${entityName}/Delete`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: itemId })
                }).then(response => {
                    if (response.ok) {
                        showToast('success', `${entityName} başarıyla silindi!`);
                        location.reload();
                    } else {
                        showToast('danger', `${entityName} silme işlemi başarısız oldu!`);
                    }
                }).catch(error => {
                    console.error(`${entityName} silme sırasında hata:`, error);
                    showToast('danger', 'Bir hata oluştu!');
                });
            }
        });
    });
}

function initializeAddButton(entityName) {
    document.getElementById("addRowBtn").addEventListener("click", function () {
        document.getElementById("inputRow").style.display = "table-row-group";
    });

    document.getElementById("submitBtn").addEventListener("click", function () {
        var ad = document.getElementById("newAd").value;
        var unvan = document.getElementById("newUnvan").value;

        if (ad && unvan) {
            var entity = {
                ad: ad,
                unvan: unvan
            };

            fetch(`/${entityName}/Add`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(entity)
            }).then(response => {
                if (response.ok) {
                    showToast('success', `${entityName} başarıyla eklendi!`);
                    location.reload();
                } else {
                    showToast('danger', `${entityName} eklenirken bir hata oluştu!`);
                }
            }).catch(error => {
                console.error("Hata:", error);
                showToast('danger', `Bir hata oluştu: ${error.message}`);
            });
        } else {
            showToast('danger', 'Lütfen tüm alanları doldurun!');
        }
    });
}

function initializeEditButtons(entityName) {
    document.querySelector('#dataTable').addEventListener('click', function (event) {
        if (event.target.classList.contains('edit-btn')) {
            const button = event.target;
            const itemId = button.getAttribute('data-id');
            const row = button.closest('tr');
            const itemNameCell = row.querySelector('td:nth-child(2)');
            const itemTitleCell = row.querySelector('td:nth-child(3)');

            const initialName = button.getAttribute('data-name');
            const initialTitle = button.getAttribute('data-title');

            const inputName = document.createElement('input');
            inputName.type = 'text';
            inputName.classList.add('form-control');
            inputName.value = initialName;

            const inputTitle = document.createElement('input');
            inputTitle.type = 'text';
            inputTitle.classList.add('form-control');
            inputTitle.value = initialTitle;

            itemNameCell.innerHTML = '';
            itemNameCell.appendChild(inputName);

            itemTitleCell.innerHTML = '';
            itemTitleCell.appendChild(inputTitle);

            button.style.display = 'none';

            const saveButton = document.createElement('button');
            saveButton.classList.add('btn', 'btn-success', 'btn-sm');
            saveButton.textContent = 'Kaydet';

            const cancelButton = document.createElement('button');
            cancelButton.classList.add('btn', 'btn-danger', 'btn-sm');
            cancelButton.textContent = 'İptal';

            const actionCell = row.querySelector('td:nth-child(4)');
            actionCell.innerHTML = '';
            actionCell.appendChild(saveButton);
            actionCell.appendChild(cancelButton);

            saveButton.addEventListener('click', function () {
                var _entity = {
                    id: itemId,
                    ad: inputName.value,
                    unvan: inputTitle.value
                };

                updateItem(_entity, entityName);

                itemNameCell.innerHTML = inputName.value;
                itemTitleCell.innerHTML = inputTitle.value;

                const newEditButton = document.createElement('button');
                newEditButton.classList.add('btn', 'btn-warning', 'btn-sm', 'edit-btn');
                newEditButton.textContent = 'Düzenle';
                newEditButton.setAttribute('data-id', itemId);
                newEditButton.setAttribute('data-name', inputName.value);
                newEditButton.setAttribute('data-title', inputTitle.value);

                const newDeleteButton = document.createElement('button');
                newDeleteButton.classList.add('btn', 'btn-danger', 'btn-sm', 'delete-btn');
                newDeleteButton.textContent = 'Sil';
                newDeleteButton.setAttribute('data-id', itemId);

                actionCell.innerHTML = '';
                actionCell.appendChild(newEditButton);
                actionCell.appendChild(newDeleteButton);

                showToast('success', `${entityName} başarıyla güncellendi!`);
            });

            cancelButton.addEventListener('click', function () {
                itemNameCell.innerHTML = initialName;
                itemTitleCell.innerHTML = initialTitle;

                const newEditButton = document.createElement('button');
                newEditButton.classList.add('btn', 'btn-warning', 'btn-sm', 'edit-btn');
                newEditButton.textContent = 'Düzenle';
                newEditButton.setAttribute('data-id', itemId);
                newEditButton.setAttribute('data-name', initialName);
                newEditButton.setAttribute('data-title', initialTitle);

                actionCell.innerHTML = '';
                actionCell.appendChild(newEditButton);

                showToast('danger', 'Düzenleme iptal edildi!');
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
        body: JSON.stringify(item)
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
