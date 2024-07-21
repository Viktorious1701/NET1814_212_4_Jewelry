using Jewelry.Business.Base;
using Jewelry.Data;
using Jewelry.Data.Models;
using Jewelry.Common;
using System.Collections;
using Microsoft.EntityFrameworkCore;
namespace Jewelry.Business
{
    public interface ICategoryBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Save(Category product);
        Task<IBusinessResult> Update(Category product);
        Task<IBusinessResult> DeleteById(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> DeleteById(int id)
        {
            try
            {
                //var Product = await _ProductRepository.GetByIdAsync(code);
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category != null)
                {
                    //var result = await _categoryRepository.RemoveAsync(category);
                    var result = await _unitOfWork.CategoryRepository.RemoveAsync(category);
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

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                #region Business rule
                #endregion

                //var category = _DAO.GetAll();
                //var category = await _ProductRepository.GetAllAsync();
                var category = await _unitOfWork.CategoryRepository.GetAllAsync();


                if (category == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, category);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                return categories ?? new List<Category>();
            }
            catch
            {
                // Optionally log the exception
                return new List<Category>();
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                #region Business rule
                #endregion

                //var Category = await _CategoryRepository.GetByIdAsync(code);
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

                if (category == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, category);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(Category category)
        {
            try
            {
                //int result = await _CategoryRepository.CreateAsync(category);
                int result = await _unitOfWork.CategoryRepository.CreateAsync(category);
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

        public async Task<IBusinessResult> Update(Category category)
        {
            try
            {
                //int result = await _CategoryRepository.UpdateAsync(category);
                int result = await _unitOfWork.CategoryRepository.UpdateAsync(category);

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
    }
}
