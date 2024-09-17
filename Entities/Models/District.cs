using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class District
{
    public int Id { get; set; }

    public string DistrictId { get; set; }

    public string Name { get; set; }

    public string NameNonUnicode { get; set; }

    public string Type { get; set; }

    public string Location { get; set; }

    public string ProvinceId { get; set; }

    public short? Status { get; set; }
}
