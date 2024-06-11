using Jewelry.Data.Base;
using Jewelry.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Jewelry.Data.Repository
{
    public class OrderRepository : GenericRepository<SiOrder>
    {
        private readonly Net1814_212_4_JewelryContext _context;

        public OrderRepository(Net1814_212_4_JewelryContext context) : base(context)
        {
            _context = context;
        }

        public void Detach(SiOrder order)
        {
            var entry = _context.Entry(order);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}