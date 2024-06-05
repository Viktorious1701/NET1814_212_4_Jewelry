using Jewelry.Data.Base;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry.Data.Repository
{
    public class CustomerRepository : GenericRepository<SiCustomer>
    {
        private readonly UnitOfWork _unitOfWork;
        public CustomerRepository()
        {
            _unitOfWork ??= new UnitOfWork();
        }
    }
}
