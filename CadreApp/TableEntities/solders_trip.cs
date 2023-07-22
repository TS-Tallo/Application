using System;
using System.Collections.Generic;

namespace CadreApp.TableEntities;

public partial class solders_trip
{
    public string? trip_id { get; set; }

    public long? solders { get; set; }

    public virtual soldier? soldersNavigation { get; set; }

    public virtual trip? trip { get; set; }
}
