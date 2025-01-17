using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SinavDetayManager : ISinavDetayService
    {
        ISinavDetayDal _sinavDetayDal;

        public SinavDetayManager(ISinavDetayDal sinavDetayDal)
        {
            _sinavDetayDal = sinavDetayDal;
        }

        public IResult Add(SinavDetay sinavDetay)
        {
            _sinavDetayDal.Add(sinavDetay);
            return new SuccessResult(Messages.SinavDetayAdded);

        }

        public IResult Delete(SinavDetay sinavDetay)
        {
            _sinavDetayDal.Delete(sinavDetay);
            return new SuccessResult(Messages.SinavDetayDeleted);
        }

        public IDataResult<List<SinavDetay>> GetAll()
        {
            var result = _sinavDetayDal.GetAll();
            return new SuccessDataResult<List<SinavDetay>>(result, $"{result.Count} tane sonuç bulundu.");
        }

        public IDataResult<List<SinavDetayDTO>> GetAllDetails()
        {
            return new SuccessDataResult<List<SinavDetayDTO>>(_sinavDetayDal.GetDetails());
        }

        public IDataResult<SinavDetayDTO> GetById(int sinavDetayId)
        {
            return new SuccessDataResult<SinavDetayDTO>(_sinavDetayDal.GetDetail(sinavDetayId));
        }

        public IResult Update(SinavDetay sinavDetay)
        {
            _sinavDetayDal.Update(sinavDetay);
            return new SuccessResult(Messages.SinavDetayUpdated);
        }
    }
}
