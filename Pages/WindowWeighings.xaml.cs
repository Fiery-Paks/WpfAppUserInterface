using System.Windows;
using Microsoft.EntityFrameworkCore;
using WpfAppUserInterface.ModelData;

namespace WpfAppUserInterface.Pages
{
    /// <summary>
    /// Логика взаимодействия для WindowWeighings.xaml
    /// </summary>
    public partial class WindowWeighings : Window
    {
        private readonly BridgeScalesContext _context;
        public WindowWeighings(BridgeScalesContext context)
        {
            InitializeComponent();
            _context = context;
            LoadWeighings();
        }
        private void LoadWeighings()
        {
            try
            {
                _context.Weighings
                    .Include(w => w.Client)
                    .Include(w => w.Operator)
                    .Include(w => w.Scale)
                    .Load();

                WeighingsDataGrid.ItemsSource = _context.Weighings.Local.ToObservableCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWeighings();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new WindowAddEditWeighing(_context);
            if (addWindow.ShowDialog() == true)
            {
                LoadWeighings();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<WindowAutorization>().FirstOrDefault()!.Visibility = Visibility.Visible;
            Close();
        }
    }
}
