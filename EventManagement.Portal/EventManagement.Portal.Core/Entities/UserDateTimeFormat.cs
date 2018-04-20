// <copyright file="UserDateTimeFormat.cs" company="KPMG ITS">
// Copyright (c) KPMG ITS. All rights reserved.
// </copyright>

namespace KPMG.TaxRay.Portal.Core.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Holds information about user date time format
    /// </summary>
    [Table("UserDateTimeFormats")]
    public class UserDateTimeFormat
    {
        /// <summary>
        /// Gets or sets the id of the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets description of the format.
        /// </summary>
        [MaxLength(100)]
        // [Index("UserDateTimeFormatDescription", IsUnique = true)]
        public string Description { get; set; }
    }
}
