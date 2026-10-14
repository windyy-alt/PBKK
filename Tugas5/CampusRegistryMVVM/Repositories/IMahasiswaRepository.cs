using CampusRegistryMVVM.Models;

namespace CampusRegistryMVVM.Repositories;

public interface IMahasiswaRepository
{
    Task InitializeAsync();
    Task<List<Mahasiswa>> GetAllAsync();
    Task<bool> NrpExistsAsync(string nrp, int? exceptId = null);
    Task AddAsync(Mahasiswa mahasiswa);
    Task UpdateAsync(Mahasiswa mahasiswa);
    Task DeleteAsync(int id);
}
