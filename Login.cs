using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EmailSpamer_v3
{
    public partial class Login : Form
    {
        public string SenderEmailAddress { get; set; }
        public string SenderPassword { get; set; }

        public bool Cancelled = true;

        public Login()
        {
            InitializeComponent();                       
        }
                
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var alertBox = new AlertBox("Did you enable less secure Logins for your Gmail Account?", "Are less secure Logins enabled", IconMode.Question);
                alertBox.ShowDialog(); 

                if (!alertBox.Return)
                {
                    var alertBoxEnable = new AlertBox("To be able to send Spam Mails over your Gmail Email Address you have to enable login access for less secure apps!\n\nIf your loged in to your Gmail account please click this link:\n\n" + @"https://myaccount.google.com/lesssecureapps" + "\n\nCould you enable the Option?", "Enable less secure Gmail Logins", IconMode.Question);
                    alertBoxEnable.ShowDialog();

                    if (!alertBoxEnable.Return)
                    {
                        var alertBoxTurtorial = new AlertBox("OK, please follow this Google article which shows you how to this:\n\n" + @"https://support.google.com/accounts/answer/6010255?hl=en", "How to enable less secure Login for Gmail", IconMode.Information);
                        alertBoxTurtorial.ShowDialog();
                    }
                }

                if (CheckLoginInputs())
                {
                    Cancelled = false;

                    Close();
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private bool CheckLoginInputs()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSenderEmailAddress.Text))
                {
                    if (txtSenderEmailAddress.Text.Contains("@gmail.com"))
                    {
                        var address = new System.Net.Mail.MailAddress(txtSenderEmailAddress.Text);
                        if (address.Address == txtSenderEmailAddress.Text)
                        {
                            SenderEmailAddress = txtSenderEmailAddress.Text;
                            SenderPassword = txtSenderPassword.Text;

                            return true;
                        }
                        else
                        {
                            var alertBox = new AlertBox("Your entered a invalid Gmail Email Address!\n\nPlease check your spelling...", "Invalid Gmail Email Address entered", IconMode.Information);
                            alertBox.ShowDialog();

                            return false;
                        }
                    }
                    else
                    {
                        var alertBox = new AlertBox("You have to use a Gmail Email Address for this Spamer", "No Gmail Email Address entered", IconMode.Information);
                        alertBox.ShowDialog();

                        return false;
                    }
                }
                else
                {
                    var alertBox = new AlertBox("Please enter your Gmail Email Address", "No Sender Email Address entered", IconMode.Information);
                    alertBox.ShowDialog();

                    return false;
                }
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();

                return false;
            }
        }

        private void Login_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            try
            {
                var alertBox = new AlertBox("To be able to send Spam Mails over your Gmail Email Address you have to enable login access for less secure apps!\n\nIf your loged in to your Gmail account please click this link:\n\n" + @"https://myaccount.google.com/lesssecureapps" + "\n\nCould you enable the Option?", "Enable less secure Gmail Logins", IconMode.Question);
                alertBox.ShowDialog();

                if (!alertBox.Return)
                {
                    var alertBoxInfo = new AlertBox("OK, please follow this Google article which shows you how to this:\n\n" + @"https://support.google.com/accounts/answer/6010255?hl=en", "How to enable less secure Login for Gmail", IconMode.Information);
                    alertBoxInfo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Cancelled)
                {
                    var alertBox = new AlertBox("Do you really want to cancel loging in?", "Cancel Login", IconMode.Question);
                    alertBox.ShowDialog();

                    if (!alertBox.Return)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }        
    }
}
