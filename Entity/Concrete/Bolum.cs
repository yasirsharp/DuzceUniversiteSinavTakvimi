using Entity.Abstract;

namespace Entity.Concrete
{
    public class Bolum : IEntity
    {
        public int Id { get; set; }
        public string Ad { get; set; }
    }
}
