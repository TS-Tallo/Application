using System;
using System.Collections.Generic;

namespace CadreApp.TableEntities;

public partial class trip_location
{
    public string trip_id { get; set; } = null!;

    public string location { get; set; } = null!;

    public virtual trip trip { get; set; } = null!;
}
