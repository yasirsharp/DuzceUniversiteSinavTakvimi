using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDerslikService
    {
        Derslik GetById(int derslikId);
        List<Derslik> GetList(Expression<Func<Derslik, bool>> filter);
        List<Derslik> GetList();
        void Add(Derslik ders);
        void Delete(Derslik ders);
        void Update(Derslik ders);
    }
}
