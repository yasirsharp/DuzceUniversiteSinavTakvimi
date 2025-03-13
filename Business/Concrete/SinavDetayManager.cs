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

        public IResult Add(SinavKayitDTO sinavKayitDTO)
        {
            try
            {
                // Derslik ve gözetmenleri listeye çevir
                List<int> derslikIdleri = sinavKayitDTO.Derslikler.Select(d => d.DerslikId).ToList();
                List<int> gozetmenIdleri = sinavKayitDTO.Derslikler.Where(d => d.GozetmenId.HasValue).Select(d => d.GozetmenId.Value).ToList();

                // Çakışma kontrolü
                var result = _sinavDetayDal.ExistSinav(derslikIdleri, gozetmenIdleri, sinavKayitDTO.DerBolumAkademikPersonelId,
                                                                      sinavKayitDTO.SinavBaslangicSaati, sinavKayitDTO.SinavBitisSaati, sinavKayitDTO.SinavTarihi);

                if (result!=null)
                    return new ErrorResult("Derslik, gözetmen veya akademik personel için çakışan bir sınav bulunmaktadır. Lütfen kontrol ediniz.");

                // Eğer çakışma yoksa, sınavı ekle
                _sinavDetayDal.AddWithTransaction(sinavKayitDTO);
                return new SuccessResult(Messages.SinavDetayAdded);
            }
            catch (Exception err)
            {
                return new ErrorResult(err.Message);
            }

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

        public IDataResult<List<SinavDetayDTO>> GetByBolumId(int bolumId)
        {
            return new SuccessDataResult<List<SinavDetayDTO>>(_sinavDetayDal.GetByBolumId(bolumId));
        }

        public IDataResult<SinavDetayDTO> GetById(int sinavDetayId)
        {
            return new SuccessDataResult<SinavDetayDTO>(_sinavDetayDal.GetDetail(sinavDetayId));
        }

        public IResult Update(SinavDetay sinavDetay)
        {
            try
            {
                // Önce kaydın var olduğundan emin ol
                var existingRecord = _sinavDetayDal.Get(s => s.Id == sinavDetay.Id);
                if (existingRecord == null)
                {
                    return new ErrorResult("Güncellenecek sınav kaydı bulunamadı.");
                }

                // Mevcut kaydın değerlerini güncelle
                existingRecord.DerBolumAkademikPersonelId = sinavDetay.DerBolumAkademikPersonelId;
                existingRecord.SinavTarihi = sinavDetay.SinavTarihi;
                existingRecord.SinavBaslangicSaati = sinavDetay.SinavBaslangicSaati;
                existingRecord.SinavBitisSaati= sinavDetay.SinavBitisSaati;
                

                // Güncellemeyi yap
                _sinavDetayDal.Update(existingRecord);

                return new SuccessResult(Messages.SinavDetayUpdated);
            }
            catch (Exception err)
            {
                return new ErrorResult(err.Message);
            }
        }
    }
}
