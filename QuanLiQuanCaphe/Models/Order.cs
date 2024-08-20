using System;
using System.Collections.Generic;

namespace QuanLiQuanCaphe.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public int? TableId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Table? Table { get; set; }

    public virtual User User { get; set; } = null!;
}
