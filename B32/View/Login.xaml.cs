using B32.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace B32
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MyContext myContext = new MyContext();

        public Login()
        {
            InitializeComponent();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Close();
        }

        private void GridBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TxtEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEmail.Text == "")
            {
                MessageBox.Show("Please, fill this column email!");
            }
            else if (TxtPw.Password == "")
            {
                MessageBox.Show("Please, fill this column password!");
            }
            else
            {
                try
                {
                    var roleId = myContext.Roles.ToList();
                    var userid = myContext.Users.FirstOrDefault(u => u.Email == TxtEmail.Text);
                    if (userid != null)
                    {
                        if (TxtEmail.Text == userid.Email && TxtPw.Password == userid.Password)
                        {
                            if (userid.Role.Id == 1)
                            {
                                MessageBox.Show("Log in successfully!");
                                DashboardSuperAdmin dashboardSuperAdmin = new DashboardSuperAdmin();
                                dashboardSuperAdmin.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Log in successfully!");
                                DashboardAdmin dashboardAdmin = new DashboardAdmin();
                                dashboardAdmin.Show();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your Email or Password is wrong!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, Call SuperAdmin for register in this system!");
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
            GridForgot.Visibility = Visibility.Hidden;
        }

        private void BtnForget_Click(object sender, RoutedEventArgs e)
        {
            GridForgot.Visibility = Visibility.Visible;
            GridLogin.Visibility = Visibility.Hidden;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GridForgot.Visibility = Visibility.Hidden;
            GridLogin.Visibility = Visibility.Visible;
        }

        private void BtnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEmailForget.Text == "")
            {
                MessageBox.Show("Please, fill column email please", "Alert", MessageBoxButton.OK);
                TxtEmail.Focus();
            }
            else
            {
                try
                {
                    var userid = myContext.Users.FirstOrDefault(u => u.Email == TxtEmailForget.Text);
                    string password = Guid.NewGuid().ToString();
                    userid.ChangePassword = userid.Password;
                    userid.Password = password;
                    myContext.SaveChanges();
                    Outlook._Application _app = new Outlook.Application();
                    Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                    //sesuaikan dengan content yang di xaml
                    mail.To = TxtEmailForget.Text;
                    mail.Subject = "Forgotten Password";
                    mail.Body = "Hi, " + userid.Name + "." +
                        "This is your new password: " + password;
                    mail.Importance = Outlook.OlImportance.olImportanceNormal;
                    ((Outlook._MailItem)mail).Send();
                    MessageBox.Show("Message has been sent. Check your email now.", "Message", MessageBoxButton.OK);
                    GridForgot.Visibility = Visibility.Hidden;
                    GridLogin.Visibility = Visibility.Visible;
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
            
        }
    }
}
