namespace Falsus.UnitTests.Fakes.Models
{
    using System;

    public class FakeNationalityModel : IEquatable<FakeNationalityModel>, IComparable<FakeNationalityModel>
    {
        public string CountryAlpha2 { get; set; }

        public string Demonym { get; set; }

        public int CompareTo(FakeNationalityModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Demonym.CompareTo(other.Demonym);
        }

        public bool Equals(FakeNationalityModel other)
        {
            return other != null && this.CountryAlpha2 == other.CountryAlpha2;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as FakeNationalityModel);
        }

        public override int GetHashCode()
        {
            return this.Demonym.GetHashCode();
        }
    }
}

