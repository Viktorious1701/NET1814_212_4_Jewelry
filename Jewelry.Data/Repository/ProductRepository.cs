using Jewelry.Data.Base;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry.Data.Repository
{
    public class ProductRepository : GenericRepository<SiProduct>
    {
        public ProductRepository() { }

        public ProductRepository(NET1814_212_4_JewelryContext context) => _context = context;
    }
}
