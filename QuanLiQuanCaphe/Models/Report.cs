using System;
using System.Collections.Generic;

namespace QuanLiQuanCaphe.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportName { get; set; } = null!;

    public DateTime ReportDate { get; set; }

    public string FilePath { get; set; } = null!;

    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
