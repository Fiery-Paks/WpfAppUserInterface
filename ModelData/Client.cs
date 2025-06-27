using System;
using System.Collections.Generic;

namespace WpfAppUserInterface.ModelData;

public partial class Client
{
    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Weighing> Weighings { get; set; } = new List<Weighing>();
}
