using CampusRegistryMVVM.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CampusRegistryMVVM.Data;

public class AppDbContext : DbContext
{
    public DbSet<Mahasiswa> Mahasiswa => Set<Mahasiswa>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Simpan di AppData agar tidak terhapus saat project dibuild ulang.
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CampusRegistryMVVM");
        Directory.CreateDirectory(folder);
        string dbPath = Path.Combine(folder, "campusregistry.db");
        options.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mahasiswa>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.Nrp).IsUnique();
            entity.Property(m => m.Nrp).HasMaxLength(10).IsRequired();
            entity.Property(m => m.Nama).HasMaxLength(100).IsRequired();
            entity.Property(m => m.Prodi).HasMaxLength(80).IsRequired();
            entity.Property(m => m.JenisKelamin).HasMaxLength(20);
            entity.Property(m => m.Alamat).HasMaxLength(255);
            entity.Property(m => m.NoTelepon).HasMaxLength(20);
        });
    }
}
