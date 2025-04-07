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
        this.dersliklerData = initialData.derslikler;
        this.dbapDetailData = initialData.dbapDetail;
        this.sinavlarData = initialData.sinavlar;
        this.akademikPersonellerData = initialData.akademikPersoneller.data;
        this.bolumId = initialData.bolumId;
        this.initializeComponents();
    }

    initializeComponents() {
        // Select2 inicializasyonu

        console.log(this.sinavlarData);

        $('#mainDerslikFilter').select2({
            placeholder: "Derslik Seçiniz",
            allowClear: true,
            width: '100%',
            multiple: true
        });

        // Takvimi oluştur
        this.initializeCalendar();
    }

    initializeCalendar() {
        calendar = new bryntum.calendar.Calendar({
            appendTo: 'calendar',
            height: '100%',
            flex: 1,
            date: new Date(),
            mode: 'week',
            weekStartDay: 1,
            startDate: 7,  // 07:00
            endDate: 21,   // 21:00
            hideNonWorkingDays: true,
            nonWorkingDays: {
                0: true,  // Pazar
                6: true   // Cumartesi
            },
            features: {
                eventTooltip: false,
                eventEdit: false,
                eventMenu: false,
                scheduleMenu: false,
                drag: {
                    validateDrop: true,
                    showTooltip: true
                },
                externalEventSource: {
                    dragRootElement: 'sinav-source',
                    dragItemSelector: '.external-event'  
                }
            },
            eventRenderer({ eventRecord, renderData }) {
                return {
                    children: [{
                        className: 'b-event-name',
                        text: eventRecord.name
                    }]
                };
            },
            listeners: {
                eventClick: ({ eventRecord }) => {
                    // Sınav detaylarını bul
                    const sinav = this.sinavlarData.find(s => s.id === eventRecord.id);
                    if (!sinav) return;

                    // Derslik bilgilerini hazırla
                    const derslikBilgileri = this.sinavlarData
                        .filter(s => s.id === sinav.id)
                        .map(s => {
                            const derslik = this.dersliklerData.find(d => d.id === s.derslikId);
                            const gozetmen = s.gozetmenId ? 
                                this.akademikPersonellerData.find(ap => ap.id === s.gozetmenId) : null;
                            
                            return `
                                <div class="mb-2">
                                    <strong>Derslik:</strong> ${derslik ? derslik.ad : 'Bilinmiyor'} 
                                    (Kapasite: ${s.derslikKontenjan})
                                    ${gozetmen ? 
                                        `<br><strong>Gözetmen:</strong> ${gozetmen.unvan} ${gozetmen.ad}` : 
                                        '<br><strong>Gözetmen:</strong> Atanmamış'}
                                </div>
                            `;
                        }).join('');

                    // Pop-up içeriğini oluştur
                    const popupContent = `
                        <div class="text-start">
                            <h5 class="mb-3">Sınav Bilgileri</h5>
                            <div class="mb-2">
                                <strong>Ders:</strong> ${sinav.dersAd}
                            </div>
                            <div class="mb-2">
                                <strong>Bölüm:</strong> ${sinav.bolumAd}
                            </div>
                            <div class="mb-2">
                                <strong>Öğretim Görevlisi:</strong> ${sinav.unvan} ${sinav.akademikPersonelAd}
                            </div>
                            <hr>
                            <div class="mb-2">
                                <strong>Tarih:</strong> ${new Date(sinav.sinavTarihi).toLocaleDateString('tr-TR')}
                            </div>
                            <div class="mb-2">
                                <strong>Başlangıç:</strong> ${sinav.sinavBaslangicSaati}
                            </div>
                            <div class="mb-2">
                                <strong>Bitiş:</strong> ${sinav.sinavBitisSaati}
                            </div>
                            <hr>
                            <h5 class="mb-3">Derslik ve Gözetmen Bilgileri</h5>
                            ${derslikBilgileri}
                        </div>
                    `;

                    // Pop-up'ı göster
                    Swal.fire({
                        title: 'Sınav Detayları',
                        html: popupContent,
                        icon: 'info',
                        confirmButtonText: 'Kapat'
                    });
                },
                beforeDragCreate: ({ context }) => {
                    const eventEl = context.draggedEvent || context.element;
                    if (!eventEl) return false;

                    // Seçili derslikleri kontrol et
                    const selectedDerslikIds = $('#mainDerslikFilter').val();
                    if (!selectedDerslikIds || selectedDerslikIds.length === 0) {
                        Swal.fire({
                            title: 'Uyarı!',
                            text: 'Lütfen en az bir derslik seçiniz.',
                            icon: 'warning'
                        });
                        return false;
                    }

                    // Sınav verilerini hazırla
                    const eventData = {
                        id: eventEl.dataset.id,
                        dersId: eventEl.dataset.dersId,
                        bolumId: eventEl.dataset.bolumId,
                        dersAd: eventEl.querySelector('h5').textContent,
                        akademikPersonelAd: eventEl.querySelector('p').textContent,
                        startDate: context.startDate,
                        endDate: new Date(context.startDate.getTime() + 60 * 60 * 1000), // 1 saat sonrası
                        derslikler: selectedDerslikIds
                    };

                    // Pop-up'ı göster
                    this.showExamSummaryPopup(eventData);
                    return false;
                }
            }
        });

        // Sürükle-bırak işlevselliğini başlat
        this.initializeDragAndDrop();
        this.loadSinavlar();
    }

    initializeDragAndDrop() {
        const draggables = document.querySelectorAll('.external-event');
        
        draggables.forEach(draggable => {
            draggable.addEventListener('dragstart', (e) => {
                e.dataTransfer.setData('text/plain', draggable.dataset.id);
                draggable.classList.add('dragging');
            });

            draggable.addEventListener('dragend', () => {
                draggable.classList.remove('dragging');
            });
        });

        // Takvim üzerine bırakma işlemi
        const calendarElement = document.getElementById('calendar');
        calendarElement.addEventListener('dragover', (e) => {
            e.preventDefault();
            calendarElement.classList.add('drag-over');
        });

        calendarElement.addEventListener('dragleave', () => {
            calendarElement.classList.remove('drag-over');
        });

        calendarElement.addEventListener('drop', (e) => {
            e.preventDefault();
            calendarElement.classList.remove('drag-over');
            
            const eventId = e.dataTransfer.getData('text/plain');
            const eventElement = document.querySelector(`[data-id="${eventId}"]`);
            
            if (eventElement) {
                const rect = calendarElement.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;
                
                // Takvim üzerindeki konumu hesapla
                const date = this.calculateDateFromPosition(x, y);
                if (date) {
                    const eventData = {
                        derBolumAkademikPersonelId: eventElement.dataset.id,
                        dersId: eventElement.dataset.dersId,
                        bolumId: eventElement.dataset.bolumId,
                        dersAd: eventElement.querySelector('h5').textContent,
                        akademikPersonelAd: eventElement.querySelector('p').textContent,
                        startDate: date,
                        endDate: new Date(date.getTime() + 60 * 60 * 1000),
                        derslikler: $('#mainDerslikFilter').val()
                    };

                    this.showExamSummaryPopup(eventData);
                }
            }
        });
    }

    calculateDateFromPosition(x, y) {
        const calendarElement = document.getElementById('calendar');
        const rect = calendarElement.getBoundingClientRect();
        
        // Takvim görünümünü al
        const view = calendar.activeView;
        if (!view) return null;

        // Görünümün başlangıç ve bitiş tarihlerini al
        const startDate = view.startDate;
        const endDate = view.endDate;

        // Görünümün yüksekliğini ve genişliğini al
        const viewHeight = rect.height;
        const viewWidth = rect.width;

        // Gün sayısını hesapla
        const days = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24));
        
        // Saat başına düşen piksel sayısını hesapla
        const hoursPerDay = 12; // 8:00 - 20:00 arası
        const pixelsPerHour = viewHeight / hoursPerDay;
        
        // Gün başına düşen piksel sayısını hesapla
        const pixelsPerDay = viewWidth / days;

        // Bırakılan konumun gün ve saatini hesapla
        const dayIndex = Math.floor(x / pixelsPerDay);
        const hourIndex = Math.floor(y / pixelsPerHour);

        // Tarihi oluştur
        const date = new Date(startDate);
        date.setDate(date.getDate() + dayIndex);
        date.setHours(8 + hourIndex); // 8:00'den başlayarak
        date.setMinutes(0);
        date.setSeconds(0);
        date.setMilliseconds(0);

        return date;
    }

    showExamSummaryPopup(eventData) {

        console.log(eventData);

        // Seçili derslikleri al
        const selectedDerslikler = eventData.derslikler.map(id => 
            this.dersliklerData.find(d => d.id === parseInt(id))
        ).filter(d => d !== undefined);

        // Gözetmen seçimi için HTML
        const gozetmenSelectHTML = `
                <select class="form-control gozetmen-select">
                    <option value="">Gözetmen Seçiniz</option>
                    ${this.akademikPersonellerData.map(ap => 
                    `<option value="${ap.id}">${ap.unvan} ${ap.ad}</option>`
                    ).join('')}
                </select>
            `;

        // Pop-up içeriği
        const popupContent = `
                <div class="text-start">
                <h5 class="mb-3">Ders Bilgileri:</h5>
                    <div class="mb-2">
                    <strong>Ders:</strong> ${eventData.dersAd}
                    </div>
                    <div class="mb-2">
                    <strong>Öğretim Görevlisi:</strong> ${eventData.akademikPersonelAd}
                    </div>
                    <hr>
                <h5 class="mb-3">Sınav Zamanı:</h5>
                    <div class="mb-2">
                    <strong>Tarih:</strong> ${eventData.startDate.toLocaleDateString('tr-TR')}
                    </div>
                    <div class="mb-2">
                    <strong>Başlangıç:</strong> ${eventData.startDate.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' })}
                    </div>
                    <div class="mb-2">
                    <strong>Bitiş:</strong> ${eventData.endDate.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' })}
                    </div>
                    <hr>
                <h5 class="mb-3">Derslik ve Gözetmen Atamaları:</h5>
                    <div id="yeniDerslikContainer">
                    ${selectedDerslikler.map((derslik, index) => `
                            <div class="mb-3 derslik-gozetmen-item" data-derslik-id="${derslik.id}">
                                <div class="mb-2">
                                    <strong>Derslik ${index + 1}:</strong> ${derslik.ad} (Kapasite: ${derslik.kapasite})
                                </div>
                                <div>
                                    <label>Gözetmen Seç:</label>
                                ${gozetmenSelectHTML}
                            </div>
                            </div>
                        `).join('')}
                    </div>
                </div>
            `;

        // Pop-up'ı göster
        Swal.fire({
            title: 'Sınav Oluşturma Onayı',
            html: popupContent,
            icon: 'question',
                showCancelButton: true,
            confirmButtonText: 'Oluştur',
                cancelButtonText: 'İptal',
                width: '800px',
                didOpen: () => {
                    $('.gozetmen-select').select2({
                        dropdownParent: $('.swal2-container'),
                        width: '100%',
                        placeholder: "Gözetmen Seçiniz"
                    });
            }
        }).then((result) => {
            if (result.isConfirmed) {
                this.createNewExam(eventData);
            }
        });
    }

    async createNewExam(eventData) {
        try {
                    // Seçilen gözetmenleri topla
            const derslikler = [];
                    document.querySelectorAll('.derslik-gozetmen-item').forEach(item => {
                derslikler.push({
                            derslikId: parseInt(item.dataset.derslikId),
                            gozetmenId: parseInt(item.querySelector('.gozetmen-select').value) || null
                        });
            });

            // Sınav verilerini hazırla
            const sinavData = {
                derBolumAkademikPersonelId: parseInt(eventData.derBolumAkademikPersonelId),
                sinavTarihi: eventData.startDate.toISOString().split('T')[0],
                sinavBaslangicSaati: eventData.startDate.toTimeString().slice(0, 8),
                sinavBitisSaati: eventData.endDate.toTimeString().slice(0, 8),
                derslikler: derslikler
            };

            // Sınav oluştur
            const response = await fetch('/SinavDetay/Add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(sinavData)
            });

            const responseData = await response.json();
            if (!responseData.success) {
                throw new Error(responseData.message);
            }

            // Başarılı mesajı göster
            await Swal.fire({
                title: 'Başarılı!',
                text: 'Sınav başarıyla oluşturuldu.',
                icon: 'success'
            });

            // Takvimi yenile
            this.loadSinavlar();

        } catch (error) {
            Swal.fire({
                title: 'Hata!',
                text: error.message,
                icon: 'error'
            });
        }
    }

    loadSinavlar() {
        if (!this.sinavlarData) {
            console.warn('Sınav verisi bulunamadı');
            return;
        }

        // Sınavları grupla (aynı ID'ye sahip sınavları birleştir)
        const gruplanmisSinavlar = this.sinavlarData.reduce((acc, sinav) => {
            if (!acc[sinav.id]) {
                acc[sinav.id] = {
                    id: sinav.id,
                    name: sinav.dersAd,
                    title: sinav.dersAd,
                    startDate: new Date(`${sinav.sinavTarihi.split('T')[0]}T${sinav.sinavBaslangicSaati}`),
                    endDate: new Date(`${sinav.sinavTarihi.split('T')[0]}T${sinav.sinavBitisSaati}`),
                    description: `
                        <div class="sinav-detay">
                            <p><strong>Ders:</strong> ${sinav.dersAd}</p>
                            <p><strong>Bölüm:</strong> ${sinav.bolumAd}</p>
                            <p><strong>Öğretim Görevlisi:</strong> ${sinav.unvan} ${sinav.akademikPersonelAd}</p>
                            <p><strong>Derslikler:</strong></p>
                            <ul>
                                ${this.sinavlarData
                                    .filter(s => s.id === sinav.id)
                                    .map(s => `<li>${this.dersliklerData.find(d => d.id === s.derslikId)?.ad} (Kapasite: ${s.derslikKontenjan})</li>`)
                                    .join('')}
                            </ul>
                        </div>
                    `,
                    eventColor: this.getBolumColor(sinav.bolumAd),
                    derslikler: this.sinavlarData
                        .filter(s => s.id === sinav.id)
                        .map(s => ({
                            derslikId: s.derslikId,
                            gozetmenId: s.gozetmenId,
                            kapasite: s.derslikKontenjan
                        }))
                };
            }
            return acc;
        }, {});

        // Takvim olaylarını güncelle
        calendar.eventStore.data = Object.values(gruplanmisSinavlar);
    }

    getBolumColor(bolumAd) {
        // Bölüm adına göre renk üret
        const colors = {
            'Bilgisayar Programcılığı': '#3498db',
            'Bilgisayar Mühendisliği': '#2ecc71',
            'Elektrik-Elektronik Mühendisliği': '#e74c3c',
            'Makine Mühendisliği': '#f1c40f',
            'İnşaat Mühendisliği': '#9b59b6',
            'Endüstri Mühendisliği': '#1abc9c'
        };

        return colors[bolumAd] || '#95a5a6';
    }
}

// Sayfa yüklendiğinde takvim yöneticisini başlat
$(document).ready(() => {
    new SinavTakvimiManager(INITIAL_DATA);
}); 