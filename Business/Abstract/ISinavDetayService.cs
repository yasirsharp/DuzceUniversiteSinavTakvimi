using Core.Utilities.Results;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Abstract
{
    public interface ISinavDetayService
    {
        IDataResult<List<SinavDetay>> GetAll();
        IDataResult<List<SinavDetayDTO>> GetByBolumId(int bolumId);
        IDataResult<List<SinavDetayDTO>> GetAllDetails();
        IDataResult<SinavDetayDTO> GetById(int sinavDetayId);
        IResult Add(SinavKayitDTO sinavKayitDTO);
        IResult Delete(SinavDetay sinavDetay);
        IResult Update(SinavDetay sinavDetay);
    }
}
