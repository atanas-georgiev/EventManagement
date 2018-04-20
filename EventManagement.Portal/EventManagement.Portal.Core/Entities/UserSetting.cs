// <copyright file="UserSettings.cs" company="KPMG ITS">
// Copyright (c) KPMG ITS. All rights reserved.
// </copyright>

namespace KPMG.TaxRay.Portal.Core.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Holds information about user settings.
    /// </summary>
    [Table("UserSettings")]
    public class UserSetting
    {
        /// <summary>
        /// Gets or sets entity's id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [MaxLength(200)]
        //[Index("UniqueUserName", IsUnique = true)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the id of selected language.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets language.
        /// </summary>
        public UserLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the id of selected date time format.
        /// </summary>
        public int DateTimeFormatId { get; set; }

        /// <summary>
        /// Gets or sets the date time format.
        /// </summary>
        public UserDateTimeFormat DateTimeFormat { get; set; }
    }
}
