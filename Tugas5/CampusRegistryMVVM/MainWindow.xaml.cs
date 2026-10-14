using System.Windows;
using CampusRegistryMVVM.Repositories;
using CampusRegistryMVVM.ViewModels;

namespace CampusRegistryMVVM;

public partial class MainWindow : Window
{
    private readonly MainViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = new MainViewModel(new MahasiswaRepository());
        DataContext = viewModel;
        Loaded += async (_, _) => await viewModel.LoadAsync();
    }
}
