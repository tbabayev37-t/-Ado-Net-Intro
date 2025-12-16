using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaMizzaMenu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection("Server=LENOVO\\SQLEXPRESS;Database=PizzaMizzaMenu;Trusted_connection=true;TrustServerCertificate=true");
            connection.Open();
            start:
            Console.WriteLine("----------------Xos gelmisiniz----------------");
            Console.WriteLine("1.Pizzalara bax");
            Console.WriteLine("2.Pizza elave et");
            Console.WriteLine("3.Pizza sil");
            string input = Console.ReadLine();
            Console.Clear();
            switch(input)
            {
                case "1":
                    PrintAllProducts(connection);
                    break;
                case "2":
                    Label:
                    Console.Write("Zehmet olmasa pizza adini daxil edin: ");
                    string name = Console.ReadLine();
                    if(name == null || name.Length < 3)
                    {
                        Console.WriteLine("Zehmet olmasa duzgun deyer daxile edin!");
                        goto Label;
                    }
                    priceInput:
                    Console.Write("Zehmet olmasa qiymeti daxil edin: ");
                    string priceInput = Console.ReadLine();
                    var isParsed = decimal.TryParse(priceInput, out decimal price);
                    if (!isParsed||price<0)
                    {
                        Console.WriteLine("Zehmet olmasa duzgun deyer daxile edin!");
                        goto priceInput;
                    }
                    SqlCommand insertCommand = new SqlCommand($"insert into Products values('{name}',{price})", connection);
                    var insertResult = insertCommand.ExecuteNonQuery();
                    if (insertResult > 0)
                    {
                        Console.WriteLine("Mehsul ugurla elave edildi");
                    }
                    break;
                case "3":
                    PrintAllProducts(connection);
                    deleteId:
                    Console.Write("Silmek istediyiniz pizzanin Id-ni daxil edin: ");
                    int idInput = int.Parse(Console.ReadLine());
                    if (idInput <= 0)
                    {
                        Console.WriteLine("Zehmet olmasa duzgun deyer daxile edin!");
                        goto deleteId;
                    }
                    SqlCommand deleteCommand = new SqlCommand($"delete from Products where Id={idInput}",connection);
                    var deleteResult = deleteCommand.ExecuteNonQuery();
                    if (deleteResult == 0)
                    {
                        Console.WriteLine("Daxil etdiyiniz Id-de pizza tapilmadi");
                    }
                    else
                    {
                        Console.WriteLine("Pizza ugurla silindi");
                    }
                    break;

            }
            goto start;

        }

        private static void PrintAllProducts(SqlConnection connection)
        {
            SqlCommand getAllCommand = new SqlCommand("select *from Products", connection);
            SqlDataAdapter getAllAdapter = new SqlDataAdapter(getAllCommand);
            DataSet getAllDataSet = new DataSet();
            getAllAdapter.Fill(getAllDataSet);

            foreach (DataRow row in getAllDataSet.Tables[0].Rows)
            {
                Console.WriteLine($"{row["Id"]}. {row["Name"]} {row["Price"]} AZN");
            }
        }
    }
}
