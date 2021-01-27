using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetFactoryEncryptHash
{
   
    public class User : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id;  }
            set {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged();
                }
                    }
        }
            
        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                if (value != login)
                {
                    login = value;
                    OnPropertyChanged();
                }
            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }
        private string address;

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (value != address)
                {
                    address = value;
                    OnPropertyChanged();
                }
            }
        }
        public string phone;

        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (value != phone)
                {
                    phone = value;
                    OnPropertyChanged();
                }
            }
        }
        private string role;
        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                if (value != role)
                {
                    role = value;
                    OnPropertyChanged();
                }
            }
        }

        public User(int id=1,string login = "No name", string password = "No password", string address = "No address", string phone = "No phone", string role = "No role")
        {
            Id = id;
            Login = login;
            Password = password;
            Address = address;
            Phone = phone;
            Role = role;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public override string ToString()
        {
            return $"{Id}\t\t{Login}\t\t{Role}";
        }
    }
}
