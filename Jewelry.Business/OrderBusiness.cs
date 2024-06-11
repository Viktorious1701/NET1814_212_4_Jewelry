using Jewelry.Business.Base;
using Jewelry.Common;
using Jewelry.Data;
using Jewelry.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Jewelry.Business
{
    public interface IOrderBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(String code);
        Task<IBusinessResult> Save(SiOrder order);
        Task<IBusinessResult> Update(SiOrder order);
        Task<IBusinessResult> DeleteById(int code);

    }
    public class OrderBusiness : IOrderBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly NET1814_212_4_JewelryContext _context;

        public OrderBusiness(UnitOfWork unitOfWork, NET1814_212_4_JewelryContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                #region Business rule
                #endregion

                var currencies = await _unitOfWork.OrderRepository.GetAllAsync();


                if (currencies == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, currencies);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(string code)
        {
            try
            {
                #region Business rule
                #endregion

                var currency = await _unitOfWork.OrderRepository.GetByIdAsync(code);

                if (currency == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, currency);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        public void Detach(SiOrder order)
        {
            var entry = _context.Entry(order);
            entry.State = EntityState.Detached;
        }

        public async Task<IBusinessResult> Save(SiOrder order)
        {
            try
            {
                int result; // Define result here so it's accessible in the entire method scope

                // Check if the order already exists in the context
                var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(order.OrderId);

                if (existingOrder != null)
                {
                    // If the order exists, update it
                    _unitOfWork.OrderRepository.Detach(existingOrder);
                    existingOrder = order;
                    result = await _unitOfWork.OrderRepository.UpdateAsync(existingOrder);
                }
                else
                {
                    // If the order doesn't exist, create it
                    result = await _unitOfWork.OrderRepository.CreateAsync(order);
                }

                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        public async Task<IBusinessResult> Update(SiOrder order)
        {
            try
            {
                int result = await _unitOfWork.OrderRepository.UpdateAsync(order);

                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }

        public async Task<IBusinessResult> DeleteById(int id)
        {
            try
            {
                var currency = await _unitOfWork.OrderRepository.GetByIdAsync(id);
                if (currency != null)
                {
                    var result = await _unitOfWork.OrderRepository.RemoveAsync(currency);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.ToString());
            }
        }
    }
}
