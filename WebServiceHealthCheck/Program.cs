// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Accenture Solutions Limited.">
// Copyright (c) Accenture. All rights reserved. THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <author>Shubham Jain</author>
// <email>shubham.r.jain@accenture.com</email>
// <date>29/06/2018</date>
// <summary> The Program class. </summary>
//------------------------------------------------------------------------

namespace WebServiceHealthCheck
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The class with main entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServiceCenter());
        }
    }
}
