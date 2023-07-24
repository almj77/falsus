namespace Falsus.Providers.Location.Models
{
    using System;

    /// <summary>
    /// This class represents the pattern that is used to validate postal codes for a given country.
    /// </summary>
    internal class PostalCodePatternModel
    {
        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the Regular Expression pattern for the postal code
        /// of a given country.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a Regular Expression.
        /// </value>
        public string Pattern { get; set; }
    }
}
