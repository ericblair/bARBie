using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public partial class Repository
    {
        bARBieEntities _barbieEntity;

        public Repository(bARBieEntities barbieEntity)
        {
            _barbieEntity = barbieEntity;
        }
    }
}
