using Core.DataAccess;
using Entity.Concrete;
using Entity.DTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface ISinavDetayDal:IEntityRepository<SinavDetay>
    {
        List<SinavDetayDTO> GetByBolumId(int bolumId);
        List<SinavDetayDTO> GetDetails();
        SinavDetayDTO GetDetail(int sinavDetayId);
    }
}
