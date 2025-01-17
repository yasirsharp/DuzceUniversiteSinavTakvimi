using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using Entity.Concrete;
using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class AkademikPersonelDal : EfEntityRepositoryBase<AkademikPersonel, DuzceUniversiteContext>, IAkademikPersonelDal
    {
    }

}
