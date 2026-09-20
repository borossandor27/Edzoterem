using Edzoterem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edzoterem.Infrastructure.Data;

public class EdzoteremDbContext : DbContext
{
    public EdzoteremDbContext(DbContextOptions<EdzoteremDbContext> options) : base(options)
    {
    }

    public DbSet<Munkakor> Munkakorok => Set<Munkakor>();
    public DbSet<Dolgozo> Dolgozok => Set<Dolgozo>();
    public DbSet<Edzo> Edzok => Set<Edzo>();
    public DbSet<EdzoElerhetoseg> EdzoElerhetosegek => Set<EdzoElerhetoseg>();
    public DbSet<Munkarend> Munkarendek => Set<Munkarend>();
    public DbSet<Tag> Tagok => Set<Tag>();
    public DbSet<Felhasznalo> Felhasznalok => Set<Felhasznalo>();
    public DbSet<BerletTipus> BerletTipusok => Set<BerletTipus>();
    public DbSet<Berlet> Berletek => Set<Berlet>();
    public DbSet<SzolgaltatasTipus> SzolgaltatasTipusok => Set<SzolgaltatasTipus>();
    public DbSet<CsoportosFoglalkozas> CsoportosFoglalkozasok => Set<CsoportosFoglalkozas>();
    public DbSet<CsoportosJelentkezes> CsoportosJelentkezesek => Set<CsoportosJelentkezes>();
    public DbSet<EgyeniFoglalkozas> EgyeniFoglalkozasok => Set<EgyeniFoglalkozas>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Dolgozo>(e =>
        {
            e.HasOne(d => d.Munkakor).WithMany(m => m.Dolgozok).HasForeignKey(d => d.MunkakorId);
            e.Property(d => d.Oradij).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Edzo>(e =>
        {
            e.HasOne(x => x.Dolgozo).WithOne(d => d.Edzo).HasForeignKey<Edzo>(x => x.DolgozoId);
            e.Property(x => x.Oradij).HasPrecision(10, 2);
        });

        modelBuilder.Entity<EdzoElerhetoseg>(e =>
        {
            e.HasOne(x => x.Edzo).WithMany(ed => ed.Elerhetosegek).HasForeignKey(x => x.EdzoId);
        });

        modelBuilder.Entity<Munkarend>(e =>
        {
            e.HasOne(x => x.Dolgozo).WithMany(d => d.Munkarendek).HasForeignKey(x => x.DolgozoId);
        });

        modelBuilder.Entity<Felhasznalo>(e =>
        {
            e.HasIndex(x => x.Felhasznalonev).IsUnique();
            e.HasOne(x => x.Dolgozo).WithOne(d => d.Felhasznalo).HasForeignKey<Felhasznalo>(x => x.DolgozoId);
            e.HasOne(x => x.Tag).WithOne(t => t.Felhasznalo).HasForeignKey<Felhasznalo>(x => x.TagId);
        });

        modelBuilder.Entity<BerletTipus>(e =>
        {
            e.Property(x => x.Ar).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Berlet>(e =>
        {
            e.HasOne(x => x.Tag).WithMany(t => t.Berletek).HasForeignKey(x => x.TagId);
            e.HasOne(x => x.BerletTipus).WithMany(bt => bt.Berletek).HasForeignKey(x => x.BerletTipusId);
            e.HasOne(x => x.LetrehozvaAltal).WithMany().HasForeignKey(x => x.LetrehozvaAltalId).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.ArFizetve).HasPrecision(10, 2);
        });

        modelBuilder.Entity<SzolgaltatasTipus>(e =>
        {
            e.Property(x => x.AlapertelmezettAr).HasPrecision(10, 2);
        });

        modelBuilder.Entity<CsoportosFoglalkozas>(e =>
        {
            e.HasOne(x => x.Edzo).WithMany(ed => ed.CsoportosFoglalkozasok).HasForeignKey(x => x.EdzoId);
            e.HasOne(x => x.Meghirdette).WithMany().HasForeignKey(x => x.MeghirdetteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Jovahagyta).WithMany().HasForeignKey(x => x.JovahagytaId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CsoportosJelentkezes>(e =>
        {
            e.HasOne(x => x.CsoportosFoglalkozas).WithMany(c => c.Jelentkezesek).HasForeignKey(x => x.CsoportosFoglalkozasId);
            e.HasOne(x => x.Tag).WithMany(t => t.CsoportosJelentkezesek).HasForeignKey(x => x.TagId);
            e.HasOne(x => x.RogzitveAltal).WithMany().HasForeignKey(x => x.RogzitveAltalId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => new { x.CsoportosFoglalkozasId, x.TagId }).IsUnique();
        });

        modelBuilder.Entity<EgyeniFoglalkozas>(e =>
        {
            e.HasOne(x => x.SzolgaltatasTipus).WithMany(s => s.EgyeniFoglalkozasok).HasForeignKey(x => x.SzolgaltatasTipusId);
            e.HasOne(x => x.Dolgozo).WithMany().HasForeignKey(x => x.DolgozoId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Tag).WithMany(t => t.EgyeniFoglalkozasok).HasForeignKey(x => x.TagId);
            e.HasOne(x => x.RogzitveAltal).WithMany().HasForeignKey(x => x.RogzitveAltalId).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Ar).HasPrecision(10, 2);
        });
    }
}
