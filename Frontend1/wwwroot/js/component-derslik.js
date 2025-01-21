document.addEventListener('DOMContentLoaded', function () {
    const entityName = 'Derslik';

    initializeDeleteButtons(entityName);
    initializeAddButton(entityName);
    initializeEditButtons(entityName);
});

// Silme işlemi
function initializeDeleteButtons(entityName) {
    document.querySelectorAll('.delete-btn').forEach(button => {
        button.addEventListener('click', function() {
            const id = this.getAttribute('data-id');
            const name = this.getAttribute('data-name');

            Swal.fire({
                title: 'Emin misiniz?',
                text: `"${name}" dersliğini silmek istediğinize emin misiniz?`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Evet, Sil!',
                cancelButtonText: 'İptal'
            }).then((result) => {
                if (result.isConfirmed) {
                    deleteEntity(id, entityName);
                }
            });
        });
    });
}

// API işlemleri
function deleteEntity(id, entityName) {
    fetch(`/${entityName}/Delete/${id}`, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: result.message,
                toast: true,
                position: 'top-end',
                timer: 2000,
                showConfirmButton: false
            });
            document.querySelector(`tr[data-id="${id}"]`).remove();
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: result.message,
                toast: true,
                position: 'top-end',
                timer: 3000,
                showConfirmButton: false
            });
        }
    })
    .catch(error => {
        Swal.fire({
            icon: 'error',
            title: 'Hata!',
            text: 'Bir hata oluştu: ' + error,
            toast: true,
            position: 'top-end',
            timer: 3000,
            showConfirmButton: false
        });
    });
}

// Ekleme işlemi
function initializeAddButton(entityName) {
    const inputRow = document.getElementById("inputRow");
    const tableBody = document.getElementById("tableBody");
    
    // Ekle butonuna tıklandığında
    document.getElementById("addRowBtn").addEventListener("click", function () {
        // Input satırını göster ve en üste taşı
        inputRow.style.display = "table-row-group";
        // Input row'u tablonun en üstüne taşı
        tableBody.parentNode.insertBefore(inputRow, tableBody);
        // Input alanlarını temizle
        document.getElementById('newAd').value = '';
        document.getElementById('newKapasite').value = '';
    });

    // İptal butonuna tıklandığında
    document.getElementById("cancelNewRowBtn").addEventListener("click", function() {
        // Input satırını gizle
        inputRow.style.display = "none";
        // Input alanlarını temizle
        document.getElementById('newAd').value = '';
        document.getElementById('newKapasite').value = '';
        
        // İptal edildiğini bildir
        Swal.fire({
            icon: 'info',
            title: 'İptal Edildi',
            text: 'Ekleme işlemi iptal edildi.',
            toast: true,
            position: 'top-end',
            timer: 2000,
            showConfirmButton: false
        });
    });

    // Kaydet butonuna tıklandığında
    document.getElementById("submitBtn").addEventListener("click", function() {
        const newAd = document.getElementById('newAd').value;
        const newKapasite = document.getElementById('newKapasite').value;
        
        if (!newAd || !newKapasite) {
            Swal.fire({
                icon: 'warning',
                title: 'Uyarı!',
                text: 'Lütfen tüm alanları doldurunuz.',
                toast: true,
                position: 'top-end',
                timer: 3000,
                showConfirmButton: false
            });
            return;
        }

        if (newKapasite <= 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Uyarı!',
                text: 'Kapasite 0\'dan büyük olmalıdır.',
                toast: true,
                position: 'top-end',
                timer: 3000,
                showConfirmButton: false
            });
            return;
        }

        addEntity(newAd, newKapasite, entityName);
    });
}

// Düzenleme işlemi
function initializeEditButtons(entityName) {
    document.querySelectorAll('.edit-btn').forEach(button => {
        button.addEventListener('click', function() {
            const id = this.getAttribute('data-id');
            const currentName = this.getAttribute('data-name');
            const currentCapacity = this.getAttribute('data-capacity');

            Swal.fire({
                title: 'Derslik Düzenle',
                html: `
                    <div style="display: flex; align-items: center; margin-bottom: 10px;">
                        <label for="swal-input1" style="width: 80px;">Derslik Adı:</label>
                        <input id="swal-input1" class="swal2-input" placeholder="Derslik adı" value="${currentName}" style="margin: 0;">
                    </div>
                    <div style="display: flex; align-items: center;">
                        <label for="swal-input2" style="width: 80px;">Kapasite:</label>
                        <input id="swal-input2" class="swal2-input" type="number" placeholder="Kapasite" value="${currentCapacity}" style="margin: 0;">
                    </div>
                `,
                showCancelButton: true,
                confirmButtonText: 'Güncelle',
                cancelButtonText: 'İptal',
                preConfirm: () => {
                    const ad = document.getElementById('swal-input1').value;
                    const kapasite = document.getElementById('swal-input2').value;
                    if (!ad || !kapasite) {
                        Swal.showValidationMessage('Lütfen tüm alanları doldurunuz');
                        return false;
                    }
                    if (kapasite <= 0) {
                        Swal.showValidationMessage('Kapasite 0\'dan büyük olmalıdır');
                        return false;
                    }
                    return { ad, kapasite }
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    updateEntity(id, result.value.ad, result.value.kapasite, entityName);
                }
            });
        });
    });
}

function addEntity(name, capacity, entityName) {
    fetch(`/${entityName}/Add`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Ad: name, Kapasite: parseInt(capacity) })
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: result.message,
                toast: true,
                position: 'top-end',
                timer: 2000,
                showConfirmButton: false
            });
            location.reload();
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: result.message,
                toast: true,
                position: 'top-end',
                timer: 3000,
                showConfirmButton: false
            });
        }
    })
    .catch(error => {
        Swal.fire({
            icon: 'error',
            title: 'Hata!',
            text: 'Bir hata oluştu: ' + error,
            toast: true,
            position: 'top-end',
            timer: 3000,
            showConfirmButton: false
        });
    });
}

function updateEntity(id, newName, newCapacity, entityName) {
    fetch(`/${entityName}/Update`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Id: id, Ad: newName, Kapasite: parseInt(newCapacity) })
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            Swal.fire({
                icon: 'success',
                title: 'Başarılı!',
                text: result.message,
                toast: true,
                position: 'top-end',
                timer: 2000,
                showConfirmButton: false
            });
            location.reload();
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: result.message,
                toast: true,
                position: 'top-end',
                timer: 3000,
                showConfirmButton: false
            });
        }
    })
    .catch(error => {
        Swal.fire({
            icon: 'error',
            title: 'Hata!',
            text: 'Bir hata oluştu: ' + error,
            toast: true,
            position: 'top-end',
            timer: 3000,
            showConfirmButton: false
        });
    });
}
