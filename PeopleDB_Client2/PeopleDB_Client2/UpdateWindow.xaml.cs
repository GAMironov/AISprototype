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
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        public List<Person> SearchPeople { get; set; }

        public bool f = false;

        public async Task<string> RequestAndResponsToDataBaseCommandToDB(string sqlcommand)
        {

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

        private async void button1_click(object sender, RoutedEventArgs e)
        {

            int n = 0;

            try
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

                string ID = null;
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
                    ID = record.Substring(0, indexOfChar);
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
                    p.ID = ID;
                    p.Name = name;
                    p.Family = family;
                    p.Phone = phone;
                    p.Email = email;

                    SearchPeople.Add(p);

                }
                f = true;
                DataContext = this;
            }
            catch
            {
                n++;
                MessageBox.Show("Ошибка!");
            }

            finally
            {
                if (f == true)
                    {
                    button2.Visibility = Visibility.Visible;
                    button3.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;
                    textBox4.Visibility = Visibility.Visible;
                    textBox5.Visibility = Visibility.Visible;
                    textBox6.Visibility = Visibility.Visible;
                }
            }

        }

        private async void button2_click(object sender, RoutedEventArgs e)
        {
            

            int k = 0;
            string sqlcommand = null;

            try
            {


                if ((textBox2.Text == "") && (textBox3.Text == "") && (textBox4.Text == ""))
                {
                    MessageBox.Show("Поля ID, Имя и Фамилия должны быть заполнены!");

                }
                else
                {
                    sqlcommand = "YCHR " + "'" + textBox2.Text + "', " +  "'" + textBox3.Text + "', " + "'" + textBox4.Text + "'";

                    if (textBox2.Text != "")
                    {
                        sqlcommand = sqlcommand + ", '" + textBox5.Text + "'";
                    }

                    if (textBox3.Text != "")
                    {
                        sqlcommand = sqlcommand + ", '" + textBox6.Text + "'";
                    }
                    sqlcommand = sqlcommand + ";";
                }


                string answerDeseriliaseSting = await RequestAndResponsToDataBaseCommandToDB(sqlcommand);
            }

            catch
            {
                k++;
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (k == 0)
                {
                    MessageBox.Show("Успешно!");
                    Close();
                }
            }
        }

        private async void button3_click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            string sqlcommand = null;

            try
            {


                if (textBox2.Text == "")
                {
                    MessageBox.Show("Поле ID должно быть заполнено!");

                }
                else
                {
                    sqlcommand = "HRPV " + "'" + textBox2.Text + "';";
                }


                string answerDeseriliaseSting = await RequestAndResponsToDataBaseCommandToDB(sqlcommand);
            }

            catch
            {
                k++;
                MessageBox.Show("Ошибка!");
            }
            finally
            {
                if (k == 0)
                {
                    MessageBox.Show("Успешно!");
                    Close();
                }
            }
        }
    }
}
