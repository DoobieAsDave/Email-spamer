using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace EmailSpamer_v3
{
    public enum IconMode
    {
        Success,
        Error,
        Information,
        Question,
        Dude
    }

    public partial class Main : Form
    {
        private string[] adjectives;
        private List<KeyValuePair<string, string>> nouns;

        private bool adjSet, nunSet, emlSet, spaming = false;

        private string targetName, targetMessage, targetEmailAddress, senderEmailAddress, senderEmailPassword;
        private int spamSpeed, mailCount;

        private Sending sendingForm;
        
        public Main()
        {
            try
            {
                InitializeComponent();

                Show();

                var alertBox = new AlertBox("Please note that this is the 3.1 Alpha Version of Email Spamer!\nThe current Build only supports German\n\nMultilangual Support will soon be ready if tha Doobie (this fucker on the left) gets his shit done...\n\nEnjoy", "This is a Alpha Build", IconMode.Dude);
                alertBox.ShowDialog();
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
                
        private void btnUploadAdjectives_Click(object sender, EventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.InitialDirectory = @"C:\User\" + Environment.UserName + @"\Desktop";
                fileDialog.Multiselect = false;
                fileDialog.Title = "Select the .txt File with all your Adjectives";
                fileDialog.Filter = "Textfile | *.txt";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    adjectives = File.ReadAllLines(fileDialog.FileName, Encoding.GetEncoding("iso-8859-1"));

                    adjSet = true;
                    btnUploadAdjectives.BackColor = Color.LightGreen;

                    if (CheckIfInputsAreReady())
                    {
                        grpTargetInformations.Enabled = true;
                        btnSpam.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }        
        private void btnUploadNouns_Click(object sender, EventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.InitialDirectory = @"C:\User\" + Environment.UserName + @"\Desktop";
                fileDialog.Multiselect = false;
                fileDialog.Title = "Select the .txt FIle with all your Nouns";
                fileDialog.Filter = "Textfile | *.txt";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    nouns = new List<KeyValuePair<string, string>>();

                    var lines = File.ReadLines(fileDialog.FileName, Encoding.GetEncoding("iso-8859-1"));
                    foreach(var line in lines)
                    {
                        nouns.Add(new KeyValuePair<string, string>(line.Split(' ')[0], line.Split(' ')[1][0].ToString().ToUpper() + line.Split(' ')[1].Substring(1)));
                    }

                    if (CheckNounTextfile(fileDialog.FileName))
                    {
                        nunSet = true;
                        btnUploadNouns.BackColor = Color.LightGreen;

                        if (CheckIfInputsAreReady())
                        {
                            grpTargetInformations.Enabled = true;
                            btnSpam.Enabled = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }       
        private bool CheckNounTextfile(string filePath)
        {
            try
            {
                string malePronoun = "der";
                string femalePronoun = "die";
                string neutralPronoun = "das";

                int lineCounter = 0;

                foreach(var noun in nouns)
                {
                    lineCounter++;

                    if (noun.Key != malePronoun && noun.Key != femalePronoun && noun.Key != neutralPronoun)
                    {
                        var alertBox = new AlertBox("Invalid Pronoun '" + noun.Key + "' in " + Path.GetFileName(filePath) + " on Line: " + lineCounter + "\n\nPlease enter a valid German Pronoun for the Noun '" + noun.Value + "'", "Unvalid Pronoun occured", IconMode.Information);
                        alertBox.ShowDialog();

                        return false;
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();

                return false;
            }
        }        
        private void btnGmailLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var loginForm = new Login();
                loginForm.ShowDialog();

                if (!loginForm.Cancelled)
                {
                    emlSet = true;
                    btnGmailLogin.BackColor = Color.LightGreen;

                    senderEmailAddress = loginForm.SenderEmailAddress;
                    senderEmailPassword = loginForm.SenderPassword;

                    if (CheckIfInputsAreReady())
                    {
                        grpTargetInformations.Enabled = true;
                        btnSpam.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }        

        private bool CheckIfInputsAreReady()
        {
            try
            {
                if(adjSet && nunSet && emlSet)
                {
                    txtTargetMessage.ForeColor = Color.Gray;
                    txtTargetMessage.Text = "z.B. du bist";

                    return true;
                }
                else
                {
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

        private void txtTargetMessage_Enter(object sender, EventArgs e)
        {
            try
            {
                if (txtTargetMessage.Text == "z.B. du bist")
                {
                    txtTargetMessage.ForeColor = SystemColors.WindowText;
                    txtTargetMessage.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private void txtTargetMessage_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtTargetMessage.Text == string.Empty)
                {
                    txtTargetMessage.ForeColor = Color.Gray;
                    txtTargetMessage.Text = "z.B. du bist";
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }

        private void btnSpam_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtTargetMessage.Text != "z.B. du bist")
                {
                    if (spaming)
                    {
                        spaming = false;
                        btnSpam.Text = "Start Spam";
                        btnSpam.BackColor = Color.LightGreen;

                        spamTimer.Enabled = false;

                        sendingForm.Close();                        

                        var alertBox = new AlertBox("Spamer was stopped!\n\n" + mailCount + " Mails has been sent...", "Spamer stopped", IconMode.Success);
                        alertBox.ShowDialog();

                        mailCount = 0;
                    }
                    else
                    {
                        spaming = true;
                        btnSpam.Text = "Stop Spam";
                        btnSpam.BackColor = Color.LightCoral;

                        if (CheckTargetInputs())
                        {
                            spamTimer.Interval = spamSpeed;
                            spamTimer.Enabled = true;

                            sendingForm = new Sending(targetEmailAddress);
                            sendingForm.Show();
                        }
                    }
                }
                else
                {
                    var alertBox = new AlertBox("The Message for the Target has to be filled with a Sentance\n\nPlease enter a Sentance like 'du siehst aus wie', 'richt wie' or 'du bist'...", "Message for Target is empty", IconMode.Information);
                    alertBox.ShowDialog();
                }                                
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private bool CheckTargetInputs()
        {
            try
            {
                double spamSpeedDouble;

                if (CheckIfTextboxIsEmpty(txtTargetMessage, lblTargetMessage) && CheckIfTextboxIsEmpty(txtTargetSurname, lblTargetSurname) && CheckIfTextboxIsEmpty(txtTargetEmailAddress, lblTargetEmailAddress) && CheckIfTextboxIsEmpty(txtSpamSpeed, lblSpamSpeed))
                {    
                    if (txtSpamSpeed.Text.Contains(',') || txtSpamSpeed.Text.Contains('.'))
                    {
                        if(!double.TryParse(txtSpamSpeed.Text, out spamSpeedDouble))
                        {
                            var alertBox = new AlertBox("Could not read \"Spam Speed\" because it is not a Number", "Could not convert Spam Speed", IconMode.Information);
                            alertBox.ShowDialog();

                            return false;
                        }
                        else
                        {
                            spamSpeed = (int)spamSpeedDouble * 1000;
                        }
                    }
                    else
                    {
                        if (!int.TryParse(txtSpamSpeed.Text, out spamSpeed))
                        {
                            var alertBox = new AlertBox("Could not read \"Spam Speed\" because it is not a Number", "Could not convert Spam Speed", IconMode.Information);
                            alertBox.ShowDialog();

                            return false;
                        }
                        else
                        {
                            spamSpeed *= 1000;
                        }
                    }

                    var address = new MailAddress(txtTargetEmailAddress.Text);
                    if (address.Address == txtTargetEmailAddress.Text)
                    {
                        targetName = txtTargetSurname.Text;
                        targetMessage = txtTargetMessage.Text;
                        targetEmailAddress = txtTargetEmailAddress.Text;

                        return true;
                    }
                    else
                    {
                        var alertbox = new AlertBox("Target Email Address is invalid!\n\nPlease check your spelling...", "Invalid Target Email Address", IconMode.Information);
                        alertbox.ShowDialog();

                        return false;
                    }
                }
                else
                {
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
        private bool CheckIfTextboxIsEmpty(TextBox textbox, Label label)
        {
            try
            {
                if (!string.IsNullOrEmpty(textbox.Text))
                {
                    return true;
                }
                else
                {
                    var alertBox = new AlertBox("The Textbox \"" + label.Text + "\" is empty!\n\nPlease fill it out...", label.Text + " is empty", IconMode.Information);
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

        private void spamTimer_Tick(object sender, EventArgs e)
        {
            try
            {               
                var senderAddress = new MailAddress(senderEmailAddress);
                var targetAddress = new MailAddress(targetEmailAddress, targetName);
                string senderPassword = senderEmailPassword;
                string body = "This Spamer was provided by Doobie Developments";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderAddress.Address, senderPassword)
                };

                string subject = CreateSubject();

                if(subject != "error" || subject != string.Empty)
                {
                    using (var message = new MailMessage(senderAddress, targetAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                        mailCount++;

                        sendingForm.SetSentCounter(mailCount);
                    }
                }
                else
                {
                    var alertBox = new AlertBox("Could not create Subject for Spam Mail!\n\nPlease try again...", "Subject could not be created", IconMode.Error);
                    alertBox.ShowDialog();
                }
            }
            catch (SmtpException smtpEx)
            {
                var alertBox = new AlertBox("Could not send Mail\n\n" + smtpEx.Message, "An Smtp Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private string CreateSubject()
        {
            try
            {
                var rdm = new Random();

                string adjective = adjectives[rdm.Next(0, adjectives.Length)];
                KeyValuePair<string, string> noun = nouns[rdm.Next(0, nouns.Count)];

                switch (noun.Key)
                {
                    case "der":
                        adjective += "er";                      
                        return targetName + " " + targetMessage + " ein " + adjective + " " + noun.Value;                         
                    case "die":
                        adjective += "e";                        
                        return targetName + " " + targetMessage + " eine " + adjective + " " + noun.Value;
                    case "das":
                        adjective += "es";                        
                        return targetName + " " + targetMessage + " ein " + adjective + " " + noun.Value;
                    default:
                        return string.Empty;                        
                }
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();

                return "error";
            }
        }

        private void Main_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            try
            {
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Documentation\Email Spamer v3 - Documentation.pdf"))
                {
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\Documentation\Email Spamer v3 - Documentation.pdf");
                }
                else
                {
                    var alertBox = new AlertBox("Could not find the Email Spamer v3 Documentation!\n\nIt probably got deleted.\nPlease reinstall the Solution to get the Documentation", "Documentation not found", IconMode.Information);
                    alertBox.ShowDialog();
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
