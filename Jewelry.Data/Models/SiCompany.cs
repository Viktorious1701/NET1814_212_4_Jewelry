﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Jewelry.Data.Models;

public partial class SiCompany
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string CompanyAddress { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}