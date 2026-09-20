namespace Edzoterem.Desktop.Services;

public class SessionState
{
    public string? Token { get; set; }
    public string? Szerepkor { get; set; }
    public string? Nev { get; set; }

    public bool VanBejelentkezve => !string.IsNullOrEmpty(Token);
    public bool Tulajdonos => Szerepkor == "Tulajdonos";
    public bool Recepcios => Szerepkor == "Recepcios";

    public void Kijelentkezes()
    {
        Token = null;
        Szerepkor = null;
        Nev = null;
    }
}
