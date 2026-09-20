namespace Edzoterem.Domain.Enums;

public enum Nem
{
    Ferfi,
    No,
    Egyeb
}

public enum Szerepkor
{
    Tulajdonos,
    Recepcios,
    Edzo,
    Tag
}

public enum HetNapja
{
    Hetfo,
    Kedd,
    Szerda,
    Csutortok,
    Pentek,
    Szombat,
    Vasarnap
}

public enum BerletKategoria
{
    Napi,
    Havi,
    Feleves,
    Eves,
    Alkalmas
}

public enum BerletStatusz
{
    Aktiv,
    Lejart,
    Felfuggesztve
}

public enum CsoportosFoglalkozasStatusz
{
    JovahagyasraVar,
    Jovahagyva,
    Elutasitva,
    Lezarva
}

public enum EgyeniFoglalkozasAllapot
{
    Lefoglalva,
    Teljesitve,
    Lemondva
}
