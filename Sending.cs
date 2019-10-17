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
    public partial class Sending : Form
    {
        public Sending(string targetMail)
        {
            InitializeComponent();

            pcbSendingGif.Image = Properties.Resources.send;
            lblSendingMessage.Text += targetMail;
        }

        public void SetSentCounter(int sentEmail)
        {
            lblSentMessage.Text = sentEmail.ToString() + " Spam Emails sent";
        }
    }
}
