using SE183584ConsoleEFApp.Base;
using SE183584ConsoleEFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE183584ConsoleEFApp.DAO
{
    public class CustomerDAO : BaseDAO<SiCustomer>
    {
        private readonly Net1814_212_4_JewelryContext _context;

        public CustomerDAO()
        {
        }
    }
}
           // _context = new Net1814_212_4_JewelryContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Net1814_212_4_JewelryContext>());
        
/*
        public SiCustomer[] GetAllCustomers()
        {
            return _context.SiCustomers.Select(c => new SiCustomer
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Phone = c.Phone,
                Address = c.Address,
            }).ToArray();
        }

        public SiCustomer GetCustomerById(int CustomerId)
        {
            return _context.SiCustomers.FirstOrDefault(c => c.CustomerId == CustomerId);
        }
        //console.add
        public void AddCustomer(SiCustomer customer)
        {
            _context.SiCustomers.Add(customer);
            _context.SaveChanges();
        }
        //console.update
        public void UpdateCustomer(SiCustomer customer)
        {
            var existingCustomer = _context.SiCustomers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Address = customer.Address;
                _context.SaveChanges();
            }
        }
        //console.remove
        public void RemoveCustomer(int CustomerId)
        {
            var customer = _context.SiCustomers.FirstOrDefault(c => c.CustomerId == CustomerId);
            if (customer != null)
            {
                _context.SiCustomers.Remove(customer);
                _context.SaveChanges();
            }
        }
    }
}

*/
