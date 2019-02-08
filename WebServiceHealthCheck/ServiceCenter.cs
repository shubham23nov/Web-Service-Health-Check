// -----------------------------------------------------------------------
// <copyright file="ServiceCenter.cs" company="Accenture Solutions Limited.">
// Copyright (c) Accenture. All rights reserved. THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <author>Shubham Jain</author>
// <email>shubham.r.jain@accenture.com</email>
// <date>29/06/2018</date>
// <summary> The ServiceCenter class. </summary>
//------------------------------------------------------------------------

namespace WebServiceHealthCheck
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;
    using Microsoft.Web.Administration;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The class for operations in Windows form.
    /// </summary>
    public partial class ServiceCenter : Form
    {
        #region Private Fields

        /// <summary>
        /// List of the Sites.
        /// </summary>
        private BindingList<Sites> data = new BindingList<Sites>();

        /// <summary>
        /// Header Check box.
        /// </summary>
        private CheckBox headerCheckBox = new CheckBox();

        #endregion

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the ServiceCenter class.
        /// </summary>
        public ServiceCenter()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Data Source of site.
        /// </summary>
        /// <value>
        /// The DataSource.
        /// </value>
        public BindingList<Sites> Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        /// <summary>
        /// Gets or sets Header Check Box.
        /// </summary>
        /// <value>
        /// The Header Check Box.
        /// </value>
        public CheckBox HeaderCheckBox
        {
            get { return this.headerCheckBox; }
            set { this.headerCheckBox = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Populating the Data Grid using Data Source.
        /// </summary>
        /// <param name="dataGridView1">The DataGridView class.</param>
        /// <param name="bindingSource1">The BindingSource class.</param>
        public void PopulateBinding(DataGridView dataGridView1, BindingSource bindingSource1)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string filePath = @"C:\Windows\System32\inetsrv\config\applicationHost.config";
                xmlDoc.Load(filePath);
                XmlNodeList site = xmlDoc.GetElementsByTagName("site");
                int i = xmlDoc.GetElementsByTagName("site").Count;
                string name = string.Empty;
                string uri = string.Empty;
                string location = string.Empty;
                Sites serv = new Sites(name, uri, location);

                for (int a = 1; a < i; a++)
                {
                    if (!string.IsNullOrEmpty(xmlDoc.GetElementsByTagName("site")[a].ToString()))
                    {
                        string protocol = Convert.ToString(site[a].ChildNodes[1].ChildNodes[0].Attributes["protocol"].Value);
                        string siteURL = Regex.Replace(site[a].ChildNodes[1].ChildNodes[0].Attributes["bindingInformation"].Value, "[^a-zA-Z.]+", string.Empty, RegexOptions.Compiled);
                        if ((protocol == "http" || protocol == "https") && !string.IsNullOrEmpty(Regex.Replace(siteURL, "[^a-zA-Z.]+", string.Empty, RegexOptions.Compiled)))
                        {
                            serv.SiteName = xmlDoc.GetElementsByTagName("site")[a].Attributes["name"].Value;
                            string port = Regex.Replace(Convert.ToString(site[a].ChildNodes[1].ChildNodes[0].Attributes["bindingInformation"].Value), "[^0-9]+", string.Empty, RegexOptions.Compiled);
                            serv.Url = protocol + "://" + siteURL + ":" + port;
                            serv.Location = xmlDoc.GetElementsByTagName("site")[a].ChildNodes[0].ChildNodes[0].Attributes["physicalPath"].Value;
                            this.Data.Add(new Sites(serv.SiteName, serv.Url, serv.Location));
                        }
                    }
                }

                dataGridView1.Columns.Add(this.CreateStatusColumn());
                
                dataGridView1.DataSource = this.Data;
                dataGridView1.Columns["SiteName"].Width = 199;
                dataGridView1.Columns["SiteName"].Resizable = DataGridViewTriState.False;

                dataGridView1.Columns["Url"].Width = 250;
                dataGridView1.Columns["Url"].Resizable = DataGridViewTriState.False;

                dataGridView1.Columns["Location"].Visible = false;

                // Find the Location of Header Cell.
                Point headerCellLocation = this.dataGridView1.GetCellDisplayRectangle(0, -1, true).Location;

                // Place the Header CheckBox in the Location of the Header Cell.
                this.HeaderCheckBox.Location = new Point(headerCellLocation.X + 26, headerCellLocation.Y + 4);
                this.HeaderCheckBox.BackColor = Color.White;
                this.HeaderCheckBox.Size = new Size(17, 17);

                // Assign Click event to the Header CheckBox.
                this.HeaderCheckBox.Click += new EventHandler(this.HeaderCheckBox_Clicked);
                dataGridView1.Controls.Add(this.HeaderCheckBox);
                dataGridView1.Columns.Insert(0, this.CreateCheckBox());

                for (int k = 0; k <= dataGridView1.Rows.Count - 1; k++)
                {
                    string serviceName = dataGridView1.Rows[k].Cells["SiteName"].Value.ToString();

                    using (ServerManager sm = new ServerManager())
                    {
                        Site st = sm.Sites.Where(t => t.Name.Equals(serviceName)).FirstOrDefault();
                        if (st.State == ObjectState.Started || st.State == ObjectState.Starting)
                        {
                            dataGridView1.Rows[k].Cells["Status"].Value = st.State;
                            dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Green;
                        }
                        else
                        {
                            dataGridView1.Rows[k].Cells["Status"].Value = st.State;
                            dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Red;
                        }
                    }
                }

                dataGridView1.CellContentClick += new DataGridViewCellEventHandler(this.DataGridView_CellClick);

                // Initialize the form.
                this.Controls.Add(this.dataGridView1);
                this.AutoSize = false;
                this.Text = "DataGridView object binding demo";
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        /// <summary>
        /// Creating check box.
        /// </summary>
        /// <returns>DataGridViewCheckBoxColumn class</returns>
        public DataGridViewCheckBoxColumn CreateCheckBox()
        {
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.HeaderText = string.Empty;
            check.Name = "Select";
            check.Width = 69;
            check.Visible = true;
            return check;
        }

        /// <summary>
        /// Creating status column.
        /// </summary>
        /// <returns>DataGridViewTextBoxColumn class</returns>
        public DataGridViewTextBoxColumn CreateStatusColumn()
        {
            DataGridViewTextBoxColumn status = new DataGridViewTextBoxColumn();
            status.HeaderText = "Status";
            status.Name = "Status";
            status.Width = 85;
            status.Visible = true;
            return status;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Event Handler for Header Check box for Check/Uncheck all.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            // Necessary to end the edit mode of the Cell.
            this.dataGridView1.EndEdit();

            //// toolTipInfo.SetToolTip(dataGridView1.Columns[0].HeaderCell, "Starts the selected Web services");

            if (((CheckBox)sender).CheckState.ToString() == "Checked")
            {
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    // row.Cells["Check"].Value = 1;
                    DataGridViewCheckBoxCell checkbox = row.Cells["Select"] as DataGridViewCheckBoxCell;
                    checkbox.Value = 1;
                }
            }
            else
            {
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell checkbox = row.Cells["Select"] as DataGridViewCheckBoxCell;
                    checkbox.Value = 0;
                }
            }
        }

        /// <summary>
        /// Event Handler for row selection on check box.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                // Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;

                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Select"].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }

                this.HeaderCheckBox.Checked = isChecked;
            }
        }

        /// <summary>
        /// Event Handler for Browsing all selected Sites.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void Browse_Click(object sender, EventArgs e)
        {
            int siteSelected = 0;
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                {
                    Process.Start(Convert.ToString(dataGridView1.Rows[i].Cells["URL"].Value));
                    siteSelected += 1;
                    ////if (Convert.ToInt32(getResponse(Convert.ToString(dataGridView1.Rows[i].Cells[1].Value))) != 200)
                    ////{
                    ////    MessageBox.Show("Response Failed");
                    ////}
                }
            }

            if (siteSelected == 0)
            {
                MessageBox.Show("Please select some service(s)");
            }
        }

        /// <summary>
        /// Retrieving response of the selected sites.
        /// </summary>
        ////public HttpStatusCode getResponse(string p)
        ////{
        ////    HttpStatusCode result = default(HttpStatusCode);
        ////    try
        ////    {                
        ////        var request = HttpWebRequest.Create(p);
        ////        request.Method = "HEAD";
        ////        using (var response = request.GetResponse() as HttpWebResponse)
        ////        {
        ////            if (response != null)
        ////            {
        ////                result = response.StatusCode;
        ////                response.Close();
        ////            }
        ////        }
        ////    }
        ////    catch
        ////    {
        ////        result = HttpStatusCode.BadRequest;
        ////    }
        ////    return result;
        ////}

        /// <summary>
        /// Event Handler for loading the form with data grid and buttons.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void ServiceCenter_Load(object sender, EventArgs e)
        {
            BindingSource bindingSource1 = new BindingSource();
            this.PopulateBinding(this.dataGridView1, bindingSource1);

            // Tool Tip Info for function of all buttons.
            this.toolTipInfo.SetToolTip(this.start, "Starts the selected Web services");
            this.toolTipInfo.SetToolTip(this.stop, "Stops the selected Web services");
            this.toolTipInfo.SetToolTip(this.stopAll, "Stops all the Web services. No need to select anything");
            this.toolTipInfo.SetToolTip(this.restartAll, "Starts all the Web services. No need to select anything");
            this.toolTipInfo.SetToolTip(this.browse, "Browses all the selected Web services.");
        }

        /// <summary>
        /// Event Handler for Stopping all selected Sites.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                int siteSelected = 0;
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                    {
                        string serviceName = Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value);

                        using (ServerManager sm = new ServerManager())
                        {
                            Site site = sm.Sites.Where(t => t.Name.Equals(serviceName)).FirstOrDefault();
                            if (site == null)
                            {
                                MessageBox.Show(Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value) + "not selected correctly.");
                            }
                            else
                            {
                                if (site.State == ObjectState.Started || site.State == ObjectState.Starting)
                                {
                                    site.Stop();
                                    dataGridView1.Rows[i].Cells["Status"].Value = site.State;
                                    dataGridView1.Rows[i].Cells["Status"].Style.BackColor = Color.Red;
                                    siteSelected += 1;
                                }
                                else if (site.State == ObjectState.Stopped || site.State == ObjectState.Stopping)
                                {
                                    MessageBox.Show(Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value) + " is already stopped");
                                    siteSelected = 5001;
                                }
                            }
                        }
                    }
                }

                if (siteSelected == 0)
                {
                    MessageBox.Show("Please select some service(s)");
                }

                if (siteSelected != 0 && (siteSelected != 5001))
                {
                    MessageBox.Show("Sites selected are stopped.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while stopping service(s)" + "\n\n" + ex);
            }
        }

        /// <summary>
        /// Event Handler for performing IIS Reset.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void Restart_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iisreset");
            
            System.Threading.Thread.Sleep(12000);

            for (int k = 0; k <= dataGridView1.Rows.Count - 1; k++)
            {
                if (dataGridView1.Rows[k].Cells["SiteName"].Value != null)
                {
                    string serviceName = dataGridView1.Rows[k].Cells["SiteName"].Value.ToString();

                    using (ServerManager sm = new ServerManager())
                    {
                        Site st = sm.Sites.Where(t => t.Name.Equals(serviceName)).FirstOrDefault();
                        dataGridView1.Rows[k].Cells["Status"].Value = st.State;
                        if ((st.State == ObjectState.Started) || (st.State == ObjectState.Starting))
                        {
                            dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Green;
                        }
                        else if ((st.State == ObjectState.Stopped) || (st.State == ObjectState.Stopping))
                        {
                            dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Red;
                        }
                    }
                }
            }

            MessageBox.Show("All Web Services have been started.");
        }

        /// <summary>
        /// Event Handler for Starting all selected Sites.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void Start_Click(object sender, EventArgs e)
        {
            try
            {
                int siteSelected = 0;
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                    {
                        string serviceName = Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value);

                        //ServerManager 7 = ServerManager.OpenRemote();
                        //7.
                        using (ServerManager sm = new ServerManager())
                        {
                            Site site = sm.Sites.Where(t => t.Name.Equals(serviceName)).FirstOrDefault();
                            if (site == null)
                            {
                                MessageBox.Show(Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value) + " : not selected correctly.");
                            }
                            else
                            {
                                if (site.State == ObjectState.Stopped)
                                {
                                    site.Start();
                                    dataGridView1.Rows[i].Cells["Status"].Value = site.State;
                                    dataGridView1.Rows[i].Cells["Status"].Style.BackColor = Color.Green;
                                    siteSelected += 1;
                                }
                                else if (site.State == ObjectState.Started || site.State == ObjectState.Starting)
                                {
                                    MessageBox.Show(Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value) + " is already started");
                                    siteSelected = 5001;
                                }
                            }
                        }
                    }
                }

                if (siteSelected == 0)
                {
                    MessageBox.Show("Please select some service(s)");
                }

                if (siteSelected != 0 && siteSelected != 5001)
                {
                    MessageBox.Show("Sites selected have been started.");
                }
            }
            catch (ServerManagerException sEx)
            {
                MessageBox.Show("Worldwide Web service is stopped. Please start it." + "\n\n" + sEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while Starting service(s)" + "\n\n" + ex);
            }
        }

        /// <summary>
        /// Event Handler for stopping all Sites.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void IIS_Stop_Click(object sender, EventArgs e)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = "W3SVC";
            sc.Stop();
            for (int k = 0; k <= dataGridView1.Rows.Count - 1; k++)
            {
                if (dataGridView1.Rows[k].Cells["SiteName"].Value != null)
                {
                    string serviceName = dataGridView1.Rows[k].Cells["SiteName"].Value.ToString();

                    using (ServerManager sm = new ServerManager())
                    {
                        Site st = sm.Sites.Where(t => t.Name.Equals(serviceName)).FirstOrDefault();
                        dataGridView1.Rows[k].Cells["Status"].Value = st.State;
                        dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Red;
                    }
                }
            }

            MessageBox.Show("All Web Services have been stopped");
        }

        /// <summary>
        /// Event Handler for clearing the selection.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void ClearSelection_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].HeaderCell.Value = false;
            dataGridView1.Refresh();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["Select"].Value = false;
            }
        }

        /// <summary>
        /// Event Handler for Refreshing the status of all web services.
        /// </summary>
        /// <param name="sender">The Object class.</param>
        /// <param name="e">The EventArgs class.</param>
        private void Refresh_Click(object sender, EventArgs e)
        {
            for (int k = 0; k <= dataGridView1.Rows.Count - 1; k++)
            {
                if (dataGridView1.Rows[k].Cells["SiteName"].Value != null)
                {
                    string serviceName = dataGridView1.Rows[k].Cells["SiteName"].Value.ToString();

                    using (ServerManager sm = new ServerManager())
                    {
                        Site st = sm.Sites.Where(t => t.Name.Equals(serviceName)).FirstOrDefault();
                        if (st.State == ObjectState.Started || st.State == ObjectState.Starting)
                        {
                            dataGridView1.Rows[k].Cells["Status"].Value = st.State;
                            dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Green;
                        }
                        else
                        {
                            dataGridView1.Rows[k].Cells["Status"].Value = st.State;
                            dataGridView1.Rows[k].Cells["Status"].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }        

        #endregion

        private void Explore_Click(object sender, EventArgs e)
        {
            int siteSelected = 0;
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                {
                    if (Directory.Exists(Convert.ToString(dataGridView1.Rows[i].Cells["Location"].Value)))
                    {
                        Process.Start("explorer.exe", Convert.ToString(dataGridView1.Rows[i].Cells["Location"].Value));
                        siteSelected += 1;
                    }
                    else
                    {
                        MessageBox.Show("File path does not exist for site : " + Convert.ToString(dataGridView1.Rows[i].Cells["SiteName"].Value));
                        siteSelected = 5001;
                    }
                } 
            }
            if (siteSelected == 0)
            {
                MessageBox.Show("Please select some service(s)");
            }
        }
    }
}
