using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WpfAppUserInterface.ModelData;

namespace WpfAppUserInterface.Pages;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class WindowAutorization : Window
{
    private BridgeScalesContext _context;
    public static User? EnterUser { get; set; }
    public WindowAutorization()
    {
        InitializeComponent();
        _context = new BridgeScalesContext();
        if (Properties.Settings.Default.RememberMe)
        {
            RememberCheckBox.IsChecked = true;
            UsernameTextBox.Text = Properties.Settings.Default.UserName;
            PasswordBox.Password = "Password";
        }
    }
    public User? FindNewUser(string username, string password)
    {
        // Хеширование пароля (рекомендуется использовать более безопасные методы)
        var hashedPassword = HashPassword(password);

        var user = _context.Users
            .FirstOrDefault(u => u.Username == username && u.Password == hashedPassword && u.IsActive == true);

        return user;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    public bool CheckRememberedUser(out int userid)
    {
        userid = Properties.Settings.Default.UserID;
        return Properties.Settings.Default.RememberMe && userid != -1;
    }
    private void Autorization(User? user, bool rememberMe)
    {
        if (user != null && rememberMe)
        {
            // Сохраняем данные для "запомнить меня" (в реальном приложении используйте безопасное хранилище)
            Properties.Settings.Default.RememberMe = true;
            Properties.Settings.Default.UserID = user.UserId;
            Properties.Settings.Default.UserName = user.Username;
            Properties.Settings.Default.Save();
        }
        else if (!rememberMe)
        {
            // Очищаем сохраненные данные, если "запомнить меня" не отмечен
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.UserID = -1;
            Properties.Settings.Default.UserName = "";
            Properties.Settings.Default.Save();
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
        }

        if (EnterUser != null)
        {
            switch (EnterUser.Role)
            {
                case "Operator":
                    WindowWeighings window = new WindowWeighings(_context);
                    window.Show();
                    Hide();
                    return;
            }
        }
        MessageBox.Show("Роль не найдена или не добавлен функционал для неё!");
    }
    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        if (Properties.Settings.Default.UserID == -1)
        {
            EnterUser = FindNewUser(UsernameTextBox.Text, PasswordBox.Password);
            Autorization(EnterUser, (bool)RememberCheckBox.IsChecked!);
        }
        else
        {
            EnterUser = _context.Users.FirstOrDefault(x => x.UserId == Properties.Settings.Default.UserID);
            Autorization(EnterUser, (bool)RememberCheckBox.IsChecked!);
        }
    }
}