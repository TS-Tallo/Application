using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

public partial class trip
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string ID { get; set; } = null!;

    public long? cadrein_ID { get; set; }

    public long cadreout_ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? time_in { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime time_out { get; set; }

    public int active { get; set; }

    public string? destinations { get; set; }
}
