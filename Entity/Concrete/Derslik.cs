using Entity.Abstract;

namespace Entity.Concrete
{
    public class Derslik : IEntity
    {
        public int Id { get; set; }
        public string DerslikAdi { get; set; }
        public int Kapasite { get; set; }
    }
}
