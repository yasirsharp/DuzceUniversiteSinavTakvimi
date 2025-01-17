using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class DersManager : IDersService
    {
        private IDersDal _dersDal;

        public DersManager(IDersDal dersDal)
        {
            _dersDal = dersDal;
        }

        IResult IDersService.Add(Ders ders)
        {
            _dersDal.Add(ders);
            return new SuccessResult(Messages.DersAdded);
        }

        IResult IDersService.Delete(Ders ders)
        {
            _dersDal.Delete(ders);
            return new SuccessResult(Messages.DersDeleted);
        }

        IDataResult<Ders> IDersService.GetById(int dersId)
        {
            return new SuccessDataResult<Ders>(_dersDal.Get(q => q.Id == dersId));
        }

        IDataResult<List<Ders>> IDersService.GetList(Expression<Func<Ders, bool>> filter)
        {
            return new SuccessDataResult<List<Ders>>(_dersDal.GetAll(filter), $"{_dersDal.GetAll().Count} tane bulundu.");
        }

        IResult IDersService.Update(Ders ders)
        {
            _dersDal.Update(ders);
            return new SuccessResult(Messages.DersUpdated);
        }
    }
}
