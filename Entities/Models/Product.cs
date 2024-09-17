using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Product
{
    public long Id { get; set; }

    public string ProductCode { get; set; }

    public string Title { get; set; }

    public double Price { get; set; }

    public int? Discount { get; set; }

    public double? Amount { get; set; }

    public string Rating { get; set; }

    public string Manufacturer { get; set; }

    public int LabelId { get; set; }

    public int? ReviewsCount { get; set; }

    public bool? IsPrimeEligible { get; set; }

    public int RateCurrent { get; set; }

    public string SellerId { get; set; }

    public string SellerName { get; set; }

    public DateTime? CreateOn { get; set; }

    public DateTime? UpdateLast { get; set; }

    public int? GroupProductId { get; set; }

    public string Description { get; set; }

    public string Information { get; set; }

    public string Variations { get; set; }

    public int? ProductMapId { get; set; }

    public string Path { get; set; }

    public string LinkSource { get; set; }

    public long? ParentId { get; set; }

    public double? ItemWeight { get; set; }

    public string UnitWeight { get; set; }
}
