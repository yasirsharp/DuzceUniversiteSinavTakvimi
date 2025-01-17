using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BolumManager : IBolumService
    {
        IBolumDal _bolumDal;

        public BolumManager(IBolumDal bolumDal)
        {
            _bolumDal = bolumDal;
        }

        IResult IBolumService.Add(Bolum bolum)
        {
            _bolumDal.Add(bolum);
            return new SuccessResult(Messages.BolumAdded);
        }

        IResult IBolumService.Delete(Bolum bolum)
        {
            _bolumDal.Delete(bolum);
            return new SuccessResult(Messages.BolumDeleted);
        }

        IDataResult<Bolum> IBolumService.GetById(int bolumId)
        {
            return new SuccessDataResult<Bolum>(_bolumDal.Get(p => p.Id == bolumId));
        }

        IDataResult<List<Bolum>> IBolumService.GetList()
        {
            return new SuccessDataResult<List<Bolum>>(_bolumDal.GetAll(), $"{_bolumDal.GetAll()} tane bulundu.");
        }

        IResult IBolumService.Update(Bolum bolum)
        {
            _bolumDal.Update(bolum);
            return new SuccessResult(Messages.BolumUpdated);
        }
    }
}
