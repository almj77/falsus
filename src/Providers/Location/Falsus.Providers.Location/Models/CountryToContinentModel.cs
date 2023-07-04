namespace Falsus.Providers.Location.Models
{
    using System;

    /// <summary>
    /// This class represents the association between a geographic Country and a geographic Continent.
    /// </summary>
    internal class CountryToContinentModel : IEquatable<CountryToContinentModel>, IComparable<CountryToContinentModel>
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

        /// <summary>
        /// Compares this instance with a specified <see cref="CountryToContinentModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="CountryToContinentModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="CountryToContinentModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(CountryToContinentModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Concat(this.CountryAlpha2, ",", this.ContinentAlpha2).CompareTo(string.Concat(other.CountryAlpha2, ",", other.ContinentAlpha2));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CountryToContinentModel other)
        {
            return other != null && this.CountryAlpha2 == other.CountryAlpha2 && this.ContinentAlpha2 == other.ContinentAlpha2;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Equals(obj as CountryToContinentModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(this.CountryAlpha2, ",", this.ContinentAlpha2).GetHashCode();
        }
    }
}
