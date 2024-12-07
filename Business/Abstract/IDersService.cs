using Entity.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDersService
    {
        Ders GetById(int dersId);
        List<Ders> GetList(Expression<Func<Ders, bool>> filter);
        List<Ders> GetList();
        void Add(Ders ders);
        void Delete(Ders ders);
        void Update(Ders ders);
    }
}
