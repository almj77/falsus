namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a geographic continent.
    /// </summary>
    public class ContinentModel : IEquatable<ContinentModel>, IComparable<ContinentModel>
    {
        /// <summary>
        /// Gets or sets the ISO Alpha2 Continent code.
        /// </summary>
        /// <value>
        /// A string containing the ISO Alpha2 Continent Code.
        /// </value>
        public string Alpha2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the Continent.
        /// </summary>
        /// <value>
        /// A string containing the continent name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="ContinentModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="ContinentModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="ContinentModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(ContinentModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Alpha2.CompareTo(other.Alpha2);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(ContinentModel other)
        {
            return other != null && this.Alpha2 == other.Alpha2;
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

            return this.Equals(obj as ContinentModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Alpha2.GetHashCode();
        }
    }
}
