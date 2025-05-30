﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OICAR.Models;

[Table("Item")]
public partial class Item
{
    [Key]
    [Column("IDItem")]
    public int Iditem { get; set; }

    [Column("ItemCategoryID")]
    public int ItemCategoryId { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Title { get; set; }

    [Required]
    [Unicode(false)]
    public string Description { get; set; }

    public int StockQuantity { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Weight { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("ItemCategoryId")]
    [InverseProperty("Items")]
    public virtual ItemCategory ItemCategory { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}