using Core.DataAccess;
using Entity.Concrete;
using Entity.DTOs;

namespace DataAccess.Abstract
{
    public interface IDBAPDal:IEntityRepository<DersBolumAkademikPersonel>
    {
        List<DersBolumAkademikPersonelDTO> GetDetails();
    }
}
