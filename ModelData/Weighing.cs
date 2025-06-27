using System;
using System.Collections.Generic;

namespace WpfAppUserInterface.ModelData;

public partial class Weighing
{
    public int WeighingId { get; set; }

    public int ScaleId { get; set; }

    public int? ClientId { get; set; }

    public string? VehicleNumber { get; set; }

    public decimal? GrossWeight { get; set; }

    public decimal? TareWeight { get; set; }

    public DateTime WeighingDateTime { get; set; }

    public int OperatorId { get; set; }

    public string? Notes { get; set; }

    public virtual Client? Client { get; set; }

    public virtual User Operator { get; set; } = null!;

    public virtual Scale Scale { get; set; } = null!;
}
