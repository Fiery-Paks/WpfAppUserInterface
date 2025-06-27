using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace WpfAppUserInterface.Classes
{
    public class FirebaseSensorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseUrl;
        private readonly string _firebaseSecret;
        private readonly string _databasePath;

        public FirebaseSensorService(
            string firebaseUrl = "https://arduinodb-b6b48-default-rtdb.europe-west1.firebasedatabase.app",
            string firebaseSecret = "7AXsBXykKQeKf03UdJIqofv7fVbVEqgYFZ9wFtuY",
            string databasePath = "sensorData")
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            _firebaseUrl = firebaseUrl;
            _firebaseSecret = firebaseSecret;
            _databasePath = databasePath;
        }

        public List<SensorData> GetAllSensorDataById(int Id)
        {
            try
            {
                string fullUrl = $"{_firebaseUrl}/{_databasePath}/idScales{Id}.json?auth={_firebaseSecret}";

                HttpResponseMessage response = _httpClient.GetAsync(fullUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrEmpty(responseBody) && responseBody != "null")
                    {
                        using JsonDocument doc = JsonDocument.Parse(responseBody);
                        JsonElement root = doc.RootElement;

                        var sensorDataList = root.EnumerateObject()
                            .Select(prop => JsonSerializer.Deserialize<SensorData>(prop.Value.GetRawText()))
                            .ToList();

                        return sensorDataList!;
                    }
                }
                else
                {
                    MessageBox.Show($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.Flatten().InnerExceptions)
                {
                    MessageBox.Show($"Исключение: {e.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Общее исключение: {ex.Message}");
            }
            return null;
        }
    }
}