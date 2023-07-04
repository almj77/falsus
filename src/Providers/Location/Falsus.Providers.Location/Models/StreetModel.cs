namespace Falsus.Shared.Models
{
    /// <summary>
    /// This class represents a street with the associated coordinates.
    /// </summary>
    internal class StreetModel
    {
        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the street name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the point represented by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the latitude value.
        /// </value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the point represented by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the longitude value.
        /// </value>
        public string Longitude { get; set; }
    }
}
