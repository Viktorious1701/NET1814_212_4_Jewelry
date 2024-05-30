﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Jewelry.Data.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string StartDate { get; set; }

    public string EndDate { get; set; }

    public string DiscountPercentage { get; set; }

    public int? CompanyId { get; set; }

    public virtual SiCompany Company { get; set; }

    public virtual ICollection<SiOrder> SiOrders { get; set; } = new List<SiOrder>();
}