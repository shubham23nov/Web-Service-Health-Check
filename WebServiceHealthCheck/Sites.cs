// -----------------------------------------------------------------------
// <copyright file="Sites.cs" company="Accenture Solutions Limited.">
// Copyright (c) Accenture. All rights reserved. THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <author>Shubham Jain</author>
// <email>shubham.r.jain@accenture.com</email>
// <date>29/06/2018</date>
// <summary> The Sites class. </summary>
//------------------------------------------------------------------------
namespace WebServiceHealthCheck
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion

    /// <summary>
    /// Sites class to maintain Service name and URL.
    /// </summary>
    public class Sites
    {
        #region Private Fields

        /// <summary>
        /// The URL of sites.
        /// </summary>
        private string urlData;

        /// <summary>
        /// The name of sites.
        /// </summary>
        private string siteName;

        /// <summary>
        /// The location of sites.
        /// </summary>
        private string location;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Sites class.
        /// </summary>
        /// <param name="name">The name of site.</param>
        /// <param name="url">The url of site.</param>
        public Sites(string name, string url, string location)
        {
            this.siteName = name;
            this.urlData = url;
            this.location = location;
        }

        /// <summary>
        /// Initializes a new instance of the Sites class.
        /// </summary>
        public Sites()
        {
            this.siteName = "Default Web Site";
            this.urlData = "*:80:";
            this.location = @"C:\inetpub\wwwroot";
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets URL of site.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get { return this.urlData; }
            set { this.urlData = value; }
        }

        /// <summary>
        /// Gets or sets name of site.
        /// </summary>
        /// <value>
        /// The SiteName.
        /// </value>
        public string SiteName
        {
            get { return this.siteName; }
            set { this.siteName = value; }
        }

        /// <summary>
        /// Gets or sets location of site.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
        #endregion
    }
}
