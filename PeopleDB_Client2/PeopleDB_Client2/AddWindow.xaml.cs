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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

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
            int k = 0;
            string sqlcommand = null;

            try
            {
                

                if ((textBox1.Text == "") && (textBox.Text == ""))
                {
                    MessageBox.Show("Поля Имя и Фамилия должны быть заполнены!");

                }
                else
                {
                     sqlcommand = "EQHZ " + "'" + textBox1.Text + "', " + "'" + textBox.Text + "'";

                    if (textBox2.Text != "")
                    {
                        sqlcommand = sqlcommand + ", '" + textBox2.Text + "'";
                    }

                    if (textBox3.Text != "")
                    {
                        sqlcommand = sqlcommand + ", '" + textBox3.Text + "'";
                    }
                    sqlcommand = sqlcommand + ";";
                }

               

                    //string sqlcommand = "EXECUTE AddItem " + "'" + textBox1.Text + "'," + "'" + textBox.Text + "'," + "'" + textBox2.Text + "'," + textBox3.Text + ";";

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
