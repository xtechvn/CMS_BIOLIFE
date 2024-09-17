using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public long ClientId { get; set; }

    public int ServiceType { get; set; }

    public double Amount { get; set; }

    public string ContractNo { get; set; }

    public int Status { get; set; }

    public int UserVerifyId { get; set; }

    public DateTime VerifyDate { get; set; }

    public string BankReference { get; set; }

    public int PaymentType { get; set; }

    public string Description { get; set; }

    public string TransactionNo { get; set; }
}
