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
    public partial class AlertBox : Form
    {
        public bool Return { get; set; }

        public AlertBox(string alertMessage, string alertTitle, IconMode alertIcon)
        {
            try
            {
                InitializeComponent();

                Text = alertTitle;
                txtAlertMessage.Text = alertMessage;

                if (alertMessage.Contains("\n"))
                {
                    txtAlertMessage.Location = new Point(78, 20);
                    txtAlertMessage.Size = new Size(294, 104);
                }                

                switch (alertIcon)
                {
                    case IconMode.Success:
                        pcbAlertIcon.Image = Properties.Resources.success;
                        break;
                    case IconMode.Error:
                        pcbAlertIcon.Image = Properties.Resources.error;
                        break;
                    case IconMode.Information:
                        pcbAlertIcon.Image = Properties.Resources.information;
                        break;
                    case IconMode.Question:
                        pcbAlertIcon.Image = Properties.Resources.question;
                        btnOK.Visible = false;
                        btnYes.Visible = true;
                        btnNo.Visible = true;
                        break;
                    case IconMode.Dude:
                        pcbAlertIcon.Image = Properties.Resources.dude;
                        break;
                }

                this.Focus();
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                Return = true;

                Close();
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                Return = false;

                Close();
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }

        private void txtAlertMessage_Enter(object sender, EventArgs e)
        {
            try
            {
                if (btnOK.Visible)
                {
                    btnOK.Focus();
                }
                else
                {
                    btnYes.Focus();
                }
            }
            catch (Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
        private void txtAlertMessage_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch(Exception ex)
            {
                var alertBox = new AlertBox(ex.Message, "An Error occured", IconMode.Error);
                alertBox.ShowDialog();
            }
        }
    }
}
