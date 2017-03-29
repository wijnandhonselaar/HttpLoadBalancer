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
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblMethod = new System.Windows.Forms.Label();
            this.lblMethodSummary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // BalanceMethod
            // 
            this.BalanceMethod.AllowDrop = true;
            this.BalanceMethod.FormattingEnabled = true;
            this.BalanceMethod.Location = new System.Drawing.Point(322, 96);
            this.BalanceMethod.Name = "BalanceMethod";
            this.BalanceMethod.Size = new System.Drawing.Size(121, 21);
            this.BalanceMethod.TabIndex = 0;
            // 
            // btnToggleLoadBalancer
            // 
            this.btnToggleLoadBalancer.Location = new System.Drawing.Point(322, 67);
            this.btnToggleLoadBalancer.Name = "btnToggleLoadBalancer";
            this.btnToggleLoadBalancer.Size = new System.Drawing.Size(75, 23);
            this.btnToggleLoadBalancer.TabIndex = 1;
            this.btnToggleLoadBalancer.Text = "Start";
            this.btnToggleLoadBalancer.UseVisualStyleBackColor = true;
            this.btnToggleLoadBalancer.Click += new System.EventHandler(this.btnToggleLoadBalancer_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(322, 41);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            // 
            // lstServers
            // 
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point(12, 41);
            this.lstServers.Name = "lstServers";
            this.lstServers.Size = new System.Drawing.Size(230, 316);
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
            this.lblPort.Location = new System.Drawing.Point(287, 43);
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
            this.lblMethod.Location = new System.Drawing.Point(270, 99);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(46, 13);
            this.lblMethod.TabIndex = 7;
            this.lblMethod.Text = "Method:";
            this.lblMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMethodSummary
            // 
            this.lblMethodSummary.AutoSize = true;
            this.lblMethodSummary.Location = new System.Drawing.Point(273, 214);
            this.lblMethodSummary.MaximumSize = new System.Drawing.Size(200, 200);
            this.lblMethodSummary.Name = "lblMethodSummary";
            this.lblMethodSummary.Size = new System.Drawing.Size(0, 13);
            this.lblMethodSummary.TabIndex = 8;
            // 
            // Gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 369);
            this.Controls.Add(this.lblMethodSummary);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstServers);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btnToggleLoadBalancer);
            this.Controls.Add(this.BalanceMethod);
            this.Name = "Gui";
            this.Text = "Gui";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox BalanceMethod;
        private System.Windows.Forms.Button btnToggleLoadBalancer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblMethod;
        public System.Windows.Forms.NumericUpDown numericUpDown1;
        public System.Windows.Forms.ListBox lstServers;
        public System.Windows.Forms.Label lblMethodSummary;
    }
}

