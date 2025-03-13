using Core.Entities;

namespace Entity.DTOs
{
    public class SinavKayitDTO:IDto
    {
        public int DerBolumAkademikPersonelId { get; set; }
        public DateTime SinavTarihi { get; set; }
        public TimeOnly SinavBaslangicSaati { get; set; }
        public TimeOnly SinavBitisSaati { get; set; }
        public List<DerslikGozetmenDTO> Derslikler { get; set; }
    }
}
