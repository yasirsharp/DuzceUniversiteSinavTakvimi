using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DersManager : IDersService
    {
        private IDersDal _dersDal;

        public DersManager(IDersDal dersDal)
        {
            _dersDal = dersDal;
        }

        public void Add(Ders ders)
        {
            _dersDal.Add(ders);
        }

        public void Delete(Ders ders)
        {
            _dersDal.Delete(ders);
        }

        public Ders GetById(int dersId)
        {
            return _dersDal.Get(q => q.Id == dersId);
        }

        public List<Ders> GetList()
        {
            return _dersDal.GetAll();
        }

        public List<Ders> GetList(Expression<Func<Ders, bool>> filter)
        {
            return _dersDal.GetAll(filter);
        }

        public void Update(Ders ders)
        {
            _dersDal.Update(ders);
        }
    }
}
