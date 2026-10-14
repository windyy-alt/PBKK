using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using CampusRegistryMVVM.Models;
using CampusRegistryMVVM.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusRegistryMVVM.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IMahasiswaRepository repository;
    private List<Mahasiswa> semuaMahasiswa = new();
    private int? idSedangDiedit;

    public ObservableCollection<Mahasiswa> MahasiswaTampil { get; } = new();

    [ObservableProperty] private string nrp = "";
    [ObservableProperty] private string nama = "";
    [ObservableProperty] private string prodi = "";
    [ObservableProperty] private string ipk = "";
    [ObservableProperty] private string jenisKelamin = "";
    [ObservableProperty] private DateTime? tanggalLahir;
    [ObservableProperty] private string alamat = "";
    [ObservableProperty] private string noTelepon = "";
    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private string formTitle = "Tambah Mahasiswa";
    [ObservableProperty] private string saveButtonText = "+ Simpan Mahasiswa";
    [ObservableProperty] private string pesanForm = "";
    [ObservableProperty] private string jumlahMahasiswa = "0";
    [ObservableProperty] private string rataRataIpk = "-";
    [ObservableProperty] private string ipkTertinggi = "-";
    [ObservableProperty] private string ipkTerendah = "-";
    [ObservableProperty] private bool sedangMemuat;
    [ObservableProperty] private bool isDeleteConfirmationOpen;
    [ObservableProperty] private Mahasiswa? mahasiswaTerpilih;

    public MainViewModel(IMahasiswaRepository repository)
    {
        this.repository = repository;
    }

    partial void OnSearchTextChanged(string value) => FilterData();

    partial void OnMahasiswaTerpilihChanged(Mahasiswa? value)
    {
        EditCommand.NotifyCanExecuteChanged();
        MintaHapusCommand.NotifyCanExecuteChanged();
        if (IsDeleteConfirmationOpen) IsDeleteConfirmationOpen = false;
    }

    public async Task LoadAsync()
    {
        SedangMemuat = true;
        try
        {
            await repository.InitializeAsync();
            await SegarkanDataAsync();
        }
        catch (Exception ex)
        {
            PesanForm = "Gagal membuka database: " + ex.Message +
                " (Pastikan migrasi sudah dibuat dan diterapkan.)";
        }
        finally
        {
            SedangMemuat = false;
        }
    }

    private async Task SegarkanDataAsync()
    {
        semuaMahasiswa = await repository.GetAllAsync();
        FilterData();
        PerbaruiStatistik();
    }

    private void FilterData()
    {
        string key = SearchText.Trim();
        IEnumerable<Mahasiswa> hasil = semuaMahasiswa.Where(m =>
            m.Nrp.Contains(key, StringComparison.OrdinalIgnoreCase) ||
            m.Nama.Contains(key, StringComparison.OrdinalIgnoreCase) ||
            m.Prodi.Contains(key, StringComparison.OrdinalIgnoreCase));
        MahasiswaTampil.Clear();
        foreach (var m in hasil) MahasiswaTampil.Add(m);
    }

    private void PerbaruiStatistik()
    {
        JumlahMahasiswa = semuaMahasiswa.Count.ToString();
        if (semuaMahasiswa.Count == 0)
        {
            RataRataIpk = IpkTertinggi = IpkTerendah = "-";
            return;
        }
        RataRataIpk = semuaMahasiswa.Average(m => m.Ipk).ToString("0.00", CultureInfo.InvariantCulture);
        IpkTertinggi = semuaMahasiswa.Max(m => m.Ipk).ToString("0.00", CultureInfo.InvariantCulture);
        IpkTerendah = semuaMahasiswa.Min(m => m.Ipk).ToString("0.00", CultureInfo.InvariantCulture);
    }

    private bool Validasi(out Mahasiswa mahasiswa)
    {
        mahasiswa = new Mahasiswa();
        string nrpBaru = Nrp.Trim();
        string namaBaru = Nama.Trim();
        string prodiBaru = Prodi.Trim();
        string alamatBaru = Alamat.Trim();
        string telepon = Regex.Replace(NoTelepon, @"[\s\-+]", "");

        if (!Regex.IsMatch(nrpBaru, @"^[0-9]{10}$"))
            PesanForm = "NRP harus tepat 10 digit angka.";
        else if (namaBaru.Length < 3)
            PesanForm = "Nama wajib diisi (minimal 3 karakter).";
        else if (prodiBaru.Length == 0)
            PesanForm = "Program studi wajib diisi.";
        else if (!decimal.TryParse(Ipk.Trim().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
                     CultureInfo.InvariantCulture, out decimal ipkBaru) || ipkBaru < 0 || ipkBaru > 4)
            PesanForm = "IPK harus berupa angka 0 sampai 4.";
        else if (JenisKelamin != "Laki-laki" && JenisKelamin != "Perempuan")
            PesanForm = "Jenis kelamin wajib dipilih.";
        else if (!TanggalLahir.HasValue || TanggalLahir.Value.Date > DateTime.Today ||
                 TanggalLahir.Value.Date > DateTime.Today.AddYears(-15))
            PesanForm = "Masukkan tanggal lahir yang valid (usia minimal 15 tahun).";
        else if (alamatBaru.Length < 5)
            PesanForm = "Alamat wajib diisi (minimal 5 karakter).";
        else if (!Regex.IsMatch(telepon, @"^(08[0-9]{8,12}|628[0-9]{8,11})$"))
            PesanForm = "Nomor telepon harus diawali 08 atau 628, panjang 10-14 digit.";
        else
        {
            mahasiswa = new Mahasiswa
            {
                Id = idSedangDiedit ?? 0,
                Nrp = nrpBaru,
                Nama = namaBaru,
                Prodi = prodiBaru,
                Ipk = ipkBaru,
                JenisKelamin = JenisKelamin,
                TanggalLahir = TanggalLahir,
                Alamat = alamatBaru,
                NoTelepon = telepon
            };
            return true;
        }
        return false;
    }

    [RelayCommand]
    private async Task SimpanAsync()
    {
        PesanForm = "";
        if (!Validasi(out var mahasiswa)) return;
        try
        {
            if (await repository.NrpExistsAsync(mahasiswa.Nrp, idSedangDiedit))
            {
                PesanForm = "NRP ini sudah digunakan mahasiswa lain.";
                return;
            }
            bool isEdit = idSedangDiedit.HasValue;
            if (isEdit) await repository.UpdateAsync(mahasiswa);
            else await repository.AddAsync(mahasiswa);
            BersihkanFormInternal();
            await SegarkanDataAsync();
            PesanForm = isEdit ? "Data berhasil diperbarui." : "Mahasiswa berhasil ditambahkan.";
        }
        catch (Exception ex)
        {
            PesanForm = "Gagal menyimpan data: " + ex.Message;
        }
    }

    private bool BisaEdit() => MahasiswaTerpilih is not null;

    [RelayCommand(CanExecute = nameof(BisaEdit))]
    private void Edit()
    {
        if (MahasiswaTerpilih is not { } m) return;
        idSedangDiedit = m.Id;
        Nrp = m.Nrp;
        Nama = m.Nama;
        Prodi = m.Prodi;
        Ipk = m.Ipk.ToString("0.00", CultureInfo.InvariantCulture);
        JenisKelamin = m.JenisKelamin;
        TanggalLahir = m.TanggalLahir;
        Alamat = m.Alamat;
        NoTelepon = m.NoTelepon;
        FormTitle = "Edit Mahasiswa";
        SaveButtonText = "Simpan Perubahan";
        PesanForm = "";
    }

    [RelayCommand(CanExecute = nameof(BisaEdit))]
    private void MintaHapus()
    {
        IsDeleteConfirmationOpen = MahasiswaTerpilih is not null;
    }

    [RelayCommand]
    private void BatalHapus() => IsDeleteConfirmationOpen = false;

    [RelayCommand]
    private async Task KonfirmasiHapusAsync()
    {
        if (MahasiswaTerpilih is not { } m) return;
        try
        {
            await repository.DeleteAsync(m.Id);
            IsDeleteConfirmationOpen = false;
            BersihkanFormInternal();
            await SegarkanDataAsync();
            PesanForm = "Data mahasiswa berhasil dihapus.";
        }
        catch (Exception ex)
        {
            PesanForm = "Gagal menghapus data: " + ex.Message;
        }
    }

    [RelayCommand]
    private void BersihkanForm() => BersihkanFormInternal();

    private void BersihkanFormInternal()
    {
        idSedangDiedit = null;
        Nrp = Nama = Prodi = Ipk = JenisKelamin = Alamat = NoTelepon = "";
        TanggalLahir = null;
        FormTitle = "Tambah Mahasiswa";
        SaveButtonText = "+ Simpan Mahasiswa";
        IsDeleteConfirmationOpen = false;
        MahasiswaTerpilih = null;
        PesanForm = "";
    }
}
