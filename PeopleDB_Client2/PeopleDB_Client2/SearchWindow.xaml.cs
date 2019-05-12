using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace PeopleDB_Client2
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        public List<Person> SearchPeople { get; set; }

        public async Task<string> RequestAndResponsToDataBaseCommandToDB(string sqlcommand)
        {
            //string appDataPath = @"~App_Data/";
            //string fileName = "Server_Connection_path.txt";
            //string PathToFile = String.Concat(appDataPath, fileName);

            FileStream fs1 = new FileStream("Server_Connection_path.txt", FileMode.Open);
            StreamReader reader = new StreamReader(fs1);
            string connectionString = reader.ReadToEnd();
            reader.Close();

            string JSONData = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(sqlcommand));

            WebRequest request = WebRequest.Create(connectionString + "/DataBase/CommandToDB");

            request.Method = "POST";

            string query = $"s={JSONData}";

            byte[] byteMsg = Encoding.UTF8.GetBytes(query);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteMsg.Length;

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(byteMsg, 0, byteMsg.Length);
            }

            WebResponse response = await request.GetResponseAsync();

            string answer = null;

            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader sR = new StreamReader(s))
                {
                    answer = await sR.ReadToEndAsync();
                }
            }

            response.Close();

            string answerDeseriliaseStingReturn = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<string>(answer));

            return answerDeseriliaseStingReturn;
        }


        private async void  button1_Click(object sender, RoutedEventArgs e)
        {

            SearchPeople = new List<Person>();

            string sqlcommand = "KRXZ " + textBox1.Text;

            string answerDeseriliaseSting = await RequestAndResponsToDataBaseCommandToDB(sqlcommand);

            int k = 0;

            IEnumerable<char> evens = from c in answerDeseriliaseSting
                                      where c == '\n'
                                      select 'm';

            foreach (char c in evens)
            {
                k++;
            }

            string name = null;
            string family = null;
            string phone = null;
            string email = null;


            for (int i = 1; i <= k; i++)
            {
                char c = '\n';
                int indexOfChar = answerDeseriliaseSting.IndexOf(c);
                string record = answerDeseriliaseSting.Substring(0, indexOfChar);
                answerDeseriliaseSting = answerDeseriliaseSting.Substring(indexOfChar + 1);

                c = ' ';
                indexOfChar = record.IndexOf(c);
                record = record.Substring(indexOfChar + 1);

                indexOfChar = record.IndexOf(c);
                name = record.Substring(0, indexOfChar);
                record = record.Substring(indexOfChar + 1);


                indexOfChar = record.IndexOf(c);
                family = record.Substring(0, indexOfChar);
                record = record.Substring(indexOfChar + 1);

                indexOfChar = record.IndexOf(c);
                phone = record.Substring(0, indexOfChar);
                record = record.Substring(indexOfChar + 1);

                indexOfChar = record.IndexOf(c);
                email = record.Substring(0, indexOfChar);
                record = record.Substring(indexOfChar + 1);

                Person p = new Person();
                p.Name = name;
                p.Family = family;
                p.Phone = phone;
                p.Email = email;

                SearchPeople.Add(p);

            }

            DataContext = this;
        }
    }
}
