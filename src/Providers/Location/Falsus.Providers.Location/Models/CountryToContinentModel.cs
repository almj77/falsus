namespace Falsus.Providers.Location.Models
{
    /// <summary>
    /// This class represents the association between a geographic Country and a geographic Continent.
    /// </summary>
    internal class CountryToContinentModel
    {
        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the ISO Alpha2 Continent code.
        /// </summary>
        /// <value>
        /// A string containing the ISO Alpha2 Continent Code.
        /// </value>
        public string ContinentAlpha2 { get; set; }
    }
}
