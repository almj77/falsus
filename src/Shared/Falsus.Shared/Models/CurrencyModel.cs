namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a monetary currency.
    /// </summary>
    public class CurrencyModel : IEquatable<CurrencyModel>, IComparable<CurrencyModel>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the currency.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the currency identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the currency.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the name of the currency.
        /// </value>
        public string CurrencyName { get; set; }

        /// <summary>
        /// Gets or sets the currency display symbol.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the symbol of the currency.
        /// </value>
        public string Symbol { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="CurrencyModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="CurrencyModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="CurrencyModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(CurrencyModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return Id.CompareTo(other.Id);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CurrencyModel other)
        {
            return other != null && Id == other.Id;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as CurrencyModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
