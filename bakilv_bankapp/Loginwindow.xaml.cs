using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace bakilv_bankapp
{
    /// <summary>
    /// Interaction logic for Loginwindow.xaml
    /// </summary>
    public partial class Loginwindow : Window
    {
        public bool sikeres = false;
        public void  bejelentkezesellenorzes(string nev, string jelszo)
        { 
            for (int i = 0; i < Users.felhasznalok.Count; i++)
            {
                if (Users.felhasznalok[i].name == nev && Users.felhasznalok[i].password == jelszo)
                {
                    user user = Users.felhasznalok[i];

                    MessageBox.Show("Sikeres bejelentkezés!");

                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    sikeres = true;
                    return;
                }
                else
                    {
                    MessageBox.Show("Hibás felhasználónév vagy jelszó!");
                    continue;
                }
            }
        }

        public Loginwindow()
            {
                InitializeComponent();
            }

            private void LoginButton_Click(object sender, RoutedEventArgs e)
            {
                string name = NameBox.Text;
                string password = PasswordBox.Password;
                byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(password));
                bejelentkezesellenorzes(name, Convert.ToBase64String(hash));
                if (sikeres)
                {
                    this.Close();
                }
        }

    }
}
