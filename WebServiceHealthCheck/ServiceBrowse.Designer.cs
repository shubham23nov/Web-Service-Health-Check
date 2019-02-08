namespace SampleWindowsForms
{
    partial class ServiceBrowse
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
            this.SuspendLayout();
            // 
            // ServiceBrowse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 545);
            this.Name = "ServiceBrowse";
            this.Text = "ServiceBrowse";
            this.Load += new System.EventHandler(this.ServiceBrowse_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.ComboBox cmbEnvironments;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblNoServiceFound;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox Error;
        private System.Windows.Forms.CheckBox Warning;
        private System.Windows.Forms.ToolTip tooltipRefresh;
        private System.Windows.Forms.Label lblHC;
        private System.Windows.Forms.PictureBox imgHC;
        private System.Windows.Forms.Label lblDashboard;



    }
}