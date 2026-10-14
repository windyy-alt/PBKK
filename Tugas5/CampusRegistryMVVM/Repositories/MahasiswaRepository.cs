using CampusRegistryMVVM.Data;
using CampusRegistryMVVM.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusRegistryMVVM.Repositories;

public class MahasiswaRepository : IMahasiswaRepository
{
    public async Task InitializeAsync()
    {
        await using var db = new AppDbContext();
        await db.Database.MigrateAsync();
    }

    public async Task<List<Mahasiswa>> GetAllAsync()
    {
        await using var db = new AppDbContext();
        return await db.Mahasiswa.AsNoTracking().OrderBy(m => m.Nrp).ToListAsync();
    }

    public async Task<bool> NrpExistsAsync(string nrp, int? exceptId = null)
    {
        await using var db = new AppDbContext();
        return await db.Mahasiswa.AnyAsync(m => m.Nrp == nrp &&
            (!exceptId.HasValue || m.Id != exceptId.Value));
    }

    public async Task AddAsync(Mahasiswa mahasiswa)
    {
        await using var db = new AppDbContext();
        db.Mahasiswa.Add(mahasiswa);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Mahasiswa mahasiswa)
    {
        await using var db = new AppDbContext();
        db.Mahasiswa.Update(mahasiswa);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = new AppDbContext();
        var mahasiswa = await db.Mahasiswa.FindAsync(id);
        if (mahasiswa is null) return;
        db.Mahasiswa.Remove(mahasiswa);
        await db.SaveChangesAsync();
    }
}
