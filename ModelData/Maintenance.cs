using System;
using System.Collections.Generic;

namespace WpfAppUserInterface.ModelData;

public partial class Maintenance
{
    public int MaintenanceId { get; set; }

    public int ScaleId { get; set; }

    public DateOnly MaintenanceDate { get; set; }

    public string MaintenanceType { get; set; } = null!;

    public int TechnicianId { get; set; }

    public string? Description { get; set; }

    public virtual Scale Scale { get; set; } = null!;

    public virtual User Technician { get; set; } = null!;
}
