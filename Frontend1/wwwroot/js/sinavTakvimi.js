/**
 * Sınav Takvimi Yönetim Modülü
 * Bu modül, sınav programı takviminin tüm işlevselliğini yönetir.
 */

// Sabit Değerler
const CALENDAR_CONFIG = {
    SLOT_MIN_TIME: '08:00:00',
    SLOT_MAX_TIME: '18:00:00',
    SLOT_DURATION: '01:00:00'
};

// Renk üretimi için yardımcı fonksiyonlar
const COLOR_CONFIG = {
    SINAV_COLOR: '#607D8B',  // Takvimde görünen sınavlar için sabit renk
    // HSL renk değerleri için sabitler
    SATURATION: 60, // Renk doygunluğu (%)
    LIGHTNESS: 45,  // Renk parlaklığı (%)
    GOLDEN_RATIO: 0.618033988749895 // Altın oran
};

/**
 * HSL renk uzayında renk üretir
 * @param {number} index - Bölüm index'i
 * @returns {string} HSL renk kodu
 */
function generateColor(index) {
    // Altın oran ile renk üret (daha estetik dağılım için)
    const hue = (index * COLOR_CONFIG.GOLDEN_RATIO * 360) % 360;
    return `hsl(${hue}, ${COLOR_CONFIG.SATURATION}%, ${COLOR_CONFIG.LIGHTNESS}%)`;
}

/**
 * Bölüm ID'si için renk döndürür
 * @param {number} bolumId - Bölüm ID'si
 * @returns {string} Renk kodu
 */
function getBolumColor(bolumId) {
    if (bolumId === 'sinav') return COLOR_CONFIG.SINAV_COLOR;
    return generateColor(bolumId);
}

// Global değişkenler
let calendar;

/**
 * Takvim yönetimi için ana sınıf
 */
class SinavTakvimiManager {
    constructor(initialData) {
        this.isProcessing = false;
        this.dersliklerData = initialData.derslikler;
        this.dbapDetailData = initialData.dbapDetail;
        this.sinavlarData = initialData.sinavlar;
        this.akademikPersonellerData = initialData.akademikPersoneller.data;
        this.initializeComponents();
        this.setupEventListeners();
    }

    /**
     * Bileşenlerin başlangıç ayarlarını yapar
     */
    initializeComponents() {
        // Select2 inicializasyonu
        $('#mainDerslikFilter').select2({
            placeholder: "Derslik Seçiniz",
            allowClear: true,
            width: '100%',
            multiple: true
        });

        // Takvimi oluştur
        this.initializeCalendar();
    }

    /**
     * Takvimi yapılandırır ve oluşturur
     */
    initializeCalendar() {
        calendar = new FullCalendar.Calendar(document.getElementById('calendar'), {
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'exportButton dayGridMonth,timeGridWeek,timeGridDay'
            },
            customButtons: {
                exportButton: {
                    text: 'Haftalık Sınav Programı',
                    click: () => this.exportToExcel()
                }
            },
            initialView: 'timeGridWeek',
            locale: 'tr',
            height: '100%',
            editable: true,
            droppable: true,
            slotMinTime: CALENDAR_CONFIG.SLOT_MIN_TIME,
            slotMaxTime: CALENDAR_CONFIG.SLOT_MAX_TIME,
            allDaySlot: false,
            slotDuration: CALENDAR_CONFIG.SLOT_DURATION,
            snapDuration: CALENDAR_CONFIG.SLOT_DURATION,
            expandRows: true,
            slotEventOverlap: false,
            nowIndicator: true,

            // Sürükle-Bırak ayarları
            eventDrop: (info) => {
                this.handleEventDrop(info);
            },
            drop: (info) => {
                this.handleExternalDrop(info);
            },
            eventReceive: (info) => {
                this.handleEventReceive(info);
            },
            // Sınav detay görüntüleme için tıklama olayı
            eventClick: (info) => {
                this.handleEventClick(info);
            }
        });

        // Dış etkinlikleri sürüklenebilir yap
        this.initializeExternalEvents();

