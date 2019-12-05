using B32.Context;
using B32.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Mail;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Outlook = Microsoft.Office.Interop.Outlook;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace B32
{
    /// <summary>
    /// Interaction logic for DashboardAdmin.xaml
    /// </summary>
    public partial class DashboardAdmin : Window
    {
        private static Random random = new Random();
        MyContext myContext = new MyContext();
        List<TransactionItem> TransDetail = new List<TransactionItem>();
        string password;
        int roleid, supplierid, itemid;
        int grandTotal = 0;
        string receipt = "NameItem\t\tQuantity\tSubTotal";

        public DashboardAdmin()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            TxtId.Text = "";
            TxtName.Text = "";
            TxtEmail.Text = "";
            TxtIdItem.Text = "";
            TxtNameItem.Text = "";
            TxtStock.Text = "";
            TxtPrice.Text = "";
            TxtIdTrans.Text = "";
            TxtIdTranss.Text = "";
            TxtPay.Text = "0";
            TxtQuantity.Text = "";
            TxtChange.Text = "Rp. 0 ,-";
            GridSupplier.ItemsSource = myContext.Suppliers.ToList();
            GridItem.ItemsSource = myContext.Items.ToList();
            //GridTransItem.ItemsSource = myContext.TransactionItems.ToList();
            GridTrans.ItemsSource = myContext.Transactions.ToList();
            comboBoxSupplier.ItemsSource = myContext.Suppliers.ToList();
            comboBoxItem.ItemsSource = myContext.Items.ToList();
        }

        private void GridSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GridSupplier.SelectedItem;
            TxtId.Text = (GridSupplier.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtName.Text = (GridSupplier.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            TxtEmail.Text = (GridSupplier.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            btnSubmit.IsEnabled = false;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void GridItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GridItem.SelectedItem;
            TxtIdItem.Text = (GridItem.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtNameItem.Text = (GridItem.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            TxtStock.Text = (GridItem.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            TxtPrice.Text = (GridItem.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
            comboBoxSupplier.Text = (GridItem.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
            btnSubmitItem.IsEnabled = false;
            btnUpdateItem.IsEnabled = true;
            btnDeleteItem.IsEnabled = true;
        }
        private void GridTransItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GridTransItem.SelectedItem;
            comboBoxItem.Text = (GridTransItem.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtQuantity.Text = (GridTransItem.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
        }

        private void GridTrans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GridTrans.SelectedItem;
            TxtIdTranss.Text = (GridTrans.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
        }

        private void TxtEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtNameItem_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9!]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtPay_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var res = myContext.Suppliers.FirstOrDefault(dataInMail => dataInMail.Email == TxtEmail.Text);
            if (TxtName.Text == "")
            {
                MessageBox.Show("Fill column name please", "Alert", MessageBoxButton.OK);
                TxtName.Focus();
            }
            else if (TxtEmail.Text == "")
            {
                MessageBox.Show("Fill column email please", "Alert", MessageBoxButton.OK);
                TxtEmail.Focus();
            }
            else if (res != null)
            {
                MessageBox.Show("This email already registered", "Alert", MessageBoxButton.OK);
                TxtEmail.Focus();
            }
            else
            {
                var push = new Supplier(TxtName.Text, TxtEmail.Text);
                myContext.Suppliers.Add(push);
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    try
                    {
                        //Outlook._Application _app = new Outlook.Application();
                        //Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                        ////sesuaikan dengan content yang di xaml
                        //mail.To = TxtEmail.Text;
                        //mail.Subject = "Bootcamp 32";
                        //mail.Body = "Congratulations, " + TxtName.Text + " has been success";
                        //mail.Importance = Outlook.OlImportance.olImportanceNormal;
                        //((Outlook._MailItem)mail).Send();
                        Load();
                        MessageBox.Show("Message has been sent.", "Message", MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
                    }
                }
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int num = Convert.ToInt32(TxtId.Text);
            var updated = myContext.Suppliers.FirstOrDefault(data => data.Id == num);
            if (TxtId.Text == "")
            {
                MessageBox.Show("Fill column id please", "Alert", MessageBoxButton.OK);
                TxtName.Focus();
            }
            else if (TxtName.Text == "")
            {
                MessageBox.Show("Fill column name please", "Alert", MessageBoxButton.OK);
                TxtName.Focus();
            }
            else
            {
                updated.Name = TxtName.Text;
                updated.Email = TxtEmail.Text;
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    Load();
                    MessageBox.Show("Updated Success!", "Success", MessageBoxButton.OK);
                }
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    int num = Convert.ToInt32(TxtId.Text);
                    var deleted = myContext.Suppliers.FirstOrDefault(data => data.Id == num);
                    myContext.Suppliers.Remove(deleted);
                    myContext.SaveChanges();
                    Load();
                }
                catch (Exception)
                {
                    MessageBox.Show("Your data has been deleted.", "Message", MessageBoxButton.OK);
                }
            }
        }

        private void btnSubmitItem_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNameItem.Text == "")
            {
                MessageBox.Show("Fill column name please", "Alert", MessageBoxButton.OK);
                TxtNameItem.Focus();
            }
            else if (TxtStock.Text == "")
            {
                MessageBox.Show("Fill column stock please", "Alert", MessageBoxButton.OK);
                TxtStock.Focus();
            }
            else if (TxtPrice.Text == "")
            {
                MessageBox.Show("Fill column price please", "Alert", MessageBoxButton.OK);
                TxtPrice.Focus();
            }
            else
            {
                try
                {
                    var itemstock = myContext.Items.FirstOrDefault(a => a.Name == TxtNameItem.Text);
                    if (itemstock != null)
                    {
                        if (itemstock.Name == TxtNameItem.Text && itemstock.Price.ToString() == TxtPrice.Text && itemstock.SupplierFK.Id.ToString() == comboBoxSupplier.Text)
                        {
                            itemstock.Stock += Convert.ToInt32(TxtStock.Text);
                            myContext.SaveChanges();
                            Load();
                            MessageBox.Show("1 row has been updated.", "Message", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("Don't submit in here, please update item");
                        }
                    }
                    else
                    {
                        var supplier = myContext.Suppliers.FirstOrDefault(data => data.Id == supplierid);
                        var push = new Item(TxtNameItem.Text, Convert.ToInt32(TxtStock.Text), Convert.ToInt32(TxtPrice.Text), supplier);
                        myContext.Items.Add(push);
                        myContext.SaveChanges();
                        Load();
                        MessageBox.Show("1 row has been inserted.", "Message", MessageBoxButton.OK);
                    }

                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                //comboBoxSupplier.Text = "--Choose Supplier--";

            }
        }

        private void btnUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            int num = Convert.ToInt32(TxtIdItem.Text);
            var updated = myContext.Items.FirstOrDefault(data => data.Id == num);
            var supplier = myContext.Suppliers.FirstOrDefault(data => data.Id == supplierid);
            if (TxtNameItem.Text == "")
            {
                MessageBox.Show("Fill column name please", "Alert", MessageBoxButton.OK);
                TxtNameItem.Focus();
            }
            else if (TxtStock.Text == "")
            {
                MessageBox.Show("Fill column stock please", "Alert", MessageBoxButton.OK);
                TxtStock.Focus();
            }
            else if (TxtPrice.Text == "")
            {
                MessageBox.Show("Fill column price please", "Alert", MessageBoxButton.OK);
                TxtPrice.Focus();
            }
            else
            {
                try
                {
                    updated.Name = TxtNameItem.Text;
                    updated.Stock = Convert.ToInt32(TxtStock.Text);
                    updated.Price = Convert.ToInt32(TxtPrice.Text);
                    updated.SupplierFK = supplier;
                    myContext.SaveChanges();
                    Load();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                MessageBox.Show("Updated Success!", "Success", MessageBoxButton.OK);
                btnSubmitItem.IsEnabled = true;
                btnUpdateItem.IsEnabled = false;
                btnUpdateItem.IsEnabled = false;
                //comboBoxSupplier.Text = "--Choose Supplier--";
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    int num = Convert.ToInt32(TxtIdItem.Text);
                    var deleted = myContext.Items.FirstOrDefault(data => data.Id == num);
                    myContext.Items.Remove(deleted);
                    myContext.SaveChanges();
                    Load();
                }
                catch (Exception)
                {
                    MessageBox.Show("Your data has been deleted.", "Message", MessageBoxButton.OK);
                }
                btnSubmitItem.IsEnabled = true;
                btnUpdateItem.IsEnabled = false;
                btnUpdateItem.IsEnabled = false;
                //comboBoxSupplier.Text = "--Choose Supplier--";
            }
        }

        private void comboBoxSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            supplierid = Convert.ToInt32(comboBoxSupplier.SelectedValue.ToString());
        }

        private void btnAddTrans_Click(object sender, RoutedEventArgs e)
        {
            var push = new Transaction(0, 0);
            myContext.Transactions.Add(push);
            myContext.SaveChanges();
            TxtTransId.Text = Convert.ToString(push.Id);
            btnAddTrans.IsEnabled = false;
            comboBoxItem.IsEnabled = true;
            TxtPay.IsEnabled = true;
            TxtQuantity.IsEnabled = true;
            TxtNameItem.IsEnabled = true;
            btnAddToCart.IsEnabled = true;
            btnCancel.IsEnabled = true;
        }

        private void TxtPay_KeyDown(Object sender, KeyEventArgs e)
        {
            TxtChange.Text = "Rp. " + (Convert.ToInt32(TxtPay.Text) - Convert.ToInt32(TxtTotal.Text)).ToString("n0") + ",-";
            btnSubmitTrans.IsEnabled = true;
        }

        private int getTotal()
        {
            var priceitem = myContext.Items.FirstOrDefault(d => d.Id == itemid);
            return Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(priceitem.Price.ToString());
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (TxtQuantity.Text == "")
            {
                MessageBox.Show("Fill column quantity please", "Alert", MessageBoxButton.OK);
                TxtQuantity.Focus();
            }
            else
            {
                try
                {
                    var dataItem = GridTransItem.SelectedItem;
                    var itemFK = myContext.Items.FirstOrDefault(data => data.Id == itemid);
                    var transFK = myContext.Transactions.FirstOrDefault(data => data.Id.ToString() == TxtTransId.Text);
                    var transitem = myContext.TransactionItems.FirstOrDefault(data => data.TransactionFK.Id.ToString() == TxtTransId.Text);
                    var iditems = TransDetail.Find(a => a.ItemFK.Id == itemid);
                    if (iditems == null)
                    {
                        if (Convert.ToInt32(TxtQuantity.Text) <= itemFK.Stock)
                        {
                            itemFK.Stock -= Convert.ToInt32(TxtQuantity.Text);
                            TransDetail.Add(new TransactionItem { TransactionFK = transFK, ItemFK = itemFK, Quantity = Convert.ToInt32(TxtQuantity.Text), SubTotal = getTotal() });
                            GridTransItem.Items.Add(new { NameItem = itemFK.Name.ToString(), Quantity = TxtQuantity.Text, SubTotal = getTotal() });
                            grandTotal += getTotal();
                            TxtTotal.Text = Convert.ToString(grandTotal);
                            Load();
                        }
                        else
                        {
                            MessageBox.Show("Quantity exceeds stock limit");
                        }

                    }
                    else
                    {
                        if (iditems.ItemFK.Id == itemid)
                        {
                            if (Convert.ToInt32(TxtQuantity.Text) <= itemFK.Stock)
                            {
                                itemFK.Stock -= Convert.ToInt32(TxtQuantity.Text);
                                int Qty = iditems.Quantity + Convert.ToInt32(TxtQuantity.Text);
                                int subTotal = iditems.SubTotal + getTotal();
                                GridTransItem.SelectedIndex = iditems.ItemFK.Id;
                                var index = Convert.ToInt32(iditems.Id);
                                GridTransItem.Items.RemoveAt(index);
                                //GridTransItem.Items.Remove(GridTransItem.SelectedItem);
                                GridTransItem.Items.Add(new { NameItem = itemFK.Name.ToString(), Quantity = Qty, SubTotal = subTotal });
                                TransDetail.Add(new TransactionItem { TransactionFK = transFK, ItemFK = itemFK, Quantity = Qty, SubTotal = subTotal });
                                TransDetail.Remove(iditems);
                                grandTotal += getTotal();
                                TxtTotal.Text = Convert.ToString(grandTotal);
                                Load();
                            }
                            else
                            {
                                MessageBox.Show("Quantity exceeds stock limit");
                            }
                        }
                    }

                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                //comboBoxItem.Text = "--Choose Item--";
                TxtPay.IsEnabled = true;
                btnAddToCart.IsEnabled = true;
                btnDeleteTrans.IsEnabled = true;
                btnCancel.IsEnabled = true;
            }
        }

        private void btnDeleteTrans_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    var iditems = TransDetail.Find(a => a.ItemFK.Id == itemid);
                    grandTotal -= iditems.SubTotal;
                    iditems.ItemFK.Stock += iditems.Quantity;
                    TxtTotal.Text = (Convert.ToInt32(TxtTotal.Text) - iditems.SubTotal).ToString();
                    TransDetail.Remove(iditems);
                    //GridTransItem.Items.Remove(GridTransItem.SelectedItem);
                    GridTransItem.Items.RemoveAt(GridTransItem.SelectedIndex);
                    Load();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                MessageBox.Show("Your item has been deleted.", "Message", MessageBoxButton.OK);
            }
            //comboBoxItem.Text = "--Choose Item--";
            btnAddToCart.IsEnabled = true;
            btnDeleteTrans.IsEnabled = true;
            btnCancel.IsEnabled = true;
        }

        private void comboBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemid = Convert.ToInt32(comboBoxItem.SelectedValue.ToString());
        }

        private void btnDeleteTranss_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this data transaction?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    int num = Convert.ToInt32(TxtIdTrans.Text);
                    var deleted = myContext.Transactions.FirstOrDefault(data => data.Id == num);
                    myContext.Transactions.Remove(deleted);
                    myContext.SaveChanges();
                    Load();
                }
                catch (Exception)
                {
                    MessageBox.Show("Your data transaction has been deleted.", "Message", MessageBoxButton.OK);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var transitems = TransDetail.Find(a => a.TransactionFK.Id.ToString() == TxtTransId.Text);
            if (transitems != null)
            {
                TxtTotal.Text = "0";
                grandTotal = 0;
                transitems.ItemFK.Stock += transitems.Quantity;
                TransDetail.Clear();
                GridTransItem.Items.Clear();
            }
            GridTransItem.Items.Refresh();
            btnDeleteTrans.IsEnabled = false;
            btnCancel.IsEnabled = false;
        }

        private void btnSubmitTrans_Click(object sender, RoutedEventArgs e)
        {
            if (TxtPay.Text == "0" || TxtPay.Text == "")
            {
                MessageBox.Show("Fill column pay please", "Alert", MessageBoxButton.OK);
                TxtPay.Focus();
            }
            else
            {
                int change = Convert.ToInt32(TxtPay.Text) - Convert.ToInt32(TxtTotal.Text);
                try
                {
                    int transid = Convert.ToInt32(TxtTransId.Text);
                    var item = myContext.TransactionItems.FirstOrDefault(i => i.TransactionFK.Id == transid);
                    var trans = myContext.Transactions.FirstOrDefault(data => data.Id == transid);
                    trans.Total = Convert.ToInt32(TxtTotal.Text);
                    trans.Pay = Convert.ToInt32(TxtPay.Text);
                    myContext.SaveChanges();
                    TxtChange.Text = "Rp. " + change.ToString("n0") + ",-";
                    foreach (var transItem in TransDetail)
                    {
                        myContext.TransactionItems.Add(transItem);
                        myContext.SaveChanges();
                    }

                    MessageBox.Show("Your change is Rp. " + change.ToString("n0") + ",-", "Message", MessageBoxButton.OK);
                    receipt += item.ItemFK.Name + "\t" + item.Quantity + "\t" + item.SubTotal + "\nTotal: Rp. "
                        + item.TransactionFK.Total.ToString("n0") + ",-\nPay: Rp. "
                        + item.TransactionFK.Pay.ToString("n0") + ",-\nChange: Rp. "
                        + (item.TransactionFK.Pay - item.TransactionFK.Total).ToString("n0") + ",-";
                    using (PdfDocument document = new PdfDocument())
                    {
                        //Add a page to the document
                        PdfPage page = document.Pages.Add();

                        //Create PDF graphics for a page
                        PdfGraphics graphics = page.Graphics;

                        //Set the standard font
                        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                        //Draw the text
                        graphics.DrawString(receipt, font, PdfBrushes.Black, new PointF(0, 0));

                        //Save the document
                        document.Save("Receipt.pdf");
                    }
                }
                catch (Exception)
                {
                }
                //comboBoxItem.Text = "--Choose Item--";
                TxtTransId.Text = "";
                GridTransItem.Items.Clear();
                grandTotal = 0;
                TxtTotal.Text = "0";
                TxtChange.Text = "0";
                TxtPay.IsEnabled = false;
                btnAddTrans.IsEnabled = true;
                comboBoxItem.IsEnabled = false;
                TxtQuantity.IsEnabled = false;
                btnAddToCart.IsEnabled = false;
                btnDeleteTrans.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnSubmitTrans.IsEnabled = false;
                Load();
            }
        }
        
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            var oldPw = myContext.Users.FirstOrDefault(o => o.Password == TxtOldPW.Password);
            if (TxtOldPW.Password == "")
            {
                MessageBox.Show("Please, fill your old password", "Alert", MessageBoxButton.OK);
                TxtOldPW.Focus();
            }
            else if (TxtNewPW.Password == "")
            {
                MessageBox.Show("Please, fill your confirm password", "Alert", MessageBoxButton.OK);
                TxtNewPW.Focus();
            }
            else if (TxtConfirmPW.Password == "")
            {
                MessageBox.Show("Please, fill your confirm password", "Alert", MessageBoxButton.OK);
                TxtConfirmPW.Focus();
            }
            else if (oldPw == null)
            {
                MessageBox.Show("Your password is wrong!", "Alert", MessageBoxButton.OK);
                TxtOldPW.Focus();
            }
            else if (TxtNewPW.Password != TxtConfirmPW.Password)
            {
                MessageBox.Show("Your new password and confirm password not match!", "Alert", MessageBoxButton.OK);
                TxtConfirmPW.Focus();
            }
            else
            {
                try
                {
                    var userid = myContext.Users.FirstOrDefault(u => u.Email == TxtEmail.Text);
                    userid.ChangePassword = userid.Password;
                    userid.Password = TxtNewPW.Password;
                    myContext.SaveChanges();
                    MessageBox.Show("Your password has been changed!", "Message", MessageBoxButton.OK);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
