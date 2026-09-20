using Edzoterem.Domain.Entities;
using Edzoterem.Domain.Enums;

namespace Edzoterem.Infrastructure.Data.Seed;

public static class DbSeeder
{
    public static void Seed(EdzoteremDbContext db)
    {
        if (db.Munkakorok.Any())
        {
            return;
        }

        var tulajdonosMunkakor = new Munkakor { Nev = "Tulajdonos" };
        var recepciosMunkakor = new Munkakor { Nev = "Recepciós" };
        var edzoMunkakor = new Munkakor { Nev = "Edző" };
        db.Munkakorok.AddRange(tulajdonosMunkakor, recepciosMunkakor, edzoMunkakor);
        db.SaveChanges();

        var tulajdonosDolgozo = new Dolgozo
        {
            Vezeteknev = "Kovács",
            Keresztnev = "István",
            MunkakorId = tulajdonosMunkakor.Id,
            SzuletesiDatum = new DateOnly(1980, 3, 12),
            Nem = Nem.Ferfi,
            Email = "tulajdonos@edzoterem.hu",
            Oradij = 0
        };

        var recepciosDolgozo = new Dolgozo
        {
            Vezeteknev = "Nagy",
            Keresztnev = "Petra",
            MunkakorId = recepciosMunkakor.Id,
            SzuletesiDatum = new DateOnly(1995, 7, 4),
            Nem = Nem.No,
            Email = "recepcio@edzoterem.hu",
            Oradij = 2500
        };

        var edzoDolgozo1 = new Dolgozo
        {
            Vezeteknev = "Szabó",
            Keresztnev = "Bence",
            MunkakorId = edzoMunkakor.Id,
            SzuletesiDatum = new DateOnly(1990, 1, 20),
            Nem = Nem.Ferfi,
            Email = "szabo.bence@edzoterem.hu",
            Oradij = 6000
        };

        var edzoDolgozo2 = new Dolgozo
        {
            Vezeteknev = "Tóth",
            Keresztnev = "Anna",
            MunkakorId = edzoMunkakor.Id,
            SzuletesiDatum = new DateOnly(1992, 9, 15),
            Nem = Nem.No,
            Email = "toth.anna@edzoterem.hu",
            Oradij = 6500
        };

        db.Dolgozok.AddRange(tulajdonosDolgozo, recepciosDolgozo, edzoDolgozo1, edzoDolgozo2);
        db.SaveChanges();

        var edzo1 = new Edzo { DolgozoId = edzoDolgozo1.Id, Szakterulet = "Erősítő edzés, funkcionális edzés", Vegzettseg = "Fitness edző OKJ", Oradij = 6000 };
        var edzo2 = new Edzo { DolgozoId = edzoDolgozo2.Id, Szakterulet = "Jóga, pilates", Vegzettseg = "Jóga oktató", Oradij = 6500 };
        db.Edzok.AddRange(edzo1, edzo2);
        db.SaveChanges();

        var tag1 = new Tag
        {
            Vezeteknev = "Kiss",
            Keresztnev = "Márton",
            SzuletesiDatum = new DateOnly(2001, 4, 2),
            Cim = "1011 Budapest, Fő utca 1.",
            Telefon = "+36301234567",
            Email = "kiss.marton@example.com",
            RegisztracioDatuma = DateOnly.FromDateTime(DateTime.UtcNow),
            VhKapcsolattartoNev = "Kiss Erika",
            VhKapcsolattartoTelefon = "+36301112233",
            Nem = Nem.Ferfi
        };

        var tag2 = new Tag
        {
            Vezeteknev = "Varga",
            Keresztnev = "Léna",
            SzuletesiDatum = new DateOnly(1998, 11, 30),
            Cim = "1024 Budapest, Kis utca 5.",
            Telefon = "+36309876543",
            Email = "varga.lena@example.com",
            RegisztracioDatuma = DateOnly.FromDateTime(DateTime.UtcNow),
            VhKapcsolattartoNev = "Varga Pál",
            VhKapcsolattartoTelefon = "+36304445566",
            Nem = Nem.No
        };

        db.Tagok.AddRange(tag1, tag2);
        db.SaveChanges();

        string Hash(string plain) => BCrypt.Net.BCrypt.HashPassword(plain);

        db.Felhasznalok.AddRange(
            new Felhasznalo { Felhasznalonev = "tulajdonos", JelszoHash = Hash("Jelszo123!"), Szerepkor = Szerepkor.Tulajdonos, DolgozoId = tulajdonosDolgozo.Id },
            new Felhasznalo { Felhasznalonev = "recepcio", JelszoHash = Hash("Jelszo123!"), Szerepkor = Szerepkor.Recepcios, DolgozoId = recepciosDolgozo.Id },
            new Felhasznalo { Felhasznalonev = "szabo.bence", JelszoHash = Hash("Jelszo123!"), Szerepkor = Szerepkor.Edzo, DolgozoId = edzoDolgozo1.Id },
            new Felhasznalo { Felhasznalonev = "toth.anna", JelszoHash = Hash("Jelszo123!"), Szerepkor = Szerepkor.Edzo, DolgozoId = edzoDolgozo2.Id },
            new Felhasznalo { Felhasznalonev = "kiss.marton", JelszoHash = Hash("Ideiglenes123!"), Szerepkor = Szerepkor.Tag, TagId = tag1.Id, JelszoIdeiglenes = true },
            new Felhasznalo { Felhasznalonev = "varga.lena", JelszoHash = Hash("Ideiglenes123!"), Szerepkor = Szerepkor.Tag, TagId = tag2.Id, JelszoIdeiglenes = true }
        );
        db.SaveChanges();

        var berletHavi = new BerletTipus { Nev = "Havi bérlet", Kategoria = BerletKategoria.Havi, Ar = 15000 };
        var berletAlkalmas = new BerletTipus { Nev = "10 alkalmas bérlet", Kategoria = BerletKategoria.Alkalmas, Ar = 25000, AlapertelmezettAlkalmak = 10 };
        db.BerletTipusok.AddRange(berletHavi, berletAlkalmas);
        db.SaveChanges();

        var tulajdonosFelhasznalo = db.Felhasznalok.First(f => f.Felhasznalonev == "tulajdonos");

        db.Berletek.AddRange(
            new Berlet
            {
                TagId = tag1.Id,
                BerletTipusId = berletHavi.Id,
                KezdoDatum = DateOnly.FromDateTime(DateTime.UtcNow),
                LejaratiDatum = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                ArFizetve = berletHavi.Ar,
                Statusz = BerletStatusz.Aktiv,
                LetrehozvaAltalId = tulajdonosFelhasznalo.Id
            },
            new Berlet
            {
                TagId = tag2.Id,
                BerletTipusId = berletAlkalmas.Id,
                KezdoDatum = DateOnly.FromDateTime(DateTime.UtcNow),
                LejaratiDatum = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                ArFizetve = berletAlkalmas.Ar,
                HatralevoAlkalmak = berletAlkalmas.AlapertelmezettAlkalmak,
                Statusz = BerletStatusz.Aktiv,
                LetrehozvaAltalId = tulajdonosFelhasznalo.Id
            }
        );

        db.SzolgaltatasTipusok.AddRange(
            new SzolgaltatasTipus { Nev = "Személyi edzés", AlapertelmezettAr = 8000 },
            new SzolgaltatasTipus { Nev = "Testösszetétel-mérés", AlapertelmezettAr = 3000 },
            new SzolgaltatasTipus { Nev = "Táplálkozási tanácsadás", AlapertelmezettAr = 6000 },
            new SzolgaltatasTipus { Nev = "Masszázs", AlapertelmezettAr = 9000 },
            new SzolgaltatasTipus { Nev = "Szolárium", AlapertelmezettAr = 2000 }
        );

        db.SaveChanges();
    }
}
