using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

[Keyless]
[Table("trip-locations")]
public partial class trip_location
{
    [StringLength(10)]
    [Unicode(false)]
    public string trip_id { get; set; } = null!;

    [StringLength(32)]
    [Unicode(false)]
    public string location { get; set; } = null!;

    [ForeignKey("trip_id")]
    public virtual trip trip { get; set; } = null!;
}
