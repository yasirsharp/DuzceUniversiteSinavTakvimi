using Core.Utilities.Results;
using Entity.Concrete;
using Entity.DTOs;
using System.Linq.Expressions;

namespace Business.Abstract
{
    public interface IDBAPService
    {
        IDataResult<List<DersBolumAkademikPersonel>> GetAll();
        IDataResult<List<DersBolumAkademikPersonelDTO>> GetAllDetails();
        IDataResult<DersBolumAkademikPersonel> GetById(int dbapId);
        IResult Add(DersBolumAkademikPersonel dersBolumAkademikPersonel);
        IResult Delete(DersBolumAkademikPersonel dersBolumAkademikPersonel);
        IResult Update(DersBolumAkademikPersonel dersBolumAkademikPersonel);
    }
}
