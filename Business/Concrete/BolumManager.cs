using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BolumManager : IBolumService
    {
        IBolumDal _bolumDal;

        public BolumManager(IBolumDal bolumDal)
        {
            _bolumDal = bolumDal;
        }

        public void Add(Bolum bolum)
        {
            _bolumDal.Add(bolum);

        }

        public void Delete(Bolum bolum)
        {
            _bolumDal.Delete(bolum);
        }

        public List<Bolum> GetById(int bolumId)
        {
            return _bolumDal.GetAll(p=>p.Id==bolumId);
        }

        public List<Bolum> GetList()
        {
            return _bolumDal.GetAll();
        }

        public void Update(Bolum bolum)
        {
            _bolumDal.Update(bolum);
        }
    }
}
