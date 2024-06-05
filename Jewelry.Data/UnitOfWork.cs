using Jewelry.Data.Models;
using Jewelry.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry.Data
{
    public class UnitOfWork
    {
        private ProductRepository _product;
        private CategoryRepository _category;
        private OrderItemRepository _orderItem;
        private CustomerRepository _customer;
        private readonly NET1814_212_4_JewelryContext _unitOfWorkContext;

        public UnitOfWork()
        {
            _unitOfWorkContext ??= new NET1814_212_4_JewelryContext();
        }
        public ProductRepository ProductRepository
        {
            get
            {
                return _product??= new Repository.ProductRepository(_unitOfWorkContext);
            }
        }

        public CategoryRepository CategoryRepository
        {
            get
            {
                return _category??= new Repository.CategoryRepository();
            }
        }
        public OrderItemRepository OrderItemRepository
        {
            get
            {
                return _orderItem ??= (_orderItem = new OrderItemRepository());
            }
        }
        public CustomerRepository CustomerRepository
        {
            get
            {
                return (_customer ??= new Repository.CustomerRepository());
            }
        }
        ////TO-DO CODE HERE/////////////////

        #region Set transaction isolation levels

        /*
        Read Uncommitted: The lowest level of isolation, allows transactions to read uncommitted data from other transactions. This can lead to dirty reads and other issues.

        Read Committed: Transactions can only read data that has been committed by other transactions. This level avoids dirty reads but can still experience other isolation problems.

        Repeatable Read: Transactions can only read data that was committed before their execution, and all reads are repeatable. This prevents dirty reads and non-repeatable reads, but may still experience phantom reads.

        Serializable: The highest level of isolation, ensuring that transactions are completely isolated from one another. This can lead to increased lock contention, potentially hurting performance.

        Snapshot: This isolation level uses row versioning to avoid locks, providing consistency without impeding concurrency. 
         */

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = _unitOfWorkContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = await _unitOfWorkContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        #endregion
    }
}
