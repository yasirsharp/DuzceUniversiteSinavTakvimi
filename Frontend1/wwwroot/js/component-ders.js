document.addEventListener('DOMContentLoaded', function () {
    // Sayfanın türüne göre entityName'i belirliyoruz
    const entityName = 'Ders'; // Bu sayfa için 'Ders' nesnesi

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
                text: `"${name}" dersini silmek istediğinize emin misiniz?`,
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
        // Input alanını temizle
        document.getElementById('newAd').value = '';
    });

    // İptal butonuna tıklandığında
    document.getElementById("cancelNewRowBtn").addEventListener("click", function() {
        // Input satırını gizle
        inputRow.style.display = "none";
        // Input alanını temizle
        document.getElementById('newAd').value = '';
        
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
        if (!newAd) {
            Swal.fire({
                icon: 'warning',
                title: 'Uyarı!',
                text: 'Lütfen ders adını giriniz.',
                toast: true,
                position: 'top-end',
                timer: 3000,
                showConfirmButton: false
            });
            return;
        }
        addEntity(newAd, entityName);
    });
}

// Düzenleme işlemi
function initializeEditButtons(entityName) {
    document.querySelectorAll('.edit-btn').forEach(button => {
        button.addEventListener('click', function() {
            const id = this.getAttribute('data-id');
            const currentName = this.getAttribute('data-name');

            Swal.fire({
                title: 'Ders Düzenle',
                input: 'text',
                inputValue: currentName,
                showCancelButton: true,
                confirmButtonText: 'Güncelle',
                cancelButtonText: 'İptal',
                inputValidator: (value) => {
                    if (!value) {
                        return 'Lütfen bir değer giriniz!';
                    }
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    updateEntity(id, result.value, entityName);
                }
            });
        });
    });
}

function addEntity(name, entityName) {
    fetch(`/${entityName}/Add`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Ad: name })
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

function updateEntity(id, newName, entityName) {
    fetch(`/${entityName}/Update`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Id: id, Ad: newName })
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
