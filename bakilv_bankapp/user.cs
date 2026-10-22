using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using MySql.Data.MySqlClient;

namespace bakilv_bankapp
{

    public static class sqllekerdezeesek
    {
        private static string connection = "server=localhost;user=root;password=;database=crackbank;Convert Zero Datetime=True;";

        public static void sqlkapcsolatesadatlehivas()
        {
            Users.felhasznalok.Clear();
            transaction.tranzakciok.Clear();

            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();


                using (var cmd = new MySqlCommand("SELECT * FROM felhasznalok", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string name = reader.GetString("name");
                        double egyenleg = reader.GetDouble("egyenleg");
                        string password = reader.GetString("password");

                        Users.felhasznalok.Add(new user(id, name, egyenleg, password));
                    }
                }


                using (var cmd2 = new MySqlCommand("SELECT * FROM tranzakciok", conn))
                using (var reader2 = cmd2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        int id = reader2.GetInt32("id");
                        int felhasznaloid = reader2.GetInt32("felhasznaloid");
                        double valtozas = reader2.GetDouble("valtozas");
                        double egyenleg = reader2.GetDouble("egyenleg");


                        DateTime date = DateTime.Now;
                        int dateOrdinal = reader2.GetOrdinal("date");

                        if (!reader2.IsDBNull(dateOrdinal))
                        {
                            try
                            {
                                date = reader2.GetDateTime(dateOrdinal);
                            }
                            catch
                            {
                                string dateStr = reader2.GetValue(dateOrdinal).ToString();
                                DateTime.TryParse(dateStr, out date);
                            }
                        }

                        transaction.tranzakciok.Add(new transaction(id, felhasznaloid, valtozas, date, egyenleg));
                    }
                }
            }
        }

        //public static void sqlkapcsolatfrissites()
        //{
        //    using (var conn = new MySqlConnection(connection))
        //    {
        //        conn.Open();


        //        using (var cmd = new MySqlCommand("DELETE FROM felhasznalok", conn))
        //        {
        //            cmd.ExecuteNonQuery();
        //        }

        //        foreach (var felhasznalo in Users.felhasznalok)
        //        {
        //            using (var cmd2 = new MySqlCommand("INSERT INTO felhasznalok (id, name, egyenleg, password) VALUES (@id, @name, @egyenleg, @password)", conn))
        //            {
        //                cmd2.Parameters.AddWithValue("@id", felhasznalo.id);
        //                cmd2.Parameters.AddWithValue("@name", felhasznalo.name);

        //                cmd2.Parameters.Add("@egyenleg", MySqlDbType.Double).Value = felhasznalo.egyenleg;

        //                cmd2.Parameters.AddWithValue("@password", felhasznalo.password);
        //                cmd2.ExecuteNonQuery();
        //            }
        //        }


        //        using (var cmd3 = new MySqlCommand("DELETE FROM tranzakciok", conn))
        //        {
        //            cmd3.ExecuteNonQuery();
        //        }

        //        foreach (var tranzakcio in transaction.tranzakciok)
        //        {
        //            using (var cmd4 = new MySqlCommand("INSERT INTO tranzakciok (id, felhasznaloid, valtozas, date, egyenleg) VALUES (@id, @felhasznaloid, @valtozas, @date, @egyenleg)", conn))
        //            {
        //                cmd4.Parameters.AddWithValue("@id", tranzakcio.id);
        //                cmd4.Parameters.AddWithValue("@felhasznaloid", tranzakcio.felhasznaloid);
        //                cmd4.Parameters.AddWithValue("@valtozas", tranzakcio.valtozas);
        //                cmd4.Parameters.AddWithValue("@date", tranzakcio.date);
        //                cmd4.Parameters.AddWithValue("@egyenleg", tranzakcio.egyenleg);
        //                cmd4.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //}
        public static void egyenlegfrissites(user felhasznalo)
        {
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("UPDATE felhasznalok SET egyenleg = @egyenleg WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@egyenleg", felhasznalo.egyenleg);
                    cmd.Parameters.AddWithValue("@id", felhasznalo.id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void ujfelhasznalohozzaadasa(user felhasznalo)
        {
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("INSERT INTO felhasznalok (id, name, egyenleg, password) VALUES (@id, @name, @egyenleg, @password)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", felhasznalo.id);
                    cmd.Parameters.AddWithValue("@name", felhasznalo.name);
                    cmd.Parameters.Add("@egyenleg", MySqlDbType.Double).Value = felhasznalo.egyenleg;
                    cmd.Parameters.AddWithValue("@password", felhasznalo.password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void ujtranzakciohozzaadasa(transaction tranzakcio)
        {
            using (var conn = new MySqlConnection(connection))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("INSERT INTO tranzakciok (id, felhasznaloid, valtozas, date, egyenleg) VALUES (@id, @felhasznaloid, @valtozas, @date, @egyenleg)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", tranzakcio.id);
                    cmd.Parameters.AddWithValue("@felhasznaloid", tranzakcio.felhasznaloid);
                    cmd.Parameters.AddWithValue("@valtozas", tranzakcio.valtozas);
                    cmd.Parameters.AddWithValue("@date", tranzakcio.date);
                    cmd.Parameters.AddWithValue("@egyenleg", tranzakcio.egyenleg);
                    cmd.ExecuteNonQuery();
                }
                
            }
        }
    }
        public static class JsonFrissites
    {
        public static void JsonFrissitese(List<user> felhasznalok)
        {
            string fajl = "C:\\Users\\bakilv\\source\\repos\\Viktorriusz\\bakilv_bankapp\\bakilv_bankapp\\felhasznalok.json";
            string json = JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fajl, json);
        }
        public static void JsonFrissiteseTransaction(List<transaction> tranzakciok)
        {
            string fajl = "C:\\Users\\bakilv\\source\\repos\\Viktorriusz\\bakilv_bankapp\\bakilv_bankapp\\tranzakciok.json";
            string json = JsonSerializer.Serialize(tranzakciok, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fajl, json);
        }
    }

    public class user
    {
        public int id { get; set; }
        public string name { get; set; } 
        public double egyenleg { get; set; }
        public string password { get; set; }


        public user(int id, string name, double egyenleg, string password)
        {
            this.id = id;
            this.name = name;
            this.egyenleg = egyenleg;
            this.password = password;
        }
    }

    public class transaction
    {
        public int id { get; set; }
        public int felhasznaloid { get; set; }
        public double valtozas { get; set; }
        public DateTime date { get; set; }
        
        public double egyenleg { get; set; }
        public transaction(int id, int felhasznaloid, double valtozas, DateTime date, double egyenleg)
        {
            this.id = id;
            this.felhasznaloid = felhasznaloid;
            this.valtozas = valtozas;
            this.date = date;
            this.egyenleg = egyenleg;
        }

        //public static List<transaction> JsonBeolvasasTransaction()
        //{
        //    string fajl = "C:\\Users\\bakilv\\source\\repos\\Viktorriusz\\bakilv_bankapp\\bakilv_bankapp\\tranzakciok.json";
        //    string json = File.ReadAllText(fajl);
        //    List<transaction> tranzakciok = JsonSerializer.Deserialize<List<transaction>>(json);
        //    return tranzakciok;
        //}
        public static List<transaction> tranzakciok = new List<transaction>();

       
    }
    public class utalas
    {
        public int id { get; set; }
        public int felhasznaloid1 { get; set; }
        public int felhasznaloid2 { get; set; }
        public double osszeg { get; set; }
        public DateTime datum { get; set; }
        public double felh1egyenleg { get; set; }

        public double felh2egyenleg { get; set; }

        public utalas(int id, int felhasznaloid1, int felhasznaloid2, double osszeg, DateTime datum, double felh1egyenleg, double felh2egyenleg)
        {
            this.id = id;
            this.felhasznaloid1 = felhasznaloid1;
            this.felhasznaloid2 = felhasznaloid2;
            this.osszeg = osszeg;
            this.datum = datum;
            this.felh1egyenleg = felh1egyenleg;
            this.felh2egyenleg = felh2egyenleg;
        }

        public static void utalaslekerdezes(List<utalas> utalasok)
        {
            var conn = new MySqlConnection("server=localhost;user=root;password=;database=crackbank;Convert Zero Datetime=True;");
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM utalasok", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        int felhasznaloid1 = reader.GetInt32("felhasznaloid1");
                        int felhasznaloid2 = reader.GetInt32("felhasznaloid2");
                        double osszeg = reader.GetDouble("osszeg");
                        DateTime datum = reader.GetDateTime("datum");
                        double felh1egyenleg = reader.GetDouble("felh1egyenleg");
                        double felh2egyenleg = reader.GetDouble("felh2egyenleg");
                        utalasok.Add(new utalas(id, felhasznaloid1, felhasznaloid2, osszeg, datum, felh1egyenleg, felh2egyenleg));
                    }
                }
                conn.Close();
            }
        }
        public static List<utalas> utalasok = new List<utalas>();

        public static void utalashozzadas(utalas utalas)
        {
            var conn = new MySqlConnection("server=localhost;user=root;password=;database=crackbank;Convert Zero Datetime=True;");
            {
                conn.Open();
                using (var cmd = new MySqlCommand("INSERT INTO utalasok (id, felhasznaloid1, felhasznaloid2, osszeg, datum,felh1egyenleg,felh2egyenleg) VALUES (@id, @felhasznaloid1, @felhasznaloid2, @osszeg, @datum,@felh1egyenleg,@felh2egyenleg)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", utalas.id);
                    cmd.Parameters.AddWithValue("@felhasznaloid1", utalas.felhasznaloid1);
                    cmd.Parameters.AddWithValue("@felhasznaloid2", utalas.felhasznaloid2);
                    cmd.Parameters.AddWithValue("@osszeg", utalas.osszeg);
                    cmd.Parameters.AddWithValue("@datum", utalas.datum);
                    cmd.Parameters.AddWithValue("@felh1egyenleg", utalas.felh1egyenleg);
                    cmd.Parameters.AddWithValue("@felh2egyenleg", utalas.felh2egyenleg);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void utalasfugveny(int felhasznaloid, int felhasznaloid2, double osszeg)
        {
            var felhasznalo1 = Users.felhasznalok.FirstOrDefault(f => f.id == felhasznaloid);
            var felhasznalo2 = Users.felhasznalok.FirstOrDefault(f => f.id == felhasznaloid2);
            if (felhasznalo1 != null && felhasznalo2 != null && felhasznalo1.egyenleg >= osszeg)
            {
                felhasznalo1.egyenleg -= osszeg;
                felhasznalo2.egyenleg += osszeg;
                transaction tranzakcio1 = new transaction(transaction.tranzakciok.Count + 1 , felhasznaloid, -osszeg, DateTime.Now, felhasznalo1.egyenleg);
                transaction.tranzakciok.Add(tranzakcio1);
                transaction tranzakcio2 = new transaction(transaction.tranzakciok.Count + 1, felhasznaloid2, osszeg, DateTime.Now, felhasznalo2.egyenleg);
                transaction.tranzakciok.Add(tranzakcio2);
                utalas utalas = new utalas(utalasok.Count , felhasznaloid, felhasznaloid2, osszeg, DateTime.Now, felhasznalo1.egyenleg, felhasznalo2.egyenleg);
                utalas.utalashozzadas(utalas);
                transaction.tranzakciok.Add(tranzakcio2);
                sqllekerdezeesek.ujtranzakciohozzaadasa(tranzakcio1);
                sqllekerdezeesek.ujtranzakciohozzaadasa(tranzakcio2);
                JsonFrissites.JsonFrissitese(Users.felhasznalok);
                sqllekerdezeesek.egyenlegfrissites(felhasznalo1);
                sqllekerdezeesek.egyenlegfrissites(felhasznalo2);

                MessageBox.Show($"Sikeres utalás! {osszeg} Ft átutalva {felhasznalo2.name} részére.");

            }
            else
            {
                MessageBox.Show("Sikertelen utalás! Ellenőrizze az egyenleget vagy a felhasználói azonosítókat.");
            }
        }
   

       
    }
        public class Users
        {
            //public static List<user> JsonBeolvasas()
            //{
            //    string fajl = "C:\\Users\\bakilv\\source\\repos\\Viktorriusz\\bakilv_bankapp\\bakilv_bankapp\\felhasznalok.json";

            //    string json = File.ReadAllText(fajl);

            //    List<user> felhasznalok = JsonSerializer.Deserialize<List<user>>(json);

            //    return felhasznalok;
            //}

            public static List<user> felhasznalok = new List<user>();

    }
 }
