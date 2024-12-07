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
    public class DerslikManager : IDerslikService
    {
        private IDerslikDal _derslikDal;

        public DerslikManager(IDerslikDal derslikDal)
        {
            _derslikDal = derslikDal;
        }

        public void Add(Derslik derslik)
        {
            _derslikDal.Add(derslik);
        }

        public void Delete(Derslik derslik)
        {
            _derslikDal.Delete(derslik);
        }

        public Derslik GetById(int derslikId)
        {
            return _derslikDal.Get(q => q.Id == derslikId);
        }

        public List<Derslik> GetList()
        {
            return _derslikDal.GetAll();
        }

        public List<Derslik> GetList(Expression<Func<Derslik, bool>> filter)
        {
            return _derslikDal.GetAll(filter);
        }

        public void Update(Derslik derslik)
        {
            _derslikDal.Update(derslik);
        }
    }
}
