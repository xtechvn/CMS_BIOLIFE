using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AccountClient
{
    public int Id { get; set; }

    public long? ClientId { get; set; }

    public int? ClientType { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string PasswordBackup { get; set; }

    public string ForgotPasswordToken { get; set; }

    public byte? Status { get; set; }

    public int? GroupPermission { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateLast { get; set; }

    public string GoogleToken { get; set; }
}
