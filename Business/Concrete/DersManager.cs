using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class DersManager : IDersService
    {
        private IDersDal _dersDal;
        IDBAPDal _dBAPDal;

        public DersManager(IDersDal dersDal, IDBAPDal dBAPDal)
        {
            _dersDal = dersDal;
            _dBAPDal = dBAPDal;
        }

        IResult IDersService.Add(Ders ders)
        {
            _dersDal.Add(ders);
            return new SuccessResult(Messages.DersAdded);
        }

        IResult IDersService.Delete(Ders ders)
        {
            var dbap = _dBAPDal.GetDetails(q => q.DersId == ders.Id);
            if (dbap.Count > 0)
            {
                string message = $"{ders.Ad} dersi için {dbap.Count} tane Bölüm-Ders-Akademik Personel Eşleştirmesi bulunmaktadır.\n";
                foreach (var item in dbap)
                {
                    message += $"{item.BolumAd} {item.DersAd} {item.AkademikPersonelAd} {item.Unvan}\n";
                }
                return new ErrorResult(message);
            }
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
