using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBolumService
    {
        List<Bolum> GetById(int bolumId);
        List<Bolum> GetList();
        void Add(Bolum bolum);
        void Delete(Bolum bolum);
        void Update(Bolum bolum);
    }
}
