using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

using DiscordBot.Common;

using Google.Authenticator;

using MelissaNet;

namespace DiscordBot
{
    public partial class frmAuth : Form
    {
        // Removed as of 2.12.0.0 due to no real point of use.
        
        public frmAuth()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtAccountTitle.Text = @"MythicalCuddlesXYZ DiscordBot";
            string key = Guid.NewGuid().ToString().Replace("-", "");
            txtSecretKey.Text = key;
            //Configuration.UpdateConfiguration(secretKey: Cryptography.EncryptString(key));

            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var setupCode = tfA.GenerateSetupCode(txtAccountTitle.Text, txtSecretKey.Text, pbQR.Width, pbQR.Height);

            WebClient wc = new WebClient();
            MemoryStream ms = new MemoryStream(wc.DownloadData(setupCode.QrCodeSetupImageUrl));
            pbQR.Image = Image.FromStream(ms);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var result = tfA.ValidateTwoFactorPIN(txtSecretKey.Text, txtCode.Text);

            MessageBox.Show(result ? @"Code Valid! TwoAuth setup successful." : @"Incorrect", @"Result");

            if (result)
            {
                Close();
            }
        }
    }
}