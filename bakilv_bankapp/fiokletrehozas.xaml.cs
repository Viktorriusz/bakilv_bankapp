using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Xml.Linq;

namespace bakilv_bankapp
{
    /// <summary>
    /// Interaction logic for fiokletrehozas.xaml
    /// </summary>
    public partial class fiokletrehozas : Window
    {
        public fiokletrehozas()
        {
            InitializeComponent();
        }

        private void regisztracioclick(object sender, RoutedEventArgs e)
        {
            string felhasznalonev = nevmezo.Text;
            string jelszo = jelszomezo.Password;
            for (int i = 0; i < Users.felhasznalok.Count; i++)
            {
                if (Users.felhasznalok[i].name == felhasznalonev)
                {
                    MessageBox.Show("Már van ilyen felhasználónév!");
                }
            }
            if (string.IsNullOrWhiteSpace(felhasznalonev) || string.IsNullOrWhiteSpace(jelszo))
            {
                MessageBox.Show("Kérlek, töltsd ki az összes mezőt!");
                return;
            }
            if(felhasznalonev.Length < 3 || jelszo.Length < 3)
            {
                MessageBox.Show("A felhasználónévnek és a jelszónak legalább 3 karakter hosszúnak kell lennie!");
                return;
            }
            else
            {
                byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(jelszo));
                Users.felhasznalok.Add(new user(Users.felhasznalok.Count + 1, felhasznalonev, 0, Convert.ToBase64String(hash)));
                sqllekerdezeesek.ujfelhasznalohozzaadasa(new user(Users.felhasznalok.Count , felhasznalonev, 0, Convert.ToBase64String(hash)));
                MessageBox.Show("Sikeres regisztráció!");
                Loginwindow visszaloginba = new Loginwindow();
                visszaloginba.Show();
                this.Close();
            }

        }
    }
}
