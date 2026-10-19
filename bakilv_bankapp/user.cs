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
