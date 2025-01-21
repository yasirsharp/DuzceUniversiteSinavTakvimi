using Entity.Abstract;

namespace Entity.DTOs
{
    public class SinavDetayDTO : IDto
    {
        public int Id { get; set; }
        public string DersAd { get; set; }
        public string BolumAd { get; set; }
        public string AkademikPersonelAd { get; set; }
        public string Unvan { get; set; }
        public int DerslikId { get; set; }
        public int DerslikKontenjan { get; set; }
        public DateTime SinavTarihi { get; set; }
        public TimeOnly SinavSaati { get; set; }
    }
}
