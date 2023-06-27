namespace Falsus.UnitTests.Fakes.Models
{
    using System;

    public class FakeCountryModel : IEquatable<FakeCountryModel>, IComparable<FakeCountryModel>
    {
        public string Name { get; set; }

        public string Alpha2 { get; set; }

        public int CompareTo(FakeCountryModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Alpha2.CompareTo(other.Alpha2);
        }

        public bool Equals(FakeCountryModel other)
        {
            return other != null && this.Alpha2 == other.Alpha2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Equals(obj as FakeCountryModel);
        }

        public override int GetHashCode()
        {
            return this.Alpha2.GetHashCode();
        }
    }
}
