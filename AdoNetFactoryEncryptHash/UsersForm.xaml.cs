using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace AdoNetFactoryEncryptHash
{
    
    public partial class Window1 : Window,INotifyPropertyChanged
    {

        private bool isEnabled;
        public bool IsEnabledLines
        {
            get { return isEnabled; }
            set { if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                } }
        }
        private User user;
        public User User
        {
            get { return user; }
            set
            {
                if (value != user)
                    user = value;
                OnPropertyChanged();
            }
        }
        public Window1(User user,bool isEnabled)
        {

            InitializeComponent();
            DataContext = this;
            this.user = user;
            IsEnabledLines = isEnabled;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        
    }
}
