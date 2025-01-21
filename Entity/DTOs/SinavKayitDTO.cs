namespace Entity.DTOs
{
    public class SinavKayitDTO
    {
        public int DbapId { get; set; }
        public DateTime SinavTarihi { get; set; }
        public TimeOnly SinavSaati { get; set; }
        public List<DerslikGozetmenDTO> Derslikler { get; set; }
    }
}