        calendar.render();
        this.loadSinavlar();
    }

    /**
     * Event listener'ları ayarlar
     */
    setupEventListeners() {
        // Derslik seçimi değiştiğinde takvim güncelleme özelliği kaldırıldı
    }

    /**
     * Sınavları yükler ve takvime ekler
     */
    loadSinavlar() {
        // Önce tüm eventları temizle
        calendar.removeAllEvents();

        try {
            // Veriyi işle
            if (Array.isArray(this.sinavlarData)) {
                this.sinavlarData.forEach(sinav => {
                    if (sinav) {
                        this.createCalendarEvent(sinav);
                    }
                });
            }

            // Takvimi güncelle
            calendar.gotoDate(new Date()); // Bugüne git
            calendar.render();
        } catch (error) {
            this.handleError(error);
        }
    }

    /**
     * Tek bir sınav için takvim eventi oluşturur
     * @param {Object} sinav - Sınav verisi
     */
    createCalendarEvent(sinav) {
        console.log(sinav);
        try {
            // Tarih ve saat bilgisini birleştir
            const eventDate = new Date(sinav.sinavTarihi);
            const [hours, minutes] = sinav.sinavSaati.split(':');
            eventDate.setHours(parseInt(hours), parseInt(minutes), 0);

            // Derslik bilgisini bul
            const derslik = this.dersliklerData.find(d => d.id === sinav.derslikId);

            // Gözetmen bilgisini bul
            const gozetmenId = sinav.gozetmenId || null;

            const event = {
                id: sinav.id,
                title: sinav.dersAd,
                start: eventDate,
                description: `${sinav.unvan} ${sinav.akademikPersonelAd}`,
                backgroundColor: COLOR_CONFIG.SINAV_COLOR,
                borderColor: COLOR_CONFIG.SINAV_COLOR,
                textColor: '#fff',
                allDay: false,
                extendedProps: {
                    dersAd: sinav.dersAd,
                    akademikPersonelAd: sinav.akademikPersonelAd,
                    bolumAd: sinav.bolumAd,
                    unvan: sinav.unvan,
                    derslikId: sinav.derslikId,
                    derslikAd: derslik ? derslik.ad : 'Derslik Atanmamış',
                    derslikKontenjan: sinav.derslikKontenjan,
                    gozetmenId: gozetmenId,
                    dbapId: sinav.dbapId
                }
            };

            calendar.addEvent(event);
        } catch (error) {
            this.handleError(error);
        }
    }

    /**
     * Hata yönetimi
     * @param {Error} error - Hata nesnesi
     */
    handleError(error) {
        Swal.fire({
            title: 'Hata!',
            text: error.message,
            icon: 'error'
        });
    }

    /**
     * Dış etkinlikleri sürüklenebilir yapar
     */
    initializeExternalEvents() {
        const draggableEvents = document.querySelectorAll('.external-event');
        draggableEvents.forEach(eventEl => {
            new FullCalendar.Draggable(eventEl, {
                eventData: (eventEl) => ({
                    title: eventEl.querySelector('h5').innerText,
                    description: eventEl.querySelector('p').innerText,
                    backgroundColor: getBolumColor(parseInt(eventEl.dataset.bolumId)),
                    borderColor: getBolumColor(parseInt(eventEl.dataset.bolumId)),
                    textColor: '#fff',
                    extendedProps: {
                        dbapId: parseInt(eventEl.dataset.id),
                        dersId: parseInt(eventEl.dataset.dersId),
                        bolumId: parseInt(eventEl.dataset.bolumId)
                    }
                })
            });
        });
    }

    /**
     * Takvim içindeki event sürüklendiğinde
     */
    async handleEventDrop(info) {
        if (this.isProcessing) return;
        this.isProcessing = true;

        try {
            const event = info.event;
            const newDate = event.start;

            const updateData = {
                id: event.id,
                sinavTarihi: newDate.toISOString(),
                sinavSaati: `${newDate.getHours()}:${newDate.getMinutes()}`
            };

            const response = await fetch('/SinavDetay/Update', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(updateData)
            });

            const result = await response.json();
            if (!result.success) {
                info.revert();
                this.handleError(new Error(result.message));
            }
        } catch (error) {
            info.revert();
            this.handleError(error);
        } finally {
            this.isProcessing = false;
        }
    }

    /**
     * Takvimde bir sınava tıklandığında
     */
    handleEventClick(info) {
        const event = info.event;
        const extendedProps = event.extendedProps;
        const eventDate = event.start;
        const formattedDate = eventDate.toLocaleDateString('tr-TR');
        const formattedTime = eventDate.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });

        // Derslik bilgisini bul
        const derslik = this.dersliklerData.find(d => d.id === extendedProps.derslikId);

        // Gözetmen bilgisini bul
        const gozetmen = this.akademikPersonellerData.find(ap => ap.id === extendedProps.gozetmenId);
        const gozetmenBilgisi = gozetmen ? `${gozetmen.ad} (${gozetmen.unvan})` : '-';

        const derslikBilgisi = derslik 
            ? `${derslik.ad} (Kapasite: ${derslik.kapasite} kişi)`
            : 'Derslik Atanmamış';

        const detayHTML = `
            <div style="text-align: left; margin-top: 10px;">
                <h4>Ders Bilgileri:</h4>
                <p><strong>Ders:</strong> ${extendedProps.dersAd}</p>
                <p><strong>Öğretim Görevlisi:</strong> ${extendedProps.unvan} ${extendedProps.akademikPersonelAd}</p>
                <p><strong>Bölüm:</strong> ${extendedProps.bolumAd}</p>
                <hr>
                <h4>Sınav Zamanı:</h4>
                <p><strong>Tarih:</strong> ${formattedDate}</p>
                <p><strong>Saat:</strong> ${formattedTime}</p>
                <hr>
                <h4>Derslik ve Gözetmen Bilgisi:</h4>
                <p><strong>Derslik:</strong> ${derslikBilgisi}</p>
                <p><strong>Gözetmen:</strong> ${gozetmenBilgisi}</p>
            </div>
        `;

        Swal.fire({
            title: 'Sınav Detayları',
            html: detayHTML,
            icon: 'info',
            showCancelButton: true,
            showDenyButton: true,
            confirmButtonText: 'Düzenle',
            denyButtonText: 'Sil',
            cancelButtonText: 'Kapat',
            confirmButtonColor: '#3085d6',
            denyButtonColor: '#d33',
            cancelButtonColor: '#6c757d'
        }).then((result) => {
            if (result.isConfirmed) {
                this.showEditDialog(event);
            } else if (result.isDenied) {
                this.showDeleteConfirmation(event);
            }
        });
    }

    /**
     * Sınav silme onay dialogu
     */
    showDeleteConfirmation(event) {
        Swal.fire({
            title: 'Emin misiniz?',
            text: "Bu sınavı silmek istediğinize emin misiniz?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Evet, Sil',
            cancelButtonText: 'İptal'
        }).then((result) => {
            if (result.isConfirmed) {
                this.deleteSinav(event.id);
            }
        });
    }

    /**
     * Sınavı sil
     */
    deleteSinav(sinavId) {
        fetch('/SinavDetay/Delete', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(sinavId)
        })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire('Başarılı!', result.message, 'success');
                this.loadSinavlar();
            } else {
                Swal.fire('Hata!', result.message, 'error');
            }
        })
        .catch(error => {
            Swal.fire('Hata!', 'Silme işlemi sırasında bir hata oluştu', 'error');
        });
    }

    /**
     * Düzenleme dialogunu göster
     */
    showEditDialog(event) {
        const props = event.extendedProps;
        const eventDate = event.start;

        // Mevcut derslik-gözetmen eşleştirmelerini al
        fetch(`/SinavDetay/GetSinavDerslikler/${event.id}`)
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    const mevcutDerslikler = result.data;

                    // Gözetmen seçimi için select listesi HTML'i oluştur
                    const gozetmenSelectHTML = (selectedGozetmenId = null) => `
                        <select class="form-control gozetmen-select">
                            <option value="">Gözetmen Seçiniz</option>
                            ${this.akademikPersonellerData.map(ap => 
                                `<option value="${ap.id}" ${ap.id === selectedGozetmenId ? 'selected' : ''}>${ap.unvan} ${ap.ad}</option>`
                            ).join('')}
                        </select>
                    `;

                    // Derslik seçimi için mevcut derslikleri al
                    const derslikSelectHTML = `
                        <select class="form-control derslik-select" multiple>
                            ${this.dersliklerData.map(d => 
                                `<option value="${d.id}" ${mevcutDerslikler.some(md => md.derslikId === d.id) ? 'selected' : ''}>
                                    ${d.ad} (Kapasite: ${d.kapasite})
                                </option>`
                            ).join('')}
                        </select>
                    `;

                    const editHTML = `
                        <div style="text-align: left; margin-top: 10px;">
                            <div class="form-group mb-3">
                                <label>Saat:</label>
                                <input type="time" class="form-control" id="sinavSaati" value="${eventDate.toTimeString().slice(0,5)}">
                            </div>
                            <div class="form-group mb-3">
                                <label>Derslikler:</label>
                                ${derslikSelectHTML}
                            </div>
                            <div id="gozetmenContainer">
                                ${mevcutDerslikler.map(md => {
                                    const derslik = this.dersliklerData.find(d => d.id === md.derslikId);
                                    return `
                                        <div class="form-group mb-3 derslik-gozetmen-item" data-derslik-id="${md.derslikId}">
                                            <label>${derslik.ad} için Gözetmen:</label>
                                            ${gozetmenSelectHTML(md.gozetmenId)}
                                        </div>
                                    `;
                                }).join('')}
                            </div>
                        </div>
                    `;

                    Swal.fire({
                        title: 'Sınavı Düzenle',
                        html: editHTML,
                        width: '600px',
                        showCancelButton: true,
                        confirmButtonText: 'Kaydet',
                        cancelButtonText: 'İptal',
                        didOpen: () => {
                            // Select2'yi aktifleştir
                            $('.derslik-select').select2({
                                dropdownParent: $('.swal2-container'),
                                width: '100%',
                                placeholder: "Derslik Seçiniz"
                            });

                            // Mevcut gözetmen seçimlerini aktifleştir
                            $('.gozetmen-select').select2({
                                dropdownParent: $('.swal2-container'),
                                width: '100%',
                                placeholder: "Gözetmen Seçiniz"
                            });

                            // Derslik seçimi değiştiğinde
                            $('.derslik-select').on('change', (e) => {
                                const selectedDerslikler = $(e.target).val();
                                const container = document.getElementById('gozetmenContainer');
                                container.innerHTML = '';

                                selectedDerslikler.forEach(derslikId => {
                                    const derslik = this.dersliklerData.find(d => d.id === parseInt(derslikId));
                                    // Mevcut gözetmen seçimini koru
                                    const mevcutDerslik = mevcutDerslikler.find(md => md.derslikId === parseInt(derslikId));
                                    container.innerHTML += `
                                        <div class="form-group mb-3 derslik-gozetmen-item" data-derslik-id="${derslikId}">
                                            <label>${derslik.ad} için Gözetmen:</label>
                                            ${gozetmenSelectHTML(mevcutDerslik?.gozetmenId)}
                                        </div>
                                    `;
                                });

                                // Yeni eklenen select2'leri aktifleştir
                                $('.gozetmen-select').select2({
                                    dropdownParent: $('.swal2-container'),
                                    width: '100%',
                                    placeholder: "Gözetmen Seçiniz"
                                });
                            });
                        },
                        preConfirm: () => {
                            const saat = document.getElementById('sinavSaati').value;
                            const derslikler = [];
                            
                            document.querySelectorAll('.derslik-gozetmen-item').forEach(item => {
                                derslikler.push({
                                    derslikId: parseInt(item.dataset.derslikId),
                                    gozetmenId: parseInt(item.querySelector('.gozetmen-select').value) || null
                                });
                            });

                            // Event'in extendedProps'undan dbapId'yi al
                            const dbapId = event.extendedProps.dbapId;
                            console.log('Event props:', event.extendedProps);
                            console.log('DBAP ID:', dbapId);

                            const updateData = {
                                id: event.id,
                                dbapId: dbapId,
                                sinavTarihi: eventDate.toISOString().split('T')[0],
                                sinavSaati: saat,
                                derslikler: derslikler
                            };

                            console.log('Update data:', updateData);
                            return updateData;
                        }
                    }).then((result) => {
                        if (result.isConfirmed) {
                            this.updateSinav(result.value);
                        }
                    });
                }
            });
    }

    /**
     * Sınavı güncelle
     */
    updateSinav(data) {
        fetch('/SinavDetay/Update', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire('Başarılı!', result.message, 'success');
                this.loadSinavlar();
            } else {
                Swal.fire('Hata!', result.message, 'error');
            }
        })
        .catch(error => {
            Swal.fire('Hata!', 'Güncelleme sırasında bir hata oluştu', 'error');
        });
    }

    /**
     * Dışarıdan sürüklenen event takvime bırakıldığında
     */
    handleExternalDrop(info) {
        const droppedEl = info.draggedEl;
        const dropDate = info.date;

        // Seçili derslikleri al
        const selectedDerslikIds = $('#mainDerslikFilter').val();

        if (!selectedDerslikIds || selectedDerslikIds.length === 0) {
            Swal.fire({
                title: 'Hata!',
                text: 'Lütfen en az bir derslik seçin',
                icon: 'error'
            });
            return false;
        }

        // Seçili dersliklerin bilgilerini al
        const selectedDerslikler = selectedDerslikIds.map(id => 
            this.dersliklerData.find(d => d.id === parseInt(id))
        );

        // Sürüklenen dersin bilgilerini al
        const dbapBilgisi = this.dbapDetailData.find(d => d.id === parseInt(droppedEl.dataset.id));

        // Tarih ve saat bilgisini formatla
        const eventDate = dropDate;
        const formattedDate = eventDate.toLocaleDateString('tr-TR');
        const formattedTime = eventDate.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });

        // Gözetmen seçimi için select listesi HTML'i oluştur
        const gozetmenSelectHTML = `
            <select class="form-control gozetmen-select">
                <option value="">Gözetmen Seçiniz</option>
                ${this.akademikPersonellerData.map(ap => 
                    `<option value="${ap.id}">${ap.unvan} ${ap.ad}</option>`
                ).join('')}
            </select>
        `;

        // Tüm bilgileri HTML formatında hazırla
        const detayHTML = `
            <div style="text-align: left; margin-top: 10px;">
                <h4>Ders Bilgileri:</h4>
                <p><strong>Ders:</strong> ${dbapBilgisi.dersAd}</p>
                <p><strong>Öğretim Görevlisi:</strong> ${dbapBilgisi.unvan} ${dbapBilgisi.akademikPersonelAd}</p>
                <p><strong>Bölüm:</strong> ${dbapBilgisi.bolumAd}</p>
                <hr>
                <h4>Sınav Zamanı:</h4>
                <p><strong>Tarih:</strong> ${formattedDate}</p>
                <p><strong>Saat:</strong> ${formattedTime}</p>
                <hr>
                <h4>Derslik ve Gözetmen Ataması:</h4>
                <div class="table-responsive">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>Derslik</th>
                                <th>Gözetmen</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${selectedDerslikler.map(derslik => `
                                <tr class="derslik-gozetmen-item">
                                    <td><strong>${derslik.ad}</strong><br>(Kapasite: ${derslik.kapasite} kişi)</td>
                                    <td>${gozetmenSelectHTML}</td>
                                </tr>
                            `).join('')}
                        </tbody>
                    </table>
                </div>
            </div>
        `;

        // SweetAlert ile göster
        Swal.fire({
            title: 'Sınav Detayları',
            html: detayHTML,
            icon: 'info',
            showCancelButton: true,
            confirmButtonText: 'Onayla',
            cancelButtonText: 'İptal',
            width: '600px',
            willClose: () => {
                // İptal edilirse veya kapatılırsa son eklenen eventi kaldır
                const lastEvent = calendar.getEvents().slice(-1)[0];
                if (lastEvent) {
                    lastEvent.remove();
                }
            },
            preConfirm: () => {
                // Onaylanırsa son eklenen eventi kaldırma
                return true;
            },
            didOpen: () => {
                // Select2'yi gözetmen seçimleri için aktifleştir
                $('.gozetmen-select').select2({
                    dropdownParent: $('.swal2-container'),
                    width: '100%',
                    placeholder: "Gözetmen Seçiniz"
                });
            }
        }).then((result) => {
            if (result.isConfirmed) {
                // Seçili gözetmenleri al
                const derslikGozetmenler = [];
                $('.derslik-gozetmen-item').each(function(index) {
                    derslikGozetmenler.push({
                        derslikId: parseInt(selectedDerslikIds[index]),
                        gozetmenId: parseInt($(this).find('.gozetmen-select').val()) || null
                    });
                });

                // Yeni sınav verisi oluştur
                const sinavData = {
                    dbapId: parseInt(droppedEl.dataset.id),
                    sinavTarihi: eventDate.toISOString().split('T')[0],
                    sinavSaati: `${eventDate.getHours().toString().padStart(2, '0')}:${eventDate.getMinutes().toString().padStart(2, '0')}:00`,
                    derslikler: derslikGozetmenler
                };

                // API'ye kayıt isteği gönder
                fetch('/SinavDetay/Add', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify(sinavData)
                })
                .then(response => response.json())
                .then(result => {
                    if (!result.success) {
                        Swal.fire({
                            title: 'Hata!',
                            text: result.message || 'Sınav eklenirken bir hata oluştu',
                            icon: 'error'
                        });
                        return false;
                    } else {
                        Swal.fire({
                            title: 'Başarılı!',
                            text: 'Sınav başarıyla eklendi',
                            icon: 'success'
                        });
                        // Takvimi yenile
                        this.loadSinavlar();
                    }
                })
                .catch(error => {
                    Swal.fire({
                        title: 'Hata!',
                        text: 'Sınav eklenirken bir hata oluştu',
                        icon: 'error'
                    });
                });
            }
        });

        return false; // Event'ın varsayılan eklenmesini engelle
    }

    /**
     * Yeni event takvime eklendiğinde
     */
    handleEventReceive(info) {
        const event = info.event;
        console.log('Yeni event alındı:', event);
    }

    /**
     * Takvim verilerini Excel'e aktarır
     */
    exportToExcel() {
        // Tüm etkinlikleri al
        const events = calendar.getEvents();
        
        // Excel için veri hazırla
        const excelData = events.map(event => {
            const props = event.extendedProps;
            const derslik = this.dersliklerData.find(d => d.id === props.derslikId);
            const gozetmen = this.akademikPersonellerData.find(ap => ap.id === props.gozetmenId);
            
            return {
                'Ders': props.dersAd,
                'Öğretim Görevlisi': `${props.unvan} ${props.akademikPersonelAd}`,
                'Bölüm': props.bolumAd,
                'Tarih': event.start.toLocaleDateString('tr-TR'),
                'Saat': event.start.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' }),
                'Derslik': derslik ? derslik.ad : 'Atanmamış',
                'Derslik Kapasitesi': derslik ? derslik.kapasite : '-',
                'Gözetmen': gozetmen ? `${gozetmen.unvan} ${gozetmen.ad}` : '-'
            };
        });

        // Excel dosyası oluştur
        const ws = XLSX.utils.json_to_sheet(excelData);
        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Sınav Programı");

        // Sütun genişliklerini ayarla
        const maxWidth = Object.keys(excelData[0]).reduce((acc, key) => Math.max(acc, key.length), 0);
        ws['!cols'] = Object.keys(excelData[0]).map(() => ({ wch: maxWidth }));

        // Haftanın başlangıç ve bitiş tarihlerini al
        const currentDate = calendar.getDate();
        const monday = new Date(currentDate);
        monday.setDate(monday.getDate() - monday.getDay() + 1); // Pazartesi
        const friday = new Date(currentDate);
        friday.setDate(friday.getDate() - friday.getDay() + 5); // Cuma

        // İki basamaklı tarih formatı için yardımcı fonksiyon
        const pad = (num) => num.toString().padStart(2, '0');

        // Dosya adını oluştur
        const fileName = `Sinav_Programi_${pad(monday.getDate())}-${pad(monday.getMonth() + 1)}_${pad(friday.getDate())}-${pad(friday.getMonth() + 1)}.xlsx`;

        XLSX.writeFile(wb, fileName);
    }
}

// Sayfa yüklendiğinde takvim yöneticisini başlat
$(document).ready(() => {
    new SinavTakvimiManager(INITIAL_DATA);
}); 