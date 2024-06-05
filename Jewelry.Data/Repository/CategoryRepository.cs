using Jewelry.Data.Base;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry.Data.Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository() { }

        public CategoryRepository(NET1814_212_4_JewelryContext context) => _context = context;

        /// TO-DO CODE HERE////////////////
    }
}
