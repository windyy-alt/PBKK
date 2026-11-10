
using System;
using System.Threading.Tasks;
using System.Windows;
using StudentRegistration.Repositories;

namespace StudentRegistration
{
    public partial class MainWindow : Window
    {
        private readonly StudentRepository _studentRepository
            = new StudentRepository();

        public MainWindow()
        {
            InitializeComponent();

            // Memuat data saat aplikasi dibuka
            Loaded += MainWindow_Loaded;

            // Mengaktifkan tombol Refresh
            RefreshButton.Click += RefreshButton_Click;
        }

        // Saat jendela pertama kali dibuka
        private async void MainWindow_Loaded(
            object sender, RoutedEventArgs e)
        {
            await LoadStudentsAsync();
        }

        // Saat tombol Refresh ditekan
        private async void RefreshButton_Click(
            object sender, RoutedEventArgs e)
        {
            await LoadStudentsAsync();
        }

        // Mengambil data mahasiswa dari SQL Server
        private async Task LoadStudentsAsync()
        {
            RefreshButton.IsEnabled = false;
            RefreshButton.Content = "Memuat data...";
            StatusText.Text = "Menghubungkan...";

            try
            {
                // Ambil daftar dan jumlah mahasiswa
                var result = await Task.Run(() =>
                {
                    var students = _studentRepository.GetAll();
                    var total = _studentRepository.GetTotal();

                    return (students, total);
                });

                // Tampilkan data ke tabel
                StudentDataGrid.ItemsSource = result.students;

                // Perbarui jumlah mahasiswa
                TotalText.Text = result.total.ToString();

                // Perbarui status database
                StatusText.Text = "Connected";

                // Perbarui waktu terakhir refresh
                LastUpdatedText.Text =
                    "Terakhir diperbarui: " +
                    DateTime.Now.ToString("dd MMM yyyy, HH:mm:ss");
            }
            catch (Exception ex)
            {
                StatusText.Text = "Koneksi gagal";

                MessageBox.Show(
                    "Gagal mengambil data dari SQL Server:\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                RefreshButton.IsEnabled = true;
                RefreshButton.Content = "↻  Refresh Data";
            }
        }
    }
}
