using Entity.Abstract;

namespace Entity.Concrete
{
    public class SinavDetay : IEntity
    {
        public int Id { get; set; }
        public int DBAPId { get; set; }
        public int DerslikId { get; set; }
        public DateTime SinavTarihi { get; set; }
        public TimeOnly SinavSaati { get; set; }
    }
}
