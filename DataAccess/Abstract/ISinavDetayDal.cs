using Core.DataAccess;
using Entity.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface ISinavDetayDal:IEntityRepository<SinavDetay>
    {
        List<SinavDetayDTO> GetDetails();
        SinavDetayDTO GetDetail(int sinavDetayId);
    }
}
