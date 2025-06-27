using System.Windows;
using WpfAppUserInterface.ModelData;
using WpfAppUserInterface.Classes;

namespace WpfAppUserInterface.Pages
{
    /// <summary>
    /// Логика взаимодействия для WindowAddEditWeighing.xaml
    /// </summary>
    public partial class WindowAddEditWeighing : Window
    {
        private readonly BridgeScalesContext _context;
        private Weighing _weighing { get; set; }
        private Scale _scaling { get; set; }
        private SensorData _sensor { get; set; }

        public WindowAddEditWeighing(BridgeScalesContext context)
        {
            _context = context;
            _weighing = new Weighing { WeighingDateTime = DateTime.Now };
            DataContext = this;
            InitializeComponent();
            LoadComboBoxData();
            _scaling = UpdateScaleData();
        }
        private Scale UpdateScaleData()
        {
            var sensorService = new FirebaseSensorService();
            _scaling = (Scale)ScaleComboBox.SelectedItem;
            var lastSensorsData = sensorService.GetAllSensorDataById(_scaling.ScaleId).OrderByDescending(x=> x.timestamp).FirstOrDefault();
            if (lastSensorsData != null)
            {
                GrossWeightTextBox.Text = lastSensorsData.weight.ToString();
                _sensor = lastSensorsData;
            }
            return _scaling;
        }
        private void LoadComboBoxData()
        {
            ScaleComboBox.ItemsSource = _context.Scales.ToList();
            ScaleComboBox.SelectedIndex = 0;
            ClientComboBox.ItemsSource = _context.Clients.ToList();
            ClientComboBox.SelectedIndex = 0;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_sensor == null)
            {
                MessageBox.Show("Весы не выбраны или отсутвуют данные!");
                return;
            }
            if (String.IsNullOrWhiteSpace(VehicleNumberTextBox.Text) && String.IsNullOrWhiteSpace(TareWeightTextBox.Text))
            {
                MessageBox.Show("Заполните данные во всех полях!");
                return;
            }

            _weighing.TareWeight = Convert.ToInt32(TareWeightTextBox.Text);
            _weighing.VehicleNumber = VehicleNumberTextBox.Text;
            _weighing.ClientId = (int)ClientComboBox.SelectedValue;
            _weighing.ScaleId = (int)ScaleComboBox.SelectedValue;
            _weighing.GrossWeight = _sensor.weight;
            _weighing.OperatorId = WindowAutorization.EnterUser!.UserId;
            _weighing.Notes = NotesTextBox.Text;

            try
            {
                _context.Weighings.Add(_weighing);
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _scaling = UpdateScaleData();
        }

        private void ScaleComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _scaling = UpdateScaleData();
        }
    }
}
