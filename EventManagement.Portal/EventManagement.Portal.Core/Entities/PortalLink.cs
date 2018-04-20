// <copyright file="PortalLink.cs" company="KPMG ITS">
// Copyright (c) KPMG ITS. All rights reserved.
// </copyright>

namespace KPMG.TaxRay.Portal.Core.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Holds information about links in the application
    /// </summary>
    [Table("PortalLinks")]
    public class PortalLink
    {
        /// <summary>
        /// Gets or sets entity's id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the link is restricted to administrator.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public virtual UserLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        [MaxLength(100)]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }
    }
}