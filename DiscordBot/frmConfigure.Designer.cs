using System.ComponentModel;

namespace DiscordBot
{
    partial class FrmConfigure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(FrmConfigure));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnValidateToken = new System.Windows.Forms.Button();
            this.txtBotToken = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.txtDbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnValidateDbSettings = new System.Windows.Forms.Button();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDbPort = new System.Windows.Forms.TextBox();
            this.txtDbHost = new System.Windows.Forms.TextBox();
            this.lblDatabaseSetup = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblGenerateNewToken = new System.Windows.Forms.Label();
            this.lblBotTokenSetup = new System.Windows.Forms.Label();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnInvite = new System.Windows.Forms.Button();
            this.txtInviteUrl = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslblBotValidation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblDbValidation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblDonate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblPatreon = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtOwnerId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnCompleteSetup = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 186);
            this.panel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label7.Location = new System.Drawing.Point(3, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(277, 151);
            this.label7.TabIndex = 9;
            this.label7.Text = resources.GetString("label7.Text");
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15.75F,
                ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "1. Introduction";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Discord API Bot Token:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnValidateToken
            // 
            this.btnValidateToken.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidateToken.Location = new System.Drawing.Point(3, 198);
            this.btnValidateToken.Name = "btnValidateToken";
            this.btnValidateToken.Size = new System.Drawing.Size(277, 24);
            this.btnValidateToken.TabIndex = 1;
            this.btnValidateToken.Text = "Validate Bot Token";
            this.btnValidateToken.UseVisualStyleBackColor = true;
            this.btnValidateToken.Click += new System.EventHandler(this.btnValidateToken_Click);
            // 
            // txtBotToken
            // 
            this.txtBotToken.Location = new System.Drawing.Point(3, 170);
            this.txtBotToken.Name = "txtBotToken";
            this.txtBotToken.PasswordChar = '•';
            this.txtBotToken.Size = new System.Drawing.Size(277, 22);
            this.txtBotToken.TabIndex = 0;
            this.txtBotToken.TextChanged += new System.EventHandler(this.txtBotToken_TextChanged);
            this.txtBotToken.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBotToken_KeyPress);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LavenderBlush;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.lblDatabaseName);
            this.panel2.Controls.Add(this.txtDbPass);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.txtDbUser);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.btnValidateDbSettings);
            this.panel2.Controls.Add(this.txtDbName);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtDbPort);
            this.panel2.Controls.Add(this.txtDbHost);
            this.panel2.Controls.Add(this.lblDatabaseSetup);
            this.panel2.Location = new System.Drawing.Point(303, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 302);
            this.panel2.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(3, 33);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(277, 45);
            this.label16.TabIndex = 9;
            this.label16.Text = "The application needs to connect to an (My)SQL Server to store information. Pleas" +
                                "e enter the details to your server below.";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.Location = new System.Drawing.Point(3, 126);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(277, 20);
            this.lblDatabaseName.TabIndex = 17;
            this.lblDatabaseName.Text = "Database Name:";
            this.lblDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDbPass
            // 
            this.txtDbPass.Location = new System.Drawing.Point(3, 245);
            this.txtDbPass.Name = "txtDbPass";
            this.txtDbPass.PasswordChar = '•';
            this.txtDbPass.Size = new System.Drawing.Size(277, 22);
            this.txtDbPass.TabIndex = 6;
            this.txtDbPass.TextChanged += new System.EventHandler(this.txtDbPass_TextChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(3, 222);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(277, 20);
            this.label12.TabIndex = 15;
            this.label12.Text = "Password:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(3, 197);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(277, 22);
            this.txtDbUser.TabIndex = 5;
            this.txtDbUser.TextChanged += new System.EventHandler(this.txtDbUser_TextChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 174);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(277, 20);
            this.label11.TabIndex = 14;
            this.label11.Text = "Username:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnValidateDbSettings
            // 
            this.btnValidateDbSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidateDbSettings.Location = new System.Drawing.Point(3, 273);
            this.btnValidateDbSettings.Name = "btnValidateDbSettings";
            this.btnValidateDbSettings.Size = new System.Drawing.Size(277, 24);
            this.btnValidateDbSettings.TabIndex = 7;
            this.btnValidateDbSettings.Text = "Validate Database Settings";
            this.btnValidateDbSettings.UseVisualStyleBackColor = true;
            this.btnValidateDbSettings.Click += new System.EventHandler(this.btnValidateDbSettings_Click);
            // 
            // txtDbName
            // 
            this.txtDbName.Location = new System.Drawing.Point(3, 149);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(277, 22);
            this.txtDbName.TabIndex = 4;
            this.txtDbName.TextChanged += new System.EventHandler(this.txtDbName_TextChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(155, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(125, 20);
            this.label14.TabIndex = 16;
            this.label14.Text = "Port:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(146, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Hostname:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDbPort
            // 
            this.txtDbPort.Location = new System.Drawing.Point(155, 101);
            this.txtDbPort.Name = "txtDbPort";
            this.txtDbPort.Size = new System.Drawing.Size(125, 22);
            this.txtDbPort.TabIndex = 3;
            this.txtDbPort.TextChanged += new System.EventHandler(this.txtDbPort_TextChanged);
            // 
            // txtDbHost
            // 
            this.txtDbHost.Location = new System.Drawing.Point(3, 101);
            this.txtDbHost.Name = "txtDbHost";
            this.txtDbHost.Size = new System.Drawing.Size(146, 22);
            this.txtDbHost.TabIndex = 2;
            this.txtDbHost.TextChanged += new System.EventHandler(this.txtDbHost_TextChanged);
            // 
            // lblDatabaseSetup
            // 
            this.lblDatabaseSetup.Font = new System.Drawing.Font("Segoe UI", 15.75F,
                ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblDatabaseSetup.Location = new System.Drawing.Point(3, 3);
            this.lblDatabaseSetup.Name = "lblDatabaseSetup";
            this.lblDatabaseSetup.Size = new System.Drawing.Size(277, 30);
            this.lblDatabaseSetup.TabIndex = 3;
            this.lblDatabaseSetup.Text = "3. Database Setup";
            this.lblDatabaseSetup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Snow;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.lblGenerateNewToken);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.lblBotTokenSetup);
            this.panel3.Controls.Add(this.btnValidateToken);
            this.panel3.Controls.Add(this.txtBotToken);
            this.panel3.Location = new System.Drawing.Point(12, 219);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(285, 227);
            this.panel3.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(277, 114);
            this.label5.TabIndex = 7;
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGenerateNewToken
            // 
            this.lblGenerateNewToken.Font = new System.Drawing.Font("Segoe UI", 8.25F,
                System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblGenerateNewToken.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblGenerateNewToken.Location = new System.Drawing.Point(160, 147);
            this.lblGenerateNewToken.Name = "lblGenerateNewToken";
            this.lblGenerateNewToken.Size = new System.Drawing.Size(120, 20);
            this.lblGenerateNewToken.TabIndex = 8;
            this.lblGenerateNewToken.Text = "Generate New Token";
            this.lblGenerateNewToken.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGenerateNewToken.Click += new System.EventHandler(this.lblGenerateNewToken_Click);
            // 
            // lblBotTokenSetup
            // 
            this.lblBotTokenSetup.Font = new System.Drawing.Font("Segoe UI", 15.75F,
                ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblBotTokenSetup.Location = new System.Drawing.Point(3, 3);
            this.lblBotTokenSetup.Name = "lblBotTokenSetup";
            this.lblBotTokenSetup.Size = new System.Drawing.Size(277, 30);
            this.lblBotTokenSetup.TabIndex = 2;
            this.lblBotTokenSetup.Text = "2. Bot Token Setup";
            this.lblBotTokenSetup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msMenu
            // 
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.msMenu.Size = new System.Drawing.Size(891, 24);
            this.msMenu.TabIndex = 1;
            this.msMenu.Text = "menuStrip1";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.MintCream;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnInvite);
            this.panel4.Controls.Add(this.txtInviteUrl);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.txtClientID);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Location = new System.Drawing.Point(303, 335);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(285, 111);
            this.panel4.TabIndex = 1;
            // 
            // btnInvite
            // 
            this.btnInvite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInvite.Location = new System.Drawing.Point(183, 83);
            this.btnInvite.Name = "btnInvite";
            this.btnInvite.Size = new System.Drawing.Size(97, 24);
            this.btnInvite.TabIndex = 10;
            this.btnInvite.Text = "Invite to Guild";
            this.btnInvite.UseVisualStyleBackColor = true;
            this.btnInvite.Click += new System.EventHandler(this.btnInvite_Click);
            // 
            // txtInviteUrl
            // 
            this.txtInviteUrl.Location = new System.Drawing.Point(3, 84);
            this.txtInviteUrl.Name = "txtInviteUrl";
            this.txtInviteUrl.ReadOnly = true;
            this.txtInviteUrl.Size = new System.Drawing.Size(174, 22);
            this.txtInviteUrl.TabIndex = 9;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(3, 33);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(277, 20);
            this.label17.TabIndex = 18;
            this.label17.Text = "Head back to General Information in the Bot App.";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClientID
            // 
            this.txtClientID.Location = new System.Drawing.Point(3, 56);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.Size = new System.Drawing.Size(277, 22);
            this.txtClientID.TabIndex = 8;
            this.txtClientID.Text = "Paste Client ID Here";
            this.txtClientID.TextChanged += new System.EventHandler(this.txtClientID_TextChanged);
            this.txtClientID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClientID_KeyPress);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Segoe UI", 15.75F,
                ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label8.Location = new System.Drawing.Point(3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(277, 30);
            this.label8.TabIndex = 5;
            this.label8.Text = "4. Inviting the Bot";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.tslblBotValidation, this.toolStripStatusLabel7, this.tslblDbValidation, this.toolStripStatusLabel3,
                this.toolStripStatusLabel4, this.toolStripStatusLabel9, this.toolStripStatusLabel8,
                this.toolStripStatusLabel6, this.tslblDonate, this.toolStripStatusLabel2, this.tslblPatreon
            });
            this.statusStrip1.Location = new System.Drawing.Point(0, 449);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(891, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslblBotValidation
            // 
            this.tslblBotValidation.Name = "tslblBotValidation";
            this.tslblBotValidation.Size = new System.Drawing.Size(130, 17);
            this.tslblBotValidation.Text = "Bot Validation Required";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel7.Text = "|";
            // 
            // tslblDbValidation
            // 
            this.tslblDbValidation.Name = "tslblDbValidation";
            this.tslblDbValidation.Size = new System.Drawing.Size(160, 17);
            this.tslblDbValidation.Text = "Database Validation Required";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(46, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(171, 17);
            this.toolStripStatusLabel4.Text = "Developed by MythicalCuddles";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel9.Text = "|";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Font = new System.Drawing.Font("Segoe UI", 9F,
                System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.toolStripStatusLabel8.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolStripStatusLabel8.IsLink = true;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(144, 17);
            this.toolStripStatusLabel8.Text = "www.mythicalcuddles.xyz";
            this.toolStripStatusLabel8.Click += new System.EventHandler(this.tslblWebsite_Click);
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(46, 17);
            this.toolStripStatusLabel6.Spring = true;
            // 
            // tslblDonate
            // 
            this.tslblDonate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tslblDonate.ForeColor = System.Drawing.SystemColors.Highlight;
            this.tslblDonate.IsLink = true;
            this.tslblDonate.Name = "tslblDonate";
            this.tslblDonate.Size = new System.Drawing.Size(45, 17);
            this.tslblDonate.Text = "Donate";
            this.tslblDonate.Click += new System.EventHandler(this.tslblDonate_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // tslblPatreon
            // 
            this.tslblPatreon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tslblPatreon.ForeColor = System.Drawing.SystemColors.Highlight;
            this.tslblPatreon.IsLink = true;
            this.tslblPatreon.Name = "tslblPatreon";
            this.tslblPatreon.Size = new System.Drawing.Size(103, 17);
            this.tslblPatreon.Text = "Become a Patreon";
            this.tslblPatreon.Click += new System.EventHandler(this.tslblPatreon_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Beige;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label19);
            this.panel5.Controls.Add(this.label18);
            this.panel5.Controls.Add(this.txtOwnerId);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new System.Drawing.Point(594, 27);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(285, 147);
            this.panel5.TabIndex = 10;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(3, 97);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(277, 47);
            this.label19.TabIndex = 19;
            this.label19.Text = "You can change more settings when the bot is up and running. To view a full list " +
                                "of configurable settings, see the wiki.";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(3, 33);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(277, 36);
            this.label18.TabIndex = 18;
            this.label18.Text = "Discord ID: (Your ID to allow you to run owner commands - see wiki on how to find" +
                                " this)";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOwnerId
            // 
            this.txtOwnerId.Location = new System.Drawing.Point(3, 72);
            this.txtOwnerId.Name = "txtOwnerId";
            this.txtOwnerId.Size = new System.Drawing.Size(277, 22);
            this.txtOwnerId.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15.75F,
                ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label9.Location = new System.Drawing.Point(3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(277, 30);
            this.label9.TabIndex = 4;
            this.label9.Text = "5. Settings";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.MistyRose;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.btnCompleteSetup);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Location = new System.Drawing.Point(594, 180);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(285, 266);
            this.panel6.TabIndex = 9;
            // 
            // btnCompleteSetup
            // 
            this.btnCompleteSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompleteSetup.Location = new System.Drawing.Point(3, 236);
            this.btnCompleteSetup.Name = "btnCompleteSetup";
            this.btnCompleteSetup.Size = new System.Drawing.Size(277, 24);
            this.btnCompleteSetup.TabIndex = 19;
            this.btnCompleteSetup.Text = "Complete Setup";
            this.btnCompleteSetup.UseVisualStyleBackColor = true;
            this.btnCompleteSetup.Click += new System.EventHandler(this.btnCompleteSetup_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(277, 198);
            this.label6.TabIndex = 18;
            this.label6.Text = resources.GetString("label6.Text");
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Segoe UI", 15.75F,
                ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label13.Location = new System.Drawing.Point(3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(277, 30);
            this.label13.TabIndex = 2;
            this.label13.Text = "6. Licensing and Terms";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 471);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.msMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.msMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DiscordBot Configurator | Developed by MythicalCuddles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConfigure_FormClosing);
            this.Load += new System.EventHandler(this.frmConfigure_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtBotToken;
        private System.Windows.Forms.Button btnValidateToken;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblBotTokenSetup;
        private System.Windows.Forms.Label lblDatabaseSetup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblGenerateNewToken;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnValidateDbSettings;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.TextBox txtDbPass;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.TextBox txtDbPort;
        private System.Windows.Forms.TextBox txtDbHost;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnInvite;
        private System.Windows.Forms.TextBox txtInviteUrl;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtClientID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCompleteSetup;
        private System.Windows.Forms.ToolStripStatusLabel tslblBotValidation;
        private System.Windows.Forms.ToolStripStatusLabel tslblDbValidation;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel tslblDonate;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtOwnerId;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ToolStripStatusLabel tslblPatreon;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label lblDatabaseName;
    }
}