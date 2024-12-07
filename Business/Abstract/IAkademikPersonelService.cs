using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAkademikPersonelService
    {
        AkademikPersonel GetById(int akademikPeronelId);
        List<AkademikPersonel> GetList(Expression<Func<AkademikPersonel, bool>> filter);
        List<AkademikPersonel> GetList();
        void Add(AkademikPersonel akademikPersonel);
        void Delete(AkademikPersonel akademikPersonel);
        void Update(AkademikPersonel akademikPersonel);
    }
}
