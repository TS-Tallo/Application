using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.TableEntities;

[Keyless]
public partial class account
{
    public long ID { get; set; }

    [StringLength(256)]
    public string password { get; set; } = null!;

    [StringLength(32)]
    public string? name { get; set; }

    [ForeignKey("ID")]
    public virtual soldier IDNavigation { get; set; } = null!;
}
