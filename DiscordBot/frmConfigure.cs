using System;
using System.Windows.Forms;
using Discord;
using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using Color = System.Drawing.Color;

namespace DiscordBot
{
    public partial class FrmConfigure : Form
    {
        // Method to check if this form should open or not.
        internal static async void CheckRunConfigurator(bool run = true)
        {
            if (!run) return;

            if (Configuration.Load().FirstTimeRun)
            {
                await new LogMessage(LogSeverity.Info, "Configurator",
                    "Hello! It looks like your new here. We're setting things up for you.").PrintToConsole();
                await new LogMessage(LogSeverity.Info, "Configurator",
                    "We're gonna need a few details and we'll be up and running in no time!").PrintToConsole();
            }
            
            await new LogMessage(LogSeverity.Info, "Configurator", "Launching Configurator.").PrintToConsole();
                
            FrmConfigure configurator = new FrmConfigure();
            configurator.ShowDialog();
        }
        
        /* frmConfigure starting point */
        
        private bool _allowedToClose, _validBotToken, _validDbSettings;
        
        private FrmConfigure()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
        }

        private void frmConfigure_Load(object sender, EventArgs e)
        {
            var configuration = Configuration.Load();
            
            txtBotToken.Text = configuration.BotToken ?? "";

            txtDbHost.Text = configuration.DatabaseHost ?? "127.0.0.1";
            txtDbPort.Text = configuration.DatabasePort.ToString() ?? "3306";
            txtDbName.Text = configuration.DatabaseName ?? "DiscordBot";
            txtDbUser.Text = configuration.DatabaseUser ?? "root";
            txtDbPass.Text = configuration.DatabasePassword ?? "";

            txtOwnerId.Text = configuration.Developer.ToString();

            InvalidateBotValidationLabel();
            InvalidateDbValidationLabel();
        }

