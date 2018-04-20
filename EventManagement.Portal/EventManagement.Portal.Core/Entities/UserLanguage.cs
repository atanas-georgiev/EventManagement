// <copyright file="UserLanguage.cs" company="KPMG ITS">
// Copyright (c) KPMG ITS. All rights reserved.
// </copyright>

namespace KPMG.TaxRay.Portal.Core.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Holds information about user language
    /// </summary>
    [Table("UserLanguages")]
    public class UserLanguage
    {
        /// <summary>
        /// Gets or sets the id of the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the language.
        /// </summary>
        [MaxLength(100)]
        // [Index("UserLanguageDescription", IsUnique = true)]
        public string Description { get; set; }
    }
}
