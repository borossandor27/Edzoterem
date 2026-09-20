using Edzoterem.Domain.Enums;

namespace Edzoterem.Domain.Entities;

public class EdzoElerhetoseg
{
    public int Id { get; set; }
    public int EdzoId { get; set; }
    public Edzo? Edzo { get; set; }
    public HetNapja HetNapja { get; set; }
    public TimeOnly TolIdo { get; set; }
    public TimeOnly IgIdo { get; set; }
}
