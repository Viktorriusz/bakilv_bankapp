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
            udvlabel.Text = nev;
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
            transaction.tranzakciok.Add(new transaction(transaction.tranzakciok.Count, felhasznalo.id, osszegdouble, DateTime.Now, felhasznalo.egyenleg));
            sqllekerdezeesek.ujtranzakciohozzaadasa(transaction.tranzakciok.Last());
            JsonFrissites.JsonFrissiteseTransaction(transaction.tranzakciok);
            sqllekerdezeesek.egyenlegfrissites(felhasznalo);
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
            transaction.tranzakciok.Add(new transaction(transaction.tranzakciok.Count, felhasznalo.id, -osszegdouble, DateTime.Now, felhasznalo.egyenleg));
            sqllekerdezeesek.ujtranzakciohozzaadasa(transaction.tranzakciok.Last());
            JsonFrissites.JsonFrissiteseTransaction(transaction.tranzakciok);
            sqllekerdezeesek.egyenlegfrissites(felhasznalo);
            logokfrissitese();
        }
        

        private void logokfrissitese()
        {
            tranzakciolog.Items.Clear();

            foreach (var elem in transaction.tranzakciok)
            {
                if (elem != null && elem.felhasznaloid == felhasznalo.id)
                {
                    tranzakciolog.Items.Add($"Változás: {elem.valtozas:N0} Ft | Dátum: {elem.date.ToString()} | Egyenleg: {elem.egyenleg:N0} Ft");
                }
            }
        }

        private void penzutalas(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Negatív összeget nem utalhatsz!");
                return;
            }
            if (osszegdouble == 0)
            {
                MessageBox.Show("0 Ft-ot nem utalhatsz!");
                return;
            }
            int felhasznalo2id = int.Parse(szamlaszam.Text);
            foreach (user u in Users.felhasznalok)
            {
                if (u.id == felhasznalo2id)
                {
                    felhasznalo2id = u.id;
                    break;
                }
            }
            user felhasznalo2 = Users.felhasznalok.Find(x => x.id == felhasznalo2id);
            if (felhasznalo2 == null)
            {
                MessageBox.Show("Nincs ilyen felhasználó!");
                return;
            }
            if (felhasznalo2.id == felhasznalo.id)
            {
                MessageBox.Show("Saját magadnak nem utalhatsz!");
                return;
            }
            utalas utalas = new utalas(utalas.utalasok.Count,felhasznalo.id, felhasznalo2.id, osszegdouble, DateTime.Now, felhasznalo.egyenleg, felhasznalo2.egyenleg);
            utalas.utalasok.Add(utalas);
            utalas.utalasfugveny(felhasznalo.id, felhasznalo2.id, osszegdouble);
            egyenleglabel.Content = $"{felhasznalo.egyenleg:N0} Ft";






        }
    }
}