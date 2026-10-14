using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CampusRegistry
{
    public partial class MainWindow : Window
    {
        // Seluruh data disimpan sementara selama program berjalan.
        private readonly ObservableCollection<Mahasiswa> daftarMahasiswa = new();
        private Mahasiswa? sedangDiedit;

        public MainWindow()
        {
            InitializeComponent();
            TampilkanData();
            PerbaruiStatistik();
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";

            string nrp = txtNrp.Text.Trim();
            string nama = txtNama.Text.Trim();
            string prodi = txtProdi.Text.Trim();
            string inputIpk = txtIpk.Text.Trim().Replace(',', '.');

            // VALIDASI INPUT
            if (nrp.Length != 10 || nrp.Any(c => c < '0' || c > '9'))
            {
                txtError.Text = "NRP harus terdiri dari tepat 10 digit angka.";
                return;
            }

            if (daftarMahasiswa.Any(m => m.NRP == nrp && !ReferenceEquals(m, sedangDiedit)))
            {
                txtError.Text = "NRP sudah terdaftar. Gunakan NRP lain.";
                return;
            }

            if (string.IsNullOrWhiteSpace(nama))
            {
                txtError.Text = "Nama mahasiswa wajib diisi.";
                return;
            }

            if (string.IsNullOrWhiteSpace(prodi))
            {
                txtError.Text = "Program studi wajib diisi.";
                return;
            }

            if (!decimal.TryParse(inputIpk,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out decimal ipk)
                || ipk < 0 || ipk > 4)
            {
                txtError.Text = "IPK harus berupa angka antara 0 sampai 4 (contoh: 3.75).";
                return;
            }

            bool modeEdit = sedangDiedit != null;

            if (sedangDiedit == null)
            {
                daftarMahasiswa.Add(new Mahasiswa
                {
                    NRP = nrp,
                    Nama = nama,
                    Prodi = prodi,
                    IPK = ipk
                });
            }
            else
            {
                sedangDiedit.NRP = nrp;
                sedangDiedit.Nama = nama;
                sedangDiedit.Prodi = prodi;
                sedangDiedit.IPK = ipk;
            }

            BersihkanForm();
            TampilkanData();
            PerbaruiStatistik();
            txtStatus.Text = modeEdit ? "Data mahasiswa berhasil diperbarui." : "Mahasiswa berhasil ditambahkan.";
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gridMahasiswa.SelectedItem is Mahasiswa m)
                MulaiEdit(m);
        }

        private void GridMahasiswa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridMahasiswa.SelectedItem is Mahasiswa m)
                MulaiEdit(m);
        }

        private void MulaiEdit(Mahasiswa m)
        {
            sedangDiedit = m;
            txtNrp.Text = m.NRP;
            txtNama.Text = m.Nama;
            txtProdi.Text = m.Prodi;
            txtIpk.Text = m.IPK.ToString("0.00", CultureInfo.InvariantCulture);
            txtFormTitle.Text = "Edit Mahasiswa";
            btnSimpan.Content = "Simpan Perubahan";
            txtError.Text = "";
            txtStatus.Text = $"Sedang mengedit mahasiswa {m.NRP}.";
            txtNama.Focus();
        }

        private void BtnHapus_Click(object sender, RoutedEventArgs e)
        {
            if (gridMahasiswa.SelectedItem is not Mahasiswa m)
                return;

            MessageBoxResult jawaban = MessageBox.Show(
                $"Yakin ingin menghapus data {m.Nama} ({m.NRP})?",
                "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (jawaban != MessageBoxResult.Yes)
                return;

            daftarMahasiswa.Remove(m);
            BersihkanForm();
            TampilkanData();
            PerbaruiStatistik();
            txtStatus.Text = "Data mahasiswa berhasil dihapus.";
        }

        private void BtnBatal_Click(object sender, RoutedEventArgs e)
        {
            BersihkanForm();
            txtStatus.Text = "Form telah dibersihkan.";
        }

        private void BersihkanForm()
        {
            sedangDiedit = null;
            txtNrp.Clear();
            txtNama.Clear();
            txtProdi.Clear();
            txtIpk.Clear();
            txtError.Text = "";
            txtFormTitle.Text = "Tambah Mahasiswa";
            btnSimpan.Content = "+ Simpan Mahasiswa";
            gridMahasiswa.SelectedItem = null;
            PerbaruiTombol();
        }

        private void TxtCari_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (gridMahasiswa != null)
                TampilkanData();
        }

        private void TampilkanData()
        {
            string kataKunci = txtCari.Text.Trim();

            var hasil = daftarMahasiswa
                .Where(m =>
                    m.NRP.Contains(kataKunci, StringComparison.OrdinalIgnoreCase) ||
                    m.Nama.Contains(kataKunci, StringComparison.OrdinalIgnoreCase) ||
                    m.Prodi.Contains(kataKunci, StringComparison.OrdinalIgnoreCase))
                .ToList();

            gridMahasiswa.ItemsSource = hasil;
            PerbaruiTombol();
        }

        private void GridMahasiswa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PerbaruiTombol();
        }

        private void PerbaruiTombol()
        {
            bool dipilih = gridMahasiswa.SelectedItem is Mahasiswa;
            btnEdit.IsEnabled = dipilih;
            btnHapus.IsEnabled = dipilih;
        }

        private void PerbaruiStatistik()
        {
            int jumlah = daftarMahasiswa.Count;
            txtJumlah.Text = jumlah.ToString(CultureInfo.InvariantCulture);

            if (jumlah == 0)
            {
                txtRata.Text = "-";
                txtTertinggi.Text = "-";
                txtTerendah.Text = "-";
                return;
            }

            txtRata.Text = daftarMahasiswa.Average(m => m.IPK).ToString("0.00", CultureInfo.InvariantCulture);
            txtTertinggi.Text = daftarMahasiswa.Max(m => m.IPK).ToString("0.00", CultureInfo.InvariantCulture);
            txtTerendah.Text = daftarMahasiswa.Min(m => m.IPK).ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
