﻿using Jewelry.Business.Base;
using Jewelry.Data;
using Jewelry.Data.Models;
using Jewelry.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jewelry.Business
{
    public interface IProductBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetAllWithCategory();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Save(SiProduct product);
        Task<IBusinessResult> Update(SiProduct product);
        Task<IBusinessResult> DeleteById(int id);
    }

    public class ProductBusiness : IProductBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductBusiness()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IBusinessResult> DeleteById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product != null)
                {
                    var result = await _unitOfWork.ProductRepository.RemoveAsync(product);
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
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                if (products == null || !products.Any())
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllWithCategory()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

                var productsWithCategories = products
                    .Select(product => new
                    {
                        Product = product,
                        Category = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId)
                    })
                    .Select(pc => new
                    {
                        pc.Product.ProductId,
                        pc.Product.Name,
                        pc.Product.Description,
                        pc.Product.Barcode,
                        pc.Product.Weight,
                        pc.Product.CostPrice,
                        pc.Product.GoldPrice,
                        pc.Product.LaborCost,
                        pc.Product.StoneCost,
                        pc.Product.SellPriceRatio,
                        Category = pc.Category
                    });

                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, productsWithCategories);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(SiProduct product)
        {
            try
            {
                int result = await _unitOfWork.ProductRepository.CreateAsync(product);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> Update(SiProduct product)
        {
            try
            {
                int result = await _unitOfWork.ProductRepository.UpdateAsync(product);
                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
