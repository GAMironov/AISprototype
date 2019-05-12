using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PeopleDB_Server1.Controllers
{
    public class DataBaseController : Controller
    {
        // GET: DataBase
        public ActionResult Index()
        {
            return View();
        }

        SqlConnection sqlConnection;

        //<summary> 
        //Дешифровка команд
        //KRXR = EXECUTE GetAll; 
        //KRXZ = EXECUTE GetItem;
        //EQHZ = EXECUTE AddItem;
        //HRPV = EXECUTE DeleteItem;
        //YCHR = EXECUTE UpdateItem;
        //</summary>

        public string Encryption(string s)
        {
            char c = ' ';

            string input = s;
            int indexOfChar = s.IndexOf(c);
            s = s.Substring(0, indexOfChar);
            input = input.Substring(indexOfChar);

            switch (s)
            {
                case "KRXR":
                    input = "EXECUTE GetAll" + input;
                    break;
                case "KRXZ":
                    input = "EXECUTE GetItem" + input;
                    break;
                case "EQHZ":
                    input = "EXECUTE AddItem" + input;
                    break;
                case "HRPV":
                    input = "EXECUTE DeleteItem" + input;
                    break;
                case "YCHR":
                    input = "EXECUTE UpdateItem" + input;
                    break;
            }

            return input;
        }
        public async Task<string> CommandToDB(string s)
        {
            string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data");
            string fileName = "DB_Connection_path.txt";
            string absolutePathToFile = Path.Combine(appDataPath, fileName);

            FileStream fs1 = new FileStream(absolutePathToFile, FileMode.Open);
            StreamReader reader = new StreamReader(fs1);
            string connectionString = reader.ReadToEnd();
            reader.Close();

            sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            string sqlcommand = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<string>(s));

            sqlcommand = await Task.Factory.StartNew(() => Encryption(sqlcommand));

            SqlDataReader sqlReader1 = null;

            SqlCommand command1 = new SqlCommand(sqlcommand, sqlConnection);

            sqlReader1 = command1.ExecuteReader();

            string[] stringArray = new string[30];

            string db_data = null;

            while (sqlReader1.Read())
            {
                if (sqlReader1 != null)
                {
                    IDataRecord record = (IDataRecord)sqlReader1;
                    db_data = db_data + record["Id"].ToString() + " " + record["Name"].ToString() + " " + record["Family"].ToString() + " " + record["Phone"].ToString() + " " + record["Email"].ToString() + " " + "\n";
                }
            }

            sqlReader1.Close();
            sqlConnection.Close();

            string sqlresponse = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(db_data));

            return sqlresponse;
        }
    }
}