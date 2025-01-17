using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DBAPManager : IDBAPService
    {
        IDBAPDal _dbapDal;

        public DBAPManager(IDBAPDal dbapDal)
        {
            _dbapDal = dbapDal;
        }

        public IResult Add(DersBolumAkademikPersonel dersBolumAkademikPersonel)
        {
            _dbapDal.Add(dersBolumAkademikPersonel);
            return new SuccessResult(Messages.DBAPAdded);
        }

        public IResult Delete(DersBolumAkademikPersonel dersBolumAkademikPersonel)
        {
            _dbapDal.Delete(dersBolumAkademikPersonel);
            return new SuccessResult(Messages.DBAPDeleted);
        }

        public IDataResult<DersBolumAkademikPersonel> GetById(int dbapId)
        {
            var result = _dbapDal.Get(q=>q.Id==dbapId);
            if (result != null)
                return new ErrorDataResult<DersBolumAkademikPersonel>(Messages.SomethingWrong);

            return new SuccessDataResult<DersBolumAkademikPersonel>(result);

        }

        public IDataResult<List<DersBolumAkademikPersonel>> GetAll()
        {
            var result = _dbapDal.GetAll().ToList();
            if (result != null) return new SuccessDataResult<List<DersBolumAkademikPersonel>>(result, $"{result.Count} tane bulundu.");

            return new ErrorDataResult<List<DersBolumAkademikPersonel>>(result, Messages.SomethingWrong);
        }

        public IResult Update(DersBolumAkademikPersonel dersBolumAkademikPersonel)
        {
            _dbapDal.Update(dersBolumAkademikPersonel);
            return new SuccessResult(Messages.DBAPUpdated);
        }

        public IDataResult<List<DersBolumAkademikPersonelDTO>> GetAllDetails()
        {
            var result = _dbapDal.GetDetails().ToList();
            return new SuccessDataResult<List<DersBolumAkademikPersonelDTO>>(result, $"{result.Count} tane bulundu.");
        }
    }
}
