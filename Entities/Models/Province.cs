using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Province
{
    public int Id { get; set; }

    public string ProvinceId { get; set; }

    public string Name { get; set; }

    public string NameNonUnicode { get; set; }

    public string Type { get; set; }

    public short? Status { get; set; }
}
