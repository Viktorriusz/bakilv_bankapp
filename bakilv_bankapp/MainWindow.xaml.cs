using System.Diagnostics;
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
            logokfrissitese();
        }

        private void penzfeltoltes(object sender, RoutedEventArgs e)
        {
            var osszeg = osszeglabel.Text;
            if (!double.TryParse(osszeg, out double osszegdouble))
            {
                MessageBox.Show("Kérlek, számot adj meg!");
                return;
            }
            if (osszegdouble < 0)
            {
                MessageBox.Show("Negatív összeget nem tölthetsz fel!");
                return;
            }
            if (osszegdouble == 0)
            {
                MessageBox.Show("0 Ft-ot nem tölthetsz fel!");
                return;
            }
            felhasznalo.egyenleg += osszegdouble;
            string penze = $"{felhasznalo.egyenleg:N0} Ft";
            egyenleglabel.Content = penze;
            JsonFrissites.JsonFrissitese(Users.felhasznalok);
            transaction.tranzakciok.Add(new transaction(transaction.tranzakciok.Count + 1, felhasznalo.id, osszegdouble, DateTime.Now.ToString(), felhasznalo.egyenleg));
            JsonFrissites.JsonFrissiteseTransaction(transaction.tranzakciok);
            logokfrissitese();
        }

        private void penzlevetel(object sender, RoutedEventArgs e)
        {
            string osszeg = osszeglabel.Text;
            if (!double.TryParse(osszeg, out double osszegdouble))
            {
                MessageBox.Show("Kérlek, számot adj meg!");
                return;
            }
            if (felhasznalo.egyenleg < osszegdouble)
            {
                MessageBox.Show("Nincs elég pénz az egyenlegeden!");
                return;
            }
            if (osszegdouble < 0)
            {
                MessageBox.Show("Negatív összeget nem vehetsz le!");
                return;
            }
            if (osszegdouble == 0)
            {
                MessageBox.Show("0 Ft-ot nem vehetsz le!");
                return;
            }
            felhasznalo.egyenleg -= osszegdouble;
            string penze = $"{felhasznalo.egyenleg:N0} Ft";
            egyenleglabel.Content = penze;
            JsonFrissites.JsonFrissitese(Users.felhasznalok);
            transaction.tranzakciok.Add(new transaction(transaction.tranzakciok.Count + 1, felhasznalo.id, -osszegdouble, DateTime.Now.ToString(), felhasznalo.egyenleg));
            JsonFrissites.JsonFrissiteseTransaction(transaction.tranzakciok);
            logokfrissitese();
        }

        private void logokfrissitese()
        {
            transaction.JsonBeolvasasTransaction();
            tranzakciolog.Items.Clear();

            foreach (var elem in transaction.tranzakciok)
            {
                if (elem != null && elem.felhasznaloid == felhasznalo.id)
                {
                    tranzakciolog.Items.Add($"Változás: {elem.valtozas:N0} Ft | Dátum: {elem.date} | Egyenleg: {elem.egyenleg:N0} Ft");
                }
            }
        }
    }
}