using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WebServiceHealthCheck
{
    public partial class ServiceBrowse : Form
    {

        #region Fields

        string NRT = string.Empty;
        string STUB = string.Empty;
        string PSG = string.Empty;
        string BLPP = string.Empty;
        string DC = string.Empty;
        string BizTalkServices = string.Empty;
        string NBSServices = string.Empty;
        string IBMMQServices = string.Empty;
        string serviceSelected = string.Empty;

        #endregion

        BindingList<Sites> data = new BindingList<Sites>();
        #region public methods
        // sJ
        public ServiceBrowse()
        {
            this.Load += new System.EventHandler(ServiceBrowse_Load);
            // btnBrowse.Click += new System.EventHandler(dataGridView1_CellContentClick);
        }

        // sJ
        public void PopulateBinding(DataGridView dataGridView1, BindingSource bindingSource1)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                string filePath = @"C:\Windows\System32\inetsrv\config\applicationHost.config";
                xDoc.Load(filePath);
                XmlNodeList site = xDoc.GetElementsByTagName("site");
                int i = xDoc.GetElementsByTagName("site").Count;
                string name = string.Empty;
                string uri = string.Empty;
                Sites url = new Sites(name, uri);

                for (int a = 1; a < i; a++)
                {
                    if (!string.IsNullOrEmpty(xDoc.GetElementsByTagName("site")[a].ToString()))
                    {
                        if (Convert.ToString(xDoc.GetElementsByTagName("binding")[a].Attributes["protocol"].Value) == "http" || Convert.ToString(xDoc.GetElementsByTagName("binding")[a].Attributes["protocol"].Value) == "https")
                        {
                            url.SiteName = xDoc.GetElementsByTagName("site")[a].Attributes["name"].Value;
                            url.Binding = site[a].ChildNodes[1].ChildNodes[0].Attributes["bindingInformation"].Value;
                            data.Add(new Sites(url.SiteName, url.Binding));
                        }
                    }
                }

                // Populate the data source.
                //bindingSource1.Add(new Sites(url.SiteName, url.Binding));

                // Initialize the DataGridView.
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.AutoSize = true;
                dataGridView1.DataSource = data;

                //dataGridView1.DataSource = new BindingSource(bindingSource1, url.SiteName);

                dataGridView1.Columns.Add(CreateCheckBox());

                // Initialize and add a text box column.
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "SiteName";
                column.Name = "Service";
                column.Width = 200;
                dataGridView1.Columns.Add(column);

                // Initialize and add a check box column.
                column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Binding";
                column.Name = "URL";
                column.Width = 250;
                dataGridView1.Columns.Add(column);

                // Initialize the form.
                this.Controls.Add(dataGridView1);
                this.AutoSize = true;
                this.Text = "DataGridView object binding demo";
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());

            }
        }
        #endregion

        #region private methods
        // sJ
        private void ServiceBrowse_Load(object sender, System.EventArgs e)
        {
            DataGridView dataGridView2 = new DataGridView();
            BindingSource bindingSource1 = new BindingSource();
            // this.button1_Click(sender, e);
            this.Browse_Button(sender, e);
            this.PopulateBinding(dataGridView2, bindingSource1);

        }

        /// <summary>
        /// Select/ Deselect all check boxes on select/ deselect of the header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  sj = trying to make header as checkbox - not in use currently
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).CheckState.ToString() == "Checked")
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["Check"].Value = 1;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["Check"].Value = 0;
                }
            }
            lblEnvironment.Select();
        }

        //DataGridViewHeaderCell CreateHeaderCheckBox()
        //{
        //    DataGridViewCheckBoxColumn headerCheck = new DataGridViewCheckBoxColumn();
        //    check.HeaderText = "Select";
        //    check.Width = 69;
        //    return check;
        //}

        /// <summary>
        /// Start the selected services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // sJ - Code to browse.. not in use currently
        private void dataGridView1_CellContentClick(object sender, EventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            var eArgs = (DataGridViewCellEventArgs)e;
            if (senderGrid.Columns[eArgs.ColumnIndex] is DataGridViewButtonColumn && eArgs.RowIndex >= 0)
            {
                System.Diagnostics.Process.Start("IExplore.exe");
                System.Diagnostics.Process.Start(Convert.ToString(dataGridView1.Rows[0].Cells[0].Value));
            }
        }

        //sJ
        //private void selectedservices()
        //{
        //    if (dataGridView1.Columns[0].Selected == true)
        //    {
        //        this.browseService();
        //    }
        //}

        // sJ
        private void Browse_Button(object sender, EventArgs e)
        {
            Button folderButton = new Button();
            folderButton.Width = 175;
            folderButton.Height = 25;
            folderButton.Location = new Point { X = 600, Y = 17 };

            folderButton.ForeColor = Color.Black;
            folderButton.Text = "Browse";
            this.Controls.Add(folderButton);
            MessageBox.Show("inside the browse_Button");
        }
        #endregion

        DataGridViewCheckBoxColumn CreateCheckBox()
        {
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.HeaderText = "Select";
            check.Width = 69;
            return check;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    button1.Width = 175;
        //    button1.Height = 25;
        //    button1.Visible = true;
        //    button1.ForeColor = System.Drawing.Color.Blue;

        //}
    }
}
