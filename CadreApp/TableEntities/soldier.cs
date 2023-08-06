using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

public partial class soldier
{
    [Key]
    public long ID { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? rank { get; set; }

    [StringLength(32)]
    [Unicode(false)]
    public string? name_first { get; set; }

    [StringLength(32)]
    [Unicode(false)]
    public string? name_last { get; set; }

    [StringLength(9)]
    [Unicode(false)]
    public string? building { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? company { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? platoon { get; set; }

    public long? phone_num { get; set; }
}
