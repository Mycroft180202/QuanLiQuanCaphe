﻿using System;
using System.Collections.Generic;

namespace QuanLiQuanCaphe.Models;

public partial class Table
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsOccupied { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
