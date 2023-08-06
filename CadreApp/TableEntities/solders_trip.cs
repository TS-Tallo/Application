using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

[Keyless]
[Table("solders-trips")]
public partial class solders_trip
{
    [StringLength(10)]
    [Unicode(false)]
    public string? trip_id { get; set; }

    public long? solders { get; set; }

    [ForeignKey("solders")]
    public virtual soldier? soldersNavigation { get; set; }

    [ForeignKey("trip_id")]
    public virtual trip? trip { get; set; }
}
