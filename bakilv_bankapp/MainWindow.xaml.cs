using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bakilv_bankapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private user felhasznalo;
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(user user)
        {
            InitializeComponent();

            felhasznalo = user;
            string nev = $" Üdv, {felhasznalo.name}";
            udvlabel.Content = nev;
            string penze = $"{felhasznalo.egyenleg:N0} Ft";
            egyenleglabel.Content = penze;
        }

        private void penzfeltoltes(object sender, RoutedEventArgs e)
        {
            double osszeg = double.Parse(osszeglabel.Text);
            felhasznalo.egyenleg += osszeg;
            string penze = $"{felhasznalo.egyenleg:N0} Ft";
            egyenleglabel.Content = penze;
            JsonFrissites.JsonFrissitese(Users.felhasznalok);
        }

        private void penzlevetel(object sender, RoutedEventArgs e)
        {
            double osszeg = double.Parse(osszeglabel.Text);
            felhasznalo.egyenleg -= osszeg;
            string penze = $"{felhasznalo.egyenleg:N0} Ft";
            egyenleglabel.Content = penze;
            JsonFrissites.JsonFrissitese(Users.felhasznalok);
        }
    }
}