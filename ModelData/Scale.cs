using System;
using System.Collections.Generic;

namespace WpfAppUserInterface.ModelData;

public partial class Scale
{
    public int ScaleId { get; set; }

    public string Model { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public decimal MaxCapacity { get; set; }

    public DateOnly? InstallationDate { get; set; }

    public DateOnly? LastCalibrationDate { get; set; }

    public string? Location { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    public virtual ICollection<Weighing> Weighings { get; set; } = new List<Weighing>();
}
