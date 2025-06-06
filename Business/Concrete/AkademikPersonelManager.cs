﻿using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Business.Concrete
{
    public class AkademikPersonelManager : IAkademikPersonelService
    {
        private readonly IAkademikPersonelDal _akademikPersonelDal;
        IDBAPDal _dBAPDal;
        ISinavDetayDal _sinavDetayDal;

        public AkademikPersonelManager(IAkademikPersonelDal akademikPersonelDal, IDBAPDal dBAPDal,ISinavDetayDal sinavDetayDal)
        {
            _akademikPersonelDal = akademikPersonelDal;
            _dBAPDal = dBAPDal;
            _sinavDetayDal = sinavDetayDal;
        }

        public IResult Add(AkademikPersonel akademikPersonel)
        {
            _akademikPersonelDal.AddAkademikPersonelWithUserOperationClaim(akademikPersonel);
            return new SuccessResult(Messages.AkademikPersonelAdded);
        }

        public IResult Delete(AkademikPersonel akademikPersonel)
        {
            var dbap = _dBAPDal.GetDetails(q => q.AkademikPersonelId == akademikPersonel.Id);
            if (dbap.Count > 0)
            {
                string message = $"{akademikPersonel.Ad} için {dbap.Count} tane Bölüm-Ders-Akademik Personel Eşleştirmesi bulunmaktadır.\n";
                foreach (var item in dbap)
                {
                    message += $"{item.BolumAd} {item.DersAd} {item.AkademikPersonelAd} {item.Unvan}\n";
                }
                return new ErrorResult(message);
            }
            _akademikPersonelDal.DeleteAkademikPersonelWithUserOperationClaim(akademikPersonel);
            return new SuccessResult(Messages.AkademikPersonelDeleted);
        }

        public IDataResult<AkademikPersonel> GetById(int akademikPeronelId)
        {
            return new SuccessDataResult<AkademikPersonel>(_akademikPersonelDal.Get(q => q.Id == akademikPeronelId));
        }

        public IDataResult<List<AkademikPersonel>> GetList(Expression<Func<AkademikPersonel, bool>> filter = null)
        {
            return new SuccessDataResult<List<AkademikPersonel>>(_akademikPersonelDal.GetAll(filter), $"{_akademikPersonelDal.GetAll(filter).Count} tane bulundu.");
        }

        public IResult Update(AkademikPersonel akademikPersonel)
        {
            _akademikPersonelDal.UpdateAkademikPersonelWithUserOperationClaim(akademikPersonel);
            return new SuccessResult(Messages.AkademikPersonelUpdated);
        }

        public async Task<IResult> AddPersonelWithUser(AkademikPersonel akademikPersonel)
        {
            try
            {
                _akademikPersonelDal.Add(akademikPersonel);
                return new SuccessResult("Akademik personel ve kullanıcı başarıyla eklendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Personel eklenirken bir hata oluştu: {ex.Message}");
            }
        }

        public IResult DeleteWithTransaction(AkademikPersonel akademikPersonel)
        {
            try
            {
                _akademikPersonelDal.DeleteAkademikPersonelWithUserOperationClaim(akademikPersonel);
                return new SuccessResult("Akademik personel ve ilişkili kullanıcı başarıyla silindi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
            }
        }

        public IResult UpdateWithTransaction(AkademikPersonel akademikPersonel)
        {
            try
            {
                _akademikPersonelDal.Update(akademikPersonel);
                return new SuccessResult("Akademik personel ve ilişkili kullanıcı başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Güncelleme işlemi sırasında bir hata oluştu: " + ex.Message);
            }
        }
        
    }
}
