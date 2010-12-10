using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.PasswordEncoder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            if(CryptoKeyEntered() && ValueEntered())
                Result.Text = CryptoUtils.Encrypt(Value.Text.Trim(), CryptoKey.Text.Trim());
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            if (CryptoKeyEntered() && ValueEntered())
                Result.Text = CryptoUtils.Decrypt(Value.Text.Trim(), CryptoKey.Text.Trim());
        }

        private void Sha1Button_Click(object sender, EventArgs e)
        {
            if (ValueEntered())
                Result.Text = CryptoUtils.SHA1(Value.Text.Trim());
        }

        private bool CryptoKeyEntered()
        {
            if (CryptoKey.Text.Trim() == "")
            {
                MessageBox.Show("Enter Crypto Key");
                CryptoKey.Focus();
                return false;
            }
            return true;
        }

        private bool ValueEntered()
        {
            if (Value.Text.Trim() == "")
            {
                MessageBox.Show("Enter Value");
                Value.Focus();
                return false;
            }
            return true;
        }
    }
}
