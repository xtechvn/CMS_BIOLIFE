using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class BankOnePay
{
    public int Id { get; set; }

    public string BankName { get; set; }

    public string Code { get; set; }

    public byte Type { get; set; }

    public string Logo { get; set; }

    public byte? Status { get; set; }

    public string FullnameEn { get; set; }

    public string FullnameVi { get; set; }
}
