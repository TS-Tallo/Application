using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

[Keyless]
public partial class Trip_Active
{
    [StringLength(10)]
    [Unicode(false)]
    public string ID { get; set; } = null!;

    [StringLength(32)]
    public string? cadrein_name { get; set; }

    [StringLength(32)]
    public string? cadreout_name { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? time_in { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime time_out { get; set; }

    public string? destinations { get; set; }

    public string? soldiers { get; set; }

    public string? platoons { get; set; }

    public string? companies { get; set; }

    public string? phone_numbers { get; set; }
}
