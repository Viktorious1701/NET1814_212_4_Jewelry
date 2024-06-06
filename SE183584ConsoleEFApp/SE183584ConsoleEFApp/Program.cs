// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using SE183584ConsoleEFApp.DAO;
using SE183584ConsoleEFApp.Models;

var _context = new Net1814_212_4_JewelryContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Net1814_212_4_JewelryContext>());

var customer = _context.SiCustomers.ToArray();

foreach (var Customer in customer)
{
    Console.WriteLine(Customer.Name);
}

Console.WriteLine("1. select * from SiCustomers");

foreach (var Customer in customer)
{
    Console.WriteLine(Customer.Name);
}

Console.WriteLine("2. select * from SiCustomers where CustomerId = 1");
var item = _context.SiCustomers.FirstOrDefault(c => c.CustomerId == 1);
if (item != null)
{
    Console.WriteLine(item.CustomerId);
}


Console.WriteLine("3. insert into SI_Customer(CustomerId, Name, Phone, Address) value('6', 'Manh', '0764374730')");
item = _context.SiCustomers.FirstOrDefault(c => c. == "CAD");
if (item == null)
{
    item = new SiCustomer();
   /* item. = "CAD";
    item. = "Canada";*/
    _context.Add(item);
    _context.SaveChanges();
}

Console.WriteLine("4. update Currency set CurrencyName = N'Đồng' where CurrencyCode = 'VND'");
item = _context.SiCustomers.FirstOrDefault(c => c.CustomerId == 3);
if (item != null)
{
    item.CustomerId = 3;
    _context.Update(item);
    _context.SaveChanges();
}


Console.WriteLine("5. delete Currency where CurrencyCode = 'VND'");
item = _context.SiCustomers.FirstOrDefault(c => c.CustomerId == 4);
if (item != null)
{
    _context.Remove(item);
    _context.SaveChanges();
}



Console.WriteLine("Call DAO ");
Console.WriteLine("DAO.getAll ");

var customerDAO = new CustomerDAO();
var customerList = customerDAO.GetAll();
foreach (var Customer in customerList)
{
    Console.WriteLine(Customer.Name);
}

Console.WriteLine("DAO.getById");
var item = customerDAO.GetByCode("1");
foreach (var Customer in customerList)
{
    Console.WriteLine(Customer.CustomerId);
}
