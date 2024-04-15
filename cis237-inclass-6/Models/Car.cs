using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace cis237_inclass_6.Models;

public partial class Car
{
    [Key]
    [Column("id")]
    [StringLength(50)]
    public string Id { get; set; }

    [Required]
    [Column("year")]
    [StringLength(50)]
    public string Year { get; set; }

    [Required]
    [Column("make")]
    [StringLength(50)]
    public string Make { get; set; }

    [Required]
    [Column("model")]
    [StringLength(50)]
    public string Model { get; set; }

    [Required]
    [Column("type")]
    [StringLength(50)]
    public string Type { get; set; }

    [Column("horsepower")]
    public int Horsepower { get; set; }

    [Column("cylinders")]
    public int Cylinders { get; set; }
}
