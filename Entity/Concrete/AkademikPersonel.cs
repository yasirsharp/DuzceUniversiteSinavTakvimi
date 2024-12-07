using Entity.Abstract;

namespace Entity.Concrete
{
    public class AkademikPersonel : IEntity
    {
        public int Id { get; set; }
        public string AkademikPersonelAdi { get; set; }
        public string Unvan { get; set; }
    }
}
