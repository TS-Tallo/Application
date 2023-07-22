using System;
using System.Collections.Generic;

namespace CadreApp.TableEntities;

public partial class soldier
{
    public long ID { get; set; }

    public string? rank { get; set; }

    public string? name_first { get; set; }

    public string? name_last { get; set; }

    public string? building { get; set; }

    public string? company { get; set; }

    public string? platoon { get; set; }
}
