namespace HttpLoadBalancer.View
{
    partial class Gui
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
            this.BalanceMethod = new System.Windows.Forms.ComboBox();
            this.btnToggleLoadBalancer = new System.Windows.Forms.Button();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblMethod = new System.Windows.Forms.Label();
            this.lblMethodSummary = new System.Windows.Forms.Label();
            this.btnRemoveServer = new System.Windows.Forms.Button();
            this.txtServerAdrress = new System.Windows.Forms.TextBox();
            this.lblSelectedServer = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.numServerPort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.HealthMonitors = new System.Windows.Forms.ComboBox();
            this.Address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstServersView = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).BeginInit();
            this.SuspendLayout();
            // 
            // BalanceMethod
            // 
            this.BalanceMethod.AllowDrop = true;
            this.BalanceMethod.FormattingEnabled = true;
            this.BalanceMethod.Location = new System.Drawing.Point(498, 96);
            this.BalanceMethod.Name = "BalanceMethod";
            this.BalanceMethod.Size = new System.Drawing.Size(121, 21);
            this.BalanceMethod.TabIndex = 0;
            // 
            // btnToggleLoadBalancer
            // 
            this.btnToggleLoadBalancer.Location = new System.Drawing.Point(498, 67);
            this.btnToggleLoadBalancer.Name = "btnToggleLoadBalancer";
            this.btnToggleLoadBalancer.Size = new System.Drawing.Size(75, 23);
            this.btnToggleLoadBalancer.TabIndex = 1;
            this.btnToggleLoadBalancer.Text = "Start";
            this.btnToggleLoadBalancer.UseVisualStyleBackColor = true;
            this.btnToggleLoadBalancer.Click += new System.EventHandler(this.btnToggleLoadBalancer_Click);
            // 
            // lstServers
            // 
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point(12, 41);
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size(398, 264);
            this.lstServers.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server list:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(463, 43);
            this.lblPort.Name = "lblPort";
            this.lblPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 5;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(446, 99);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(46, 13);
            this.lblMethod.TabIndex = 7;
            this.lblMethod.Text = "Method:";
            this.lblMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMethodSummary
            // 
            this.lblMethodSummary.AutoSize = true;
            this.lblMethodSummary.Location = new System.Drawing.Point(449, 214);
            this.lblMethodSummary.MaximumSize = new System.Drawing.Size(200, 200);
            this.lblMethodSummary.Name = "lblMethodSummary";
            this.lblMethodSummary.Size = new System.Drawing.Size(0, 13);
            this.lblMethodSummary.TabIndex = 8;
            // 
            // btnRemoveServer
            // 
            this.btnRemoveServer.Location = new System.Drawing.Point(335, 311);
            this.btnRemoveServer.Name = "btnRemoveServer";
            this.btnRemoveServer.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveServer.TabIndex = 9;
            this.btnRemoveServer.Text = "Remove";
            this.btnRemoveServer.UseVisualStyleBackColor = true;
            this.btnRemoveServer.Click += new System.EventHandler(this.btnRemoveServer_Click);
            // 
            // txtServerAdrress
            // 
            this.txtServerAdrress.Location = new System.Drawing.Point(498, 154);
            this.txtServerAdrress.Name = "txtServerAdrress";
            this.txtServerAdrress.Size = new System.Drawing.Size(120, 20);
            this.txtServerAdrress.TabIndex = 10;
            // 
            // lblSelectedServer
            // 
            this.lblSelectedServer.AutoSize = true;
            this.lblSelectedServer.Location = new System.Drawing.Point(15, 321);
            this.lblSelectedServer.Name = "lblSelectedServer";
            this.lblSelectedServer.Size = new System.Drawing.Size(0, 13);
            this.lblSelectedServer.TabIndex = 14;
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(498, 41);
            this.numPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(120, 20);
            this.numPort.TabIndex = 2;
            this.numPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // numServerPort
            // 
            this.numServerPort.Location = new System.Drawing.Point(498, 180);
            this.numServerPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numServerPort.Name = "numServerPort";
            this.numServerPort.Size = new System.Drawing.Size(120, 20);
            this.numServerPort.TabIndex = 15;
            this.numServerPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(444, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Address:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 182);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Port:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(498, 209);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(75, 23);
            this.btnAddServer.TabIndex = 18;
            this.btnAddServer.Text = "Add Server";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(416, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "HealthMonitors:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // HealthMonitors
            // 
            this.HealthMonitors.AllowDrop = true;
            this.HealthMonitors.FormattingEnabled = true;
            this.HealthMonitors.Location = new System.Drawing.Point(498, 123);
            this.HealthMonitors.Name = "HealthMonitors";
            this.HealthMonitors.Size = new System.Drawing.Size(121, 21);
            this.HealthMonitors.TabIndex = 19;
            this.HealthMonitors.SelectedIndexChanged += new System.EventHandler(this.HealthMonitor_SelectedIndexChanged);
            // 
            // Address
            // 
            this.Address.Text = "Address";
            // 
            // Port
            // 
            this.Port.Text = "Port";
            // 
            // Status
            // 
            this.Status.Text = "Status";
            // 
            // lstServersView
            // 
            this.lstServersView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Address,
            this.Port,
            this.Status});
            this.lstServersView.GridLines = true;
            this.lstServersView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstServersView.Location = new System.Drawing.Point(12, 338);
            this.lstServersView.MultiSelect = false;
            this.lstServersView.Name = "lstServersView";
            this.lstServersView.Size = new System.Drawing.Size(398, 234);
            this.lstServersView.TabIndex = 21;
            this.lstServersView.UseCompatibleStateImageBehavior = false;
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 764);
            this.Controls.Add(this.lstServersView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.HealthMonitors);
            this.Controls.Add(this.btnAddServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numServerPort);
            this.Controls.Add(this.lblSelectedServer);
            this.Controls.Add(this.txtServerAdrress);
            this.Controls.Add(this.btnRemoveServer);
            this.Controls.Add(this.lblMethodSummary);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstServers);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.btnToggleLoadBalancer);
            this.Controls.Add(this.BalanceMethod);
            this.Name = "Gui";
            this.Text = "Gui";
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox BalanceMethod;
        private System.Windows.Forms.Button btnToggleLoadBalancer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblMethod;
        public System.Windows.Forms.ListBox lstServers;
        public System.Windows.Forms.Label lblMethodSummary;
        private System.Windows.Forms.Button btnRemoveServer;
        private System.Windows.Forms.Label lblSelectedServer;
        public System.Windows.Forms.NumericUpDown numPort;
        public System.Windows.Forms.NumericUpDown numServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddServer;
        public System.Windows.Forms.TextBox txtServerAdrress;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox HealthMonitors;
        private System.Windows.Forms.ColumnHeader Address;
        private System.Windows.Forms.ColumnHeader Port;
        private System.Windows.Forms.ColumnHeader Status;
        public System.Windows.Forms.ListView lstServersView;
    }
}

