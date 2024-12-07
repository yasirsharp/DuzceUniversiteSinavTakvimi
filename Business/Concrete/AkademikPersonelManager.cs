using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AkademikPersonelManager : IAkademikPersonelService
    {
        private IAkademikPersonelDal _akademikPersonel;

        public AkademikPersonelManager(IAkademikPersonelDal akademikPersonel)
        {
            _akademikPersonel = akademikPersonel;
        }

        public void Add(AkademikPersonel akademikPersonel)
        {
            _akademikPersonel.Add(akademikPersonel);
        }

        public void Delete(AkademikPersonel akademikPersonel)
        {
            _akademikPersonel.Delete(akademikPersonel);
        }

        public AkademikPersonel GetById(int akademikPeronelId)
        {
            return _akademikPersonel.Get(q => q.Id == akademikPeronelId);
        }

        public List<AkademikPersonel> GetList(Expression<Func<AkademikPersonel, bool>> filter)
        {
            return _akademikPersonel.GetAll(filter);
        }

        public List<AkademikPersonel> GetList()
        {
            return _akademikPersonel.GetAll();
        }

        public void Update(AkademikPersonel akademikPersonel)
        {
            _akademikPersonel.Update(akademikPersonel);
        }
    }
}
