using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleDB_Client2
{
    public class Person : INotifyPropertyChanged
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Person(string id, string n) { ID = id; Name = n; }
        public Person(string id, string f, string n) { ID = id; Name = n; Family = f; }
        public Person(string id, string f, string n, string p) { ID = id; Name = n; Family = f; Phone = p; }
        public Person(string id, string f, string n, string p, string e) { ID = id; Name = n; Family = f; Phone = p; Email = e; }

        public Person()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
