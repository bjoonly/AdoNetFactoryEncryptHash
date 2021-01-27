using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdoNetFactoryEncryptHash
{
    class ViewModel: INotifyPropertyChanged
    {
        private Command fillCommand;
        private Command addCommand;
        private Command removeCommand;
        private Command showCommand;
        public ICommand ShowCommand => showCommand;

        public ICommand AddCommand => addCommand;
        public ICommand FillCommand => fillCommand;
        public ICommand RemoveCommand => removeCommand;

        private string commandText;
        public string CommandText
        {
            get { return commandText; }
            set
            {
                if (value != commandText)
                {
                    commandText = value;
                    OnPropertyChanged();
                }
            }
        }


        private DbProviderFactory fact = null;

        private DbConnection conn = null;

        private DbDataAdapter da = null;
        private DataSet set = null;
        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                if (value != selectedUser)
                {
                    if (value == null)
                        selectedUser = new User();
                    selectedUser = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public void Show()
        {
            if (selectedUser != null)
            {
                Window1 w = new Window1(SelectedUser, false);
                w.ShowDialog();
            }
        }
      
        private readonly ICollection<User> users = new ObservableCollection<User>();

        public IEnumerable<User> Users => users;


        private void Remove()
        {
            try {
                if (selectedUser != null)
                {
                    da = fact.CreateDataAdapter();
              
                                        
                    da.SelectCommand = conn.CreateCommand();
                    da.SelectCommand.CommandText = $"delete from Users from Users where Id = @Id";
                    var param = da.SelectCommand.CreateParameter();

                    param.ParameterName = "@Id";
                    param.DbType = DbType.Int32;
                    param.Value = selectedUser.Id;
                    da.SelectCommand.Parameters.Add(param);
                    da.Fill(set);
                    User u=null;
                    foreach (var item in users)
                    {
                        if (item.Id == selectedUser.Id)
                        {
                            u = item;
                            break;
                        }
                    }
                    users.Remove(u);
                    
                   
              
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            }

        private void Fill()
        {
            try
            {
                

                if (commandText == String.Empty)
                    return;
                da = fact.CreateDataAdapter();
                da.SelectCommand = conn.CreateCommand();
                da.SelectCommand.CommandText = commandText;
                var builder = fact.CreateCommandBuilder();
                set = new DataSet();
                da.Fill(set);
                users.Clear();
                DbDataReader dbDataReader = set.CreateDataReader();
                while (dbDataReader.Read())
                {
                    users.Add(new User(dbDataReader.GetInt32(0), dbDataReader.GetString(1), dbDataReader.GetString(2), dbDataReader.GetString(3), dbDataReader.GetString(4), dbDataReader.GetString(5)));

                } 
                selectedUser = null;
          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Add()
        {
            try
            {
                User newUser = new User();
                Window1 w = new Window1(newUser,true); 
            
            

                if (w.ShowDialog()==true )
                {
                    newUser.Password = ComputeSha256Hash(newUser.Password);


                    da = fact.CreateDataAdapter();
                    da.SelectCommand = conn.CreateCommand();

                    da.SelectCommand.CommandText = $"Insert into Users(Login, Password, Address, Phone, Role) values(@Login,@Password,@Address,@Phone,@Role)";


                    var param1 = da.SelectCommand.CreateParameter();
                    var param2 = da.SelectCommand.CreateParameter();
                    var param3 = da.SelectCommand.CreateParameter();
                    var param4 = da.SelectCommand.CreateParameter();
                    var param5 = da.SelectCommand.CreateParameter();

                    param1.ParameterName = "@Login";
                    param1.DbType = DbType.String;
                    param1.Value = newUser.Login;
                    da.SelectCommand.Parameters.Add(param1);


                    param2.ParameterName = "@Password";
                    param2.DbType = DbType.String;
                    param2.Value = newUser.Password;


                    da.SelectCommand.Parameters.Add(param2);

                    param3.ParameterName = "@Address";
                    param3.DbType = DbType.String;
                    param3.Value = newUser.Address;

                    da.SelectCommand.Parameters.Add(param3);


                    param4.ParameterName = "@Phone";
                    param4.DbType = DbType.String;
                    param4.Value = newUser.Phone;
                    da.SelectCommand.Parameters.Add(param4);


                    param5.ParameterName = "@Role";
                    param5.DbType = DbType.String;
                    param5.Value = newUser.Role;
                    da.SelectCommand.Parameters.Add(param5);

                    var builder = fact.CreateCommandBuilder();
                    set = new DataSet();
                    da.Fill(set);
          
               
                    users.Add(newUser);





                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        static string ComputeSha256Hash(string rawData)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private static void EncryptConnSettings(string section)
        {

            Configuration objConfig = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
            ConnectionStringsSection conSringSection = (ConnectionStringsSection)objConfig.GetSection(section);



            if (!conSringSection.SectionInformation.IsProtected)
            {
                conSringSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                conSringSection.SectionInformation.ForceSave = true;
                objConfig.Save(ConfigurationSaveMode.Modified);
            }
            
        }
        public ViewModel()
        {
     

            
            string cs = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                fillCommand = new DelegateCommand(Fill);
               addCommand = new DelegateCommand(Add);
            removeCommand = new DelegateCommand(Remove);
            showCommand = new DelegateCommand(Show);
                fact = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["connStr"].ProviderName);
            selectedUser = new User();
            conn = fact.CreateConnection();
            conn.ConnectionString = cs;
            EncryptConnSettings("connectionStrings");
            PropertyChanged+=(sender,args)=>{if (args.PropertyName == nameof(SelectedUser))
            {
                removeCommand.RaiseCanExecuteChanged();
                addCommand.RaiseCanExecuteChanged();
            }
        };

    }
    }
}

