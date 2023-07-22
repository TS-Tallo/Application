using System;
using System.Collections.Generic;

namespace CadreApp.TableEntities;

public partial class account
{
    public long ID { get; set; }

    public string username { get; set; } = null!;

    public string password { get; set; } = null!;

    public virtual soldier IDNavigation { get; set; } = null!;
}
