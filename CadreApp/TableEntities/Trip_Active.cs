using System;
using System.Collections.Generic;

namespace CadreApp.TableEntities;

public partial class Trip_Active
{
    public string ID { get; set; } = null!;

    public long? cadrein_ID { get; set; }

    public long cadreout_ID { get; set; }

    public DateTime? time_in { get; set; }

    public DateTime time_out { get; set; }

    public int active { get; set; }
}