        private async void frmConfigure_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            
            if (_allowedToClose)
            {
                Configuration.UpdateConfiguration(firstTimeRun: false);
                e.Cancel = false;
            }
            else
            {
                var result = MessageBox.Show(@"Are you sure you want to exit? You won't be able to launch the bot without setting up the configuration.", @"Are you sure?", MessageBoxButtons.YesNo);
                
                if (result != DialogResult.Yes) return;
                
                await new LogMessage(LogSeverity.Warning, "Configurator",
                    "Configurator setup incomplete. Exiting application.").PrintToConsole();
                    
                e.Cancel = false;
                Environment.Exit(0);
            }
        }

        #region 2. Bot Token
        private void txtBotToken_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ValidateToken();
            }
        }

        private void btnValidateToken_Click(object sender, EventArgs e)
        {
            ValidateToken();
        }

        private async void ValidateToken()
        {
            lblBotTokenSetup.ForeColor = Color.Black;

            if (string.IsNullOrWhiteSpace(txtBotToken.Text))
            {
                await new LogMessage(LogSeverity.Warning, "Bot Token Validation Error",
                    "Bot Token field can not be blank or null.").PrintToConsole();
                return;
            }

            _validBotToken = await Methods.VerifyBotToken(txtBotToken.Text);

            if (_validBotToken)
            {
                tslblBotValidation.Text = @"Token Validated.";
                tslblBotValidation.ForeColor = Color.Green;

                MessageBox.Show(@"Token verification successful. The token has been saved to the configuration. If you go to change it, you will need to verify the token again.", @"Token verification successful", MessageBoxButtons.OK);

                Configuration.UpdateConfiguration(botToken: txtBotToken.Text);
            }
            else
            {
                InvalidateBotValidationLabel();
                MessageBox.Show(@"We were unable to verify your token. Please ensure you have a stable internet connection and a valid token.", @"Unable to verify token", MessageBoxButtons.OK);
            }
        }

        private void InvalidateBotValidationLabel()
        {
            tslblBotValidation.Text = @"Token Validation Required.";
            tslblBotValidation.ForeColor = Color.Red;
        }

        private void txtBotToken_TextChanged(object sender, EventArgs e)
        {
            _validBotToken = false;
            InvalidateBotValidationLabel();
        }

        private void lblGenerateNewToken_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discordapp.com/developers/applications/");
        }
        #endregion

        #region 3. Database Settings
        private void txtDbHost_TextChanged(object sender, EventArgs e)
        {
            InvalidateDbValidationLabel();
        }

        private void txtDbPort_TextChanged(object sender, EventArgs e)
        {
            InvalidateDbValidationLabel();
        }

        private void txtDbName_TextChanged(object sender, EventArgs e)
        {
            InvalidateDbValidationLabel();
        }

        private void txtDbUser_TextChanged(object sender, EventArgs e)
        {
            InvalidateDbValidationLabel();
        }

        private void txtDbPass_TextChanged(object sender, EventArgs e)
        {
            InvalidateDbValidationLabel();
        }

        private void btnValidateDbSettings_Click(object sender, EventArgs e)
        {
            TestDatabaseValues();
        }

        private void InvalidateDbValidationLabel()
        {
            _validDbSettings = false;
            
            tslblDbValidation.Text = @"Database Validation Required.";
            tslblDbValidation.ForeColor = Color.Red;
        }

        private void TestDatabaseValues()
        {
            InvalidateDbValidationLabel();
            lblDatabaseSetup.ForeColor = Color.Black;
            lblDatabaseName.ForeColor = Color.Black;

            int port = 3306;
            try
            {
                int.TryParse(txtDbPort.Text, out port);
            }
            catch (Exception e)
            {
                new LogMessage(LogSeverity.Error, "DB Validation Error", e.Message).PrintToConsole().GetAwaiter();
                return;
            }

            (bool validConnection, bool dbExists) = DatabaseActivity.TestDatabaseSettings(txtDbHost.Text, txtDbUser.Text, txtDbPass.Text, port, txtDbName.Text).GetAwaiter().GetResult();

            if (!validConnection)
            {
                MessageBox.Show(
                    @"Unable to connect to the database server specified. Please ensure the values are correct and your server is up and running!",
                    @"Unable to connect to database", MessageBoxButtons.OK);
                return;
            }

            if (dbExists)
            {
                var result =
                    MessageBox.Show(@"A database with the name specified already exists. Would you like to use that?",
                        @"Database Exists", MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                {
                    lblDatabaseName.ForeColor = Color.Red;
                    return;
                }
            }

            MessageBox.Show(@"Database settings configured successfully!", @"Successful Database Connection", MessageBoxButtons.OK);
            Configuration.UpdateConfiguration(databaseHost: txtDbHost.Text, databasePort: port, databaseName: txtDbName.Text, databaseUser: txtDbUser.Text, databasePassword: txtDbPass.Text);
            _validDbSettings = true;
            tslblDbValidation.Text = @"Database Validated.";
            tslblDbValidation.ForeColor = Color.Green;
        }
        #endregion

        #region 4. Inviting the Bot
        private void txtClientID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClientID.Text)) return;

            txtInviteUrl.Text =
                $@"https://discordapp.com/oauth2/authorize?client_id={txtClientID.Text}&scope=bot&permissions=8";
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            LaunchInviteCode();
        }

        private void txtClientID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                LaunchInviteCode();
            }
        }

        private void LaunchInviteCode()
        {
            System.Diagnostics.Process.Start(txtInviteUrl.Text);
        }
        #endregion

        private async void btnCompleteSetup_Click(object sender, EventArgs e)
        {
            if (!_validBotToken || !_validDbSettings)
            {
                if (!_validBotToken)
                {
                    lblBotTokenSetup.ForeColor = Color.Red;
                }

                if (!_validDbSettings)
                {
                    lblDatabaseSetup.ForeColor = Color.Red;
                }

                MessageBox.Show(@"One or more areas still need verified. Please verify your settings before completing the setup phase.", @"Unverified Settings", MessageBoxButtons.OK);

                return;
            }

            _allowedToClose = true;

            ulong uId = Configuration.Load().Developer;

            try
            {
                ulong.TryParse(txtOwnerId.Text, out uId);
            }
            catch (Exception)
            {
                await new LogMessage(LogSeverity.Error, "Owner ID",
                    "There was an issue parsing the Owner ID into the configuration.").PrintToConsole();
            }

            Configuration.UpdateConfiguration(developer: uId);

            this.Close();
        }

        private void tslblDonate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.me/mythicalcuddles");
        }

        private void tslblPatreon_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.patreon.com/mythicalcuddles");
        }

        private void tslblWebsite_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.mythicalcuddles.xyz");
        }
    }
}