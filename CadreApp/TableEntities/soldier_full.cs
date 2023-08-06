using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

[Keyless]
public partial class soldier_full
{
    public long ID { get; set; }

    [StringLength(9)]
    [Unicode(false)]
    public string? building { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? company { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? platoon { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string rank_name { get; set; } = null!;

    [StringLength(13)]
    public string? formatted_phone { get; set; }
}
