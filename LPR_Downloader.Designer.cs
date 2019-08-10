namespace LPR_Downloader
{
    partial class frm_LPR_Downloader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_ManuallyUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_TimerMinutes = new System.Windows.Forms.TextBox();
            this.btn_TimerStart = new System.Windows.Forms.Button();
            this.lbl_NextDownload = new System.Windows.Forms.Label();
            this.btn_TimerStop = new System.Windows.Forms.Button();
            this.lbl_LastDownload = new System.Windows.Forms.Label();
            this.wb_OpenALPR = new System.Windows.Forms.WebBrowser();
            this.timer_Download = new System.Windows.Forms.Timer(this.components);
            this.dgv_ImportHistory = new System.Windows.Forms.DataGridView();
            this.tc_LPR_Downloader = new System.Windows.Forms.TabControl();
            this.tp_Main_Page = new System.Windows.Forms.TabPage();
            this.tp_Settings = new System.Windows.Forms.TabPage();
            this.btn_SettingsSave = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txt_pushUser = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_pushToken = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_alprPassword = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_alprUser = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_csvArchiveLocation = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_emailDefaultTo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_emailTestSend = new System.Windows.Forms.Button();
            this.chk_emailUseSSL = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_emailPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_emailSignIn = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_emailPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_emailServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_WebServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_SqlCon = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ImportHistory)).BeginInit();
            this.tc_LPR_Downloader.SuspendLayout();
            this.tp_Main_Page.SuspendLayout();
            this.tp_Settings.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_ManuallyUpload);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_TimerMinutes);
            this.groupBox1.Controls.Add(this.btn_TimerStart);
            this.groupBox1.Controls.Add(this.lbl_NextDownload);
            this.groupBox1.Controls.Add(this.btn_TimerStop);
            this.groupBox1.Controls.Add(this.lbl_LastDownload);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 234);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Imports";
            // 
            // btn_ManuallyUpload
            // 
            this.btn_ManuallyUpload.Location = new System.Drawing.Point(9, 202);
            this.btn_ManuallyUpload.Name = "btn_ManuallyUpload";
            this.btn_ManuallyUpload.Size = new System.Drawing.Size(131, 23);
            this.btn_ManuallyUpload.TabIndex = 20;
            this.btn_ManuallyUpload.Text = "Manually Upload File";
            this.btn_ManuallyUpload.UseVisualStyleBackColor = true;
            this.btn_ManuallyUpload.Click += new System.EventHandler(this.Btn_ManuallyUpload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Timer Frequency in Minutes";
            // 
            // txt_TimerMinutes
            // 
            this.txt_TimerMinutes.Location = new System.Drawing.Point(9, 32);
            this.txt_TimerMinutes.Name = "txt_TimerMinutes";
            this.txt_TimerMinutes.Size = new System.Drawing.Size(34, 20);
            this.txt_TimerMinutes.TabIndex = 6;
            this.txt_TimerMinutes.Text = "1";
            // 
            // btn_TimerStart
            // 
            this.btn_TimerStart.Location = new System.Drawing.Point(9, 58);
            this.btn_TimerStart.Name = "btn_TimerStart";
            this.btn_TimerStart.Size = new System.Drawing.Size(131, 23);
            this.btn_TimerStart.TabIndex = 8;
            this.btn_TimerStart.Text = "Start Auto Downloads";
            this.btn_TimerStart.UseVisualStyleBackColor = true;
            this.btn_TimerStart.Click += new System.EventHandler(this.Btn_TimerStart_Click);
            // 
            // lbl_NextDownload
            // 
            this.lbl_NextDownload.AutoSize = true;
            this.lbl_NextDownload.Location = new System.Drawing.Point(12, 130);
            this.lbl_NextDownload.Name = "lbl_NextDownload";
            this.lbl_NextDownload.Size = new System.Drawing.Size(0, 13);
            this.lbl_NextDownload.TabIndex = 11;
            // 
            // btn_TimerStop
            // 
            this.btn_TimerStop.Enabled = false;
            this.btn_TimerStop.Location = new System.Drawing.Point(9, 87);
            this.btn_TimerStop.Name = "btn_TimerStop";
            this.btn_TimerStop.Size = new System.Drawing.Size(131, 23);
            this.btn_TimerStop.TabIndex = 9;
            this.btn_TimerStop.Text = "Stop Auto Downloads";
            this.btn_TimerStop.UseVisualStyleBackColor = true;
            this.btn_TimerStop.Click += new System.EventHandler(this.Btn_TimerStop_Click);
            // 
            // lbl_LastDownload
            // 
            this.lbl_LastDownload.AutoSize = true;
            this.lbl_LastDownload.Location = new System.Drawing.Point(12, 117);
            this.lbl_LastDownload.Name = "lbl_LastDownload";
            this.lbl_LastDownload.Size = new System.Drawing.Size(96, 13);
            this.lbl_LastDownload.TabIndex = 10;
            this.lbl_LastDownload.Text = "Timer Not Running";
            // 
            // wb_OpenALPR
            // 
            this.wb_OpenALPR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wb_OpenALPR.Location = new System.Drawing.Point(6, 246);
            this.wb_OpenALPR.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb_OpenALPR.Name = "wb_OpenALPR";
            this.wb_OpenALPR.ScriptErrorsSuppressed = true;
            this.wb_OpenALPR.Size = new System.Drawing.Size(560, 176);
            this.wb_OpenALPR.TabIndex = 18;
            // 
            // timer_Download
            // 
            this.timer_Download.Tick += new System.EventHandler(this.Timer_Download_Tick);
            // 
            // dgv_ImportHistory
            // 
            this.dgv_ImportHistory.AllowUserToAddRows = false;
            this.dgv_ImportHistory.AllowUserToDeleteRows = false;
            this.dgv_ImportHistory.AllowUserToResizeRows = false;
            this.dgv_ImportHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_ImportHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ImportHistory.Location = new System.Drawing.Point(168, 6);
            this.dgv_ImportHistory.MultiSelect = false;
            this.dgv_ImportHistory.Name = "dgv_ImportHistory";
            this.dgv_ImportHistory.ReadOnly = true;
            this.dgv_ImportHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_ImportHistory.Size = new System.Drawing.Size(398, 234);
            this.dgv_ImportHistory.TabIndex = 26;
            // 
            // tc_LPR_Downloader
            // 
            this.tc_LPR_Downloader.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tc_LPR_Downloader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_LPR_Downloader.Controls.Add(this.tp_Main_Page);
            this.tc_LPR_Downloader.Controls.Add(this.tp_Settings);
            this.tc_LPR_Downloader.Location = new System.Drawing.Point(12, 12);
            this.tc_LPR_Downloader.Name = "tc_LPR_Downloader";
            this.tc_LPR_Downloader.SelectedIndex = 0;
            this.tc_LPR_Downloader.Size = new System.Drawing.Size(580, 454);
            this.tc_LPR_Downloader.TabIndex = 27;
            // 
            // tp_Main_Page
            // 
            this.tp_Main_Page.Controls.Add(this.groupBox1);
            this.tp_Main_Page.Controls.Add(this.dgv_ImportHistory);
            this.tp_Main_Page.Controls.Add(this.wb_OpenALPR);
            this.tp_Main_Page.Location = new System.Drawing.Point(4, 4);
            this.tp_Main_Page.Name = "tp_Main_Page";
            this.tp_Main_Page.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Main_Page.Size = new System.Drawing.Size(572, 428);
            this.tp_Main_Page.TabIndex = 0;
            this.tp_Main_Page.Text = "Main";
            this.tp_Main_Page.UseVisualStyleBackColor = true;
            // 
            // tp_Settings
            // 
            this.tp_Settings.Controls.Add(this.btn_SettingsSave);
            this.tp_Settings.Controls.Add(this.groupBox5);
            this.tp_Settings.Controls.Add(this.groupBox4);
            this.tp_Settings.Controls.Add(this.groupBox3);
            this.tp_Settings.Controls.Add(this.groupBox2);
            this.tp_Settings.Controls.Add(this.txt_WebServer);
            this.tp_Settings.Controls.Add(this.label3);
            this.tp_Settings.Controls.Add(this.txt_SqlCon);
            this.tp_Settings.Controls.Add(this.label2);
            this.tp_Settings.Location = new System.Drawing.Point(4, 4);
            this.tp_Settings.Name = "tp_Settings";
            this.tp_Settings.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Settings.Size = new System.Drawing.Size(590, 428);
            this.tp_Settings.TabIndex = 1;
            this.tp_Settings.Text = "Settings";
            this.tp_Settings.UseVisualStyleBackColor = true;
            // 
            // btn_SettingsSave
            // 
            this.btn_SettingsSave.Location = new System.Drawing.Point(10, 357);
            this.btn_SettingsSave.Name = "btn_SettingsSave";
            this.btn_SettingsSave.Size = new System.Drawing.Size(571, 60);
            this.btn_SettingsSave.TabIndex = 10;
            this.btn_SettingsSave.Text = "Save / Update Settings";
            this.btn_SettingsSave.UseVisualStyleBackColor = true;
            this.btn_SettingsSave.Click += new System.EventHandler(this.Btn_SettingsSave_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txt_pushUser);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.txt_pushToken);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Location = new System.Drawing.Point(312, 237);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(269, 114);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PushOver.Net Settings (Blank = No Push)";
            // 
            // txt_pushUser
            // 
            this.txt_pushUser.Location = new System.Drawing.Point(9, 83);
            this.txt_pushUser.Name = "txt_pushUser";
            this.txt_pushUser.Size = new System.Drawing.Size(254, 20);
            this.txt_pushUser.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "User";
            // 
            // txt_pushToken
            // 
            this.txt_pushToken.Location = new System.Drawing.Point(9, 33);
            this.txt_pushToken.Name = "txt_pushToken";
            this.txt_pushToken.Size = new System.Drawing.Size(254, 20);
            this.txt_pushToken.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 16);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Token";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_alprPassword);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.txt_alprUser);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(312, 51);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(269, 114);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "OpenALPR Settings";
            // 
            // txt_alprPassword
            // 
            this.txt_alprPassword.Location = new System.Drawing.Point(9, 83);
            this.txt_alprPassword.Name = "txt_alprPassword";
            this.txt_alprPassword.Size = new System.Drawing.Size(254, 20);
            this.txt_alprPassword.TabIndex = 7;
            this.txt_alprPassword.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Password";
            // 
            // txt_alprUser
            // 
            this.txt_alprUser.Location = new System.Drawing.Point(9, 33);
            this.txt_alprUser.Name = "txt_alprUser";
            this.txt_alprUser.Size = new System.Drawing.Size(254, 20);
            this.txt_alprUser.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Username";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_csvArchiveLocation);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(312, 171);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(269, 60);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CSV Save Settings";
            // 
            // txt_csvArchiveLocation
            // 
            this.txt_csvArchiveLocation.Location = new System.Drawing.Point(9, 33);
            this.txt_csvArchiveLocation.Name = "txt_csvArchiveLocation";
            this.txt_csvArchiveLocation.Size = new System.Drawing.Size(254, 20);
            this.txt_csvArchiveLocation.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(212, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "CSV Archive Location (Blank = No Archive)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_emailDefaultTo);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btn_emailTestSend);
            this.groupBox2.Controls.Add(this.chk_emailUseSSL);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txt_emailPassword);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txt_emailSignIn);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txt_emailPort);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txt_emailServer);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(10, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 271);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "E-mail Settings";
            // 
            // txt_emailDefaultTo
            // 
            this.txt_emailDefaultTo.Location = new System.Drawing.Point(10, 198);
            this.txt_emailDefaultTo.Name = "txt_emailDefaultTo";
            this.txt_emailDefaultTo.Size = new System.Drawing.Size(241, 20);
            this.txt_emailDefaultTo.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 182);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(248, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Default Send To Address (May be same as Sign-In)";
            // 
            // btn_emailTestSend
            // 
            this.btn_emailTestSend.Location = new System.Drawing.Point(10, 230);
            this.btn_emailTestSend.Name = "btn_emailTestSend";
            this.btn_emailTestSend.Size = new System.Drawing.Size(241, 30);
            this.btn_emailTestSend.TabIndex = 10;
            this.btn_emailTestSend.Text = "Test Email to Default Send To";
            this.btn_emailTestSend.UseVisualStyleBackColor = true;
            this.btn_emailTestSend.Click += new System.EventHandler(this.Btn_emailTestSend_Click);
            // 
            // chk_emailUseSSL
            // 
            this.chk_emailUseSSL.AutoSize = true;
            this.chk_emailUseSSL.Location = new System.Drawing.Point(219, 40);
            this.chk_emailUseSSL.Name = "chk_emailUseSSL";
            this.chk_emailUseSSL.Size = new System.Drawing.Size(15, 14);
            this.chk_emailUseSSL.TabIndex = 9;
            this.chk_emailUseSSL.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(202, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Use SSL";
            // 
            // txt_emailPassword
            // 
            this.txt_emailPassword.Location = new System.Drawing.Point(10, 139);
            this.txt_emailPassword.Name = "txt_emailPassword";
            this.txt_emailPassword.Size = new System.Drawing.Size(241, 20);
            this.txt_emailPassword.TabIndex = 7;
            this.txt_emailPassword.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Email Password";
            // 
            // txt_emailSignIn
            // 
            this.txt_emailSignIn.Location = new System.Drawing.Point(10, 88);
            this.txt_emailSignIn.Name = "txt_emailSignIn";
            this.txt_emailSignIn.Size = new System.Drawing.Size(241, 20);
            this.txt_emailSignIn.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Email Address";
            // 
            // txt_emailPort
            // 
            this.txt_emailPort.Location = new System.Drawing.Point(157, 37);
            this.txt_emailPort.Name = "txt_emailPort";
            this.txt_emailPort.Size = new System.Drawing.Size(34, 20);
            this.txt_emailPort.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(154, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Port";
            // 
            // txt_emailServer
            // 
            this.txt_emailServer.Location = new System.Drawing.Point(10, 37);
            this.txt_emailServer.Name = "txt_emailServer";
            this.txt_emailServer.Size = new System.Drawing.Size(127, 20);
            this.txt_emailServer.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Mail Server";
            // 
            // txt_WebServer
            // 
            this.txt_WebServer.Location = new System.Drawing.Point(312, 24);
            this.txt_WebServer.Name = "txt_WebServer";
            this.txt_WebServer.Size = new System.Drawing.Size(269, 20);
            this.txt_WebServer.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Local LPR Web Server";
            // 
            // txt_SqlCon
            // 
            this.txt_SqlCon.Location = new System.Drawing.Point(10, 24);
            this.txt_SqlCon.Multiline = true;
            this.txt_SqlCon.Name = "txt_SqlCon";
            this.txt_SqlCon.Size = new System.Drawing.Size(269, 49);
            this.txt_SqlCon.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "SQL Connection Information";
            // 
            // frm_LPR_Downloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 478);
            this.Controls.Add(this.tc_LPR_Downloader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frm_LPR_Downloader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "LPR Downloader";
            this.Load += new System.EventHandler(this.Frm_LPR_Downloader_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ImportHistory)).EndInit();
            this.tc_LPR_Downloader.ResumeLayout(false);
            this.tp_Main_Page.ResumeLayout(false);
            this.tp_Settings.ResumeLayout(false);
            this.tp_Settings.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_ManuallyUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_TimerMinutes;
        private System.Windows.Forms.Button btn_TimerStart;
        private System.Windows.Forms.Label lbl_NextDownload;
        private System.Windows.Forms.Button btn_TimerStop;
        private System.Windows.Forms.Label lbl_LastDownload;
        private System.Windows.Forms.WebBrowser wb_OpenALPR;
        private System.Windows.Forms.Timer timer_Download;
        private System.Windows.Forms.DataGridView dgv_ImportHistory;
        private System.Windows.Forms.TabControl tc_LPR_Downloader;
        private System.Windows.Forms.TabPage tp_Main_Page;
        private System.Windows.Forms.TabPage tp_Settings;
        private System.Windows.Forms.TextBox txt_WebServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_SqlCon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_emailPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_emailSignIn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_emailPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_emailServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_emailDefaultTo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_emailTestSend;
        private System.Windows.Forms.CheckBox chk_emailUseSSL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txt_csvArchiveLocation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txt_alprPassword;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_alprUser;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txt_pushUser;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_pushToken;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btn_SettingsSave;
    }
}

