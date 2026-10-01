using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace bakilv_bankapp
{
    static class JsonFrissites
    {
        public static void JsonFrissitese(List<user> felhasznalok)
        {
            string fajl = "C:\\Users\\bakilv\\source\\repos\\bakilv_bankapp\\bakilv_bankapp\\felhasznalok.json";
            string json = JsonSerializer.Serialize(felhasznalok, new JsonSerializerOptions { WriteIndented = true });
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
        public int id {  get; set; }
        public int felhasznaloid { get; set; }
        public double valtozas { get; set; }
        public DateTime date;
        public transaction(int id, int userId, double valtozas, DateTime date)
        {
            this.id = id;
            this.felhasznaloid = userId;
            this.valtozas = valtozas;
            this.date = date;
        }
    }
    public class Users
    {
        public static List<user> JsonBeolvasas()
        {
            string fajl = "C:\\Users\\Viktor\\source\\repos\\Viktorriusz\\bakilv_bankapp\\bakilv_bankapp\\felhasznalok.json";

            string json = File.ReadAllText(fajl);

            List<user> felhasznalok = JsonSerializer.Deserialize<List<user>>(json);

            return felhasznalok;
        }

        public static List<user> felhasznalok = JsonBeolvasas();


    }
   }
