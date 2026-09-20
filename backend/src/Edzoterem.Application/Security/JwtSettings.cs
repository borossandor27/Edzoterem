namespace Edzoterem.Application.Security;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Kulcs { get; set; } = string.Empty;
    public string Kiado { get; set; } = "Edzoterem.Api";
    public string Kozonseg { get; set; } = "Edzoterem.Kliens";
    public int LejaratPercben { get; set; } = 480;
}
