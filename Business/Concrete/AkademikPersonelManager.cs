using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class AkademikPersonelManager : IAkademikPersonelService
    {
        private IAkademikPersonelDal _akademikPersonel;

        public AkademikPersonelManager(IAkademikPersonelDal akademikPersonel)
        {
            _akademikPersonel = akademikPersonel;
        }

        public IResult Add(AkademikPersonel akademikPersonel)
        {
            _akademikPersonel.Add(akademikPersonel);
            return new SuccessResult(Messages.AkademikPersonelAdded);
        }

        public IResult Delete(AkademikPersonel akademikPersonel)
        {
            _akademikPersonel.Delete(akademikPersonel);
            return new SuccessResult(Messages.AkademikPersonelDeleted);
        }

        public IDataResult<AkademikPersonel> GetById(int akademikPeronelId)
        {
            return new SuccessDataResult<AkademikPersonel>(_akademikPersonel.Get(q => q.Id == akademikPeronelId));
        }

        public IDataResult<List<AkademikPersonel>> GetList(Expression<Func<AkademikPersonel, bool>> filter = null)
        {
            return new SuccessDataResult<List<AkademikPersonel>>(_akademikPersonel.GetAll(filter), $"{_akademikPersonel.GetAll(filter).Count} tane bulundu.");
        }

        public IResult Update(AkademikPersonel akademikPersonel)
        {
            _akademikPersonel.Update(akademikPersonel);
            return new SuccessResult(Messages.AkademikPersonelUpdated);
        }
    }
}
