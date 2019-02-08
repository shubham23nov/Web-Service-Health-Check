// -----------------------------------------------------------------------
// <copyright file="ServiceState.cs" company="Avanade Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WebServiceHealthCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceProcess;
    using System.Windows.Forms;

    /// <summary>
    /// To maintain the state of a service
    /// </summary>
    public class ServiceState
    {
        //// public string ServiceName { get; set; }

        /// <summary>
        /// Events.
        /// </summary>
        public string Events { get; set; }

        /// <summary>
        /// Server.
        /// </summary>
        public string Server { get; set; }
    }
}
