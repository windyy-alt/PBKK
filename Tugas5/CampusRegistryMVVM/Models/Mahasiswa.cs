namespace CampusRegistryMVVM.Models;

public class Mahasiswa
{
    public int Id { get; set; }
    public string Nrp { get; set; } = "";
    public string Nama { get; set; } = "";
    public string Prodi { get; set; } = "";
    public decimal Ipk { get; set; }
    public string JenisKelamin { get; set; } = "";
    public DateTime? TanggalLahir { get; set; }
    public string Alamat { get; set; } = "";
    public string NoTelepon { get; set; } = "";
}
