CAMPUS REGISTRY — PBKK WEEK 5 (WPF, MVVM, REPOSITORY, EF CORE + SQLITE)
====================================================================

Folder CampusRegistryMVVM adalah satu project WPF lengkap.
Tidak perlu menyalin file dari Week 4, dan jangan mengganti file Week 4.

SYARAT
- Windows 10/11
- .NET 10 SDK dan Visual Studio dengan workload .NET desktop development
- Koneksi internet saat pertama kali NuGet restore

INSTALASI
1. Ekstrak ZIP di C:\AC\A\PBKK\Tugas5\ (hasilnya: Tugas5\CampusRegistryMVVM\...)
2. Buka PowerShell di folder CampusRegistryMVVM.
3. Jalankan berurutan:

   dotnet restore
   dotnet tool restore
   dotnet ef migrations add InitialCreate
   dotnet run

   Migrasi dibuat satu kali saja, jangan diulang dengan nama sama.
   Pada run pertama, migrasi dijalankan otomatis dan file SQLite dibuat.

4. Alternatif: buka CampusRegistryMVVM.csproj di Visual Studio, tetapi migrasi
   di atas tetap perlu dibuat satu kali sebelum dijalankan.

PENYIMPANAN
Database SQLite berada di:
%LOCALAPPDATA%\CampusRegistryMVVM\campusregistry.db
Data TETAP ADA setelah aplikasi ditutup dan build ulang.
File database personal tidak ikut di-upload ke GitHub.

FITUR
- Tambah, edit, hapus (konfirmasi di dalam aplikasi)
- Live search NRP, nama, prodi
- DataGrid dengan sorting
- Validasi NRP 10 digit dan unik, nama min. 3 karakter, prodi, IPK 0-4,
  gender, tanggal lahir usia minimal 15, alamat min. 5 karakter,
  telepon 10-14 digit diawali 08 atau 628
- Empat ringkasan: jumlah, rata-rata IPK, IPK tertinggi & terendah
- MVVM via CommunityToolkit.Mvvm, Repository Pattern, EF Core & SQLite

CATATAN PENTING
- Jika sebelumnya sudah pernah membuat database manual tanpa migrasi,
  jangan hapus database sembarangan karena data akan hilang.
- UI Windows WPF tidak dapat diuji dengan dotnet di lingkungan Linux.
  Kompilasi dan jalankan di Windows menggunakan dotnet run.
- MainWindow.xaml tidak mempunyai badge Week 4 ataupun status bar bawah.
