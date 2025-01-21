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

const BOLUM_COLORS = {
    1: '#2196F3', // Bilgisayar - Mavi
    2: '#F44336', // İnşaat - Kırmızı
    3: '#4CAF50', // Elektronik - Yeşil
    4: '#9C27B0', // Alternatif - Mor
    5: '#FF9800'  // Diğer - Turuncu
};

// Global değişkenler
let calendar;

/**
 * Takvim yönetimi için ana sınıf
 */
class SinavTakvimiManager {
    constructor(initialData) {
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
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
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
        try {
            // Tarih ve saat bilgisini birleştir
            const eventDate = new Date(sinav.sinavTarihi);
            const [hours, minutes] = sinav.sinavSaati.split(':');
            eventDate.setHours(parseInt(hours), parseInt(minutes), 0);

            const event = {
                id: sinav.id,
                title: sinav.dersAd,
                start: eventDate,
                description: `${sinav.unvan} ${sinav.akademikPersonelAd}`,
                backgroundColor: this.getBolumColor(sinav.bolumId || 4), // Alternatif Enerji için 4
                borderColor: this.getBolumColor(sinav.bolumId || 4),
                textColor: '#fff',
                allDay: false,
                extendedProps: {
                    dersAd: sinav.dersAd,
                    akademikPersonelAd: sinav.akademikPersonelAd,
                    bolumAd: sinav.bolumAd,
                    unvan: sinav.unvan,
                    derslikAd: sinav.derslikAd || 'Derslik Atanmamış',
                    derslikKontenjan: sinav.derslikKontenjan || 0
                }
            };

            calendar.addEvent(event);
        } catch (error) {
            this.handleError(error);
        }
    }

    /**
     * Bölüm ID'sine göre renk döndürür
     * @param {number} bolumId - Bölüm ID'si
     * @returns {string} Renk kodu
     */
    getBolumColor(bolumId) {
        return BOLUM_COLORS[bolumId] || BOLUM_COLORS[1];
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
                    backgroundColor: this.getBolumColor(eventEl.dataset.bolumId),
                    borderColor: this.getBolumColor(eventEl.dataset.bolumId),
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
    handleEventDrop(info) {
        const event = info.event;
        const newDate = event.start;

        // API'ye güncelleme isteği gönder
        const updateData = {
            id: event.id,
            sinavTarihi: newDate.toISOString(),
            sinavSaati: `${newDate.getHours()}:${newDate.getMinutes()}`
        };

        fetch('/SinavDetay/Update', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(updateData)
        })
        .then(response => response.json())
        .then(result => {
            if (!result.success) {
                info.revert();
                this.handleError(new Error(result.message));
            }
        })
        .catch(error => {
            info.revert();
            this.handleError(error);
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
                <div class="derslik-gozetmen-list">
                    ${selectedDerslikler.map(derslik => `
                        <div class="derslik-gozetmen-item mb-3">
                            <p><strong>${derslik.ad}</strong> (Kapasite: ${derslik.kapasite} kişi)</p>
                            <div class="form-group">
                                <label>Gözetmen Seç:</label>
                                ${gozetmenSelectHTML}
                            </div>
                        </div>
                    `).join('')}
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
}

// Sayfa yüklendiğinde takvim yöneticisini başlat
$(document).ready(() => {
    new SinavTakvimiManager(INITIAL_DATA);
}); 