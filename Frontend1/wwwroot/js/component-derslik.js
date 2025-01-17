function initializeDeleteButtons(entityName) {
    const deleteButtons = document.querySelectorAll('.delete-btn');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function () {
            const itemId = this.getAttribute('data-id');
            const itemName = this.getAttribute('data-name');
            if (confirm(`Bu ${itemName} silmek istediğinizden emin misiniz?`)) {
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
        var kapasite = document.getElementById("newKapasite").value;

        if (ad && kapasite) {
            var entity = { ad: ad, kapasite: parseInt(kapasite) };

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
            showToast('danger', 'Lütfen gerekli alanları doldurun!');
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
            const itemCapacityCell = row.querySelector('td:nth-child(3)');

            const initialName = button.getAttribute('data-name');
            const initialCapacity = button.getAttribute('data-capacity');

            const inputName = document.createElement('input');
            inputName.type = 'text';
            inputName.classList.add('form-control');
            inputName.value = initialName;

            const inputCapacity = document.createElement('input');
            inputCapacity.type = 'number';
            inputCapacity.classList.add('form-control');
            inputCapacity.value = initialCapacity;

            itemNameCell.innerHTML = '';
            itemNameCell.appendChild(inputName);

            itemCapacityCell.innerHTML = '';
            itemCapacityCell.appendChild(inputCapacity);

            button.style.display = 'none';

            const saveButton = document.createElement('button');
            saveButton.classList.add('btn', 'btn-success', 'btn-sm');
            saveButton.textContent = 'Kaydet';

            const cancelButton = document.createElement('button');
            cancelButton.classList.add('btn', 'btn-secondary', 'btn-sm');
            cancelButton.textContent = 'İptal';

            const actionCell = row.querySelector('td:nth-child(4)');
            actionCell.innerHTML = '';
            actionCell.appendChild(saveButton);
            actionCell.appendChild(cancelButton);

            saveButton.addEventListener('click', function () {
                var updatedEntity = {
                    id: itemId,
                    ad: inputName.value,
                    kapasite: parseInt(inputCapacity.value)
                };

                fetch(`/${entityName}/Update`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(updatedEntity)
                }).then(response => {
                    if (response.ok) {
                        showToast('success', `${entityName} başarıyla güncellendi!`);
                        location.reload();
                    } else {
                        showToast('danger', `${entityName} güncelleme işlemi başarısız oldu!`);
                    }
                }).catch(error => {
                    console.error(`${entityName} güncelleme sırasında hata:`, error);
                    showToast('danger', 'Bir hata oluştu!');
                });
            });

            cancelButton.addEventListener('click', function () {
                itemNameCell.innerHTML = initialName;
                itemCapacityCell.innerHTML = initialCapacity;

                actionCell.innerHTML = '';
                actionCell.appendChild(button);

                showToast('danger', 'Düzenleme iptal edildi!');
            });
        }
    });
}

function showToast(type, message) {
    const toastContainer = document.getElementById('toast-container') || document.body.appendChild(document.createElement('div'));
    toastContainer.id = 'toast-container';

    const toast = document.createElement('div');
    toast.classList.add('toast', 'fade', 'show', `bg-${type}`, 'text-white');
    toast.setAttribute('role', 'alert');
    toast.innerHTML = `<div class="toast-body">${message}</div>`;

    toastContainer.appendChild(toast);

    setTimeout(() => {
        toast.classList.remove('show');
        toast.classList.add('hide');
        setTimeout(() => toast.remove(), 300);
    }, 5000);
}
