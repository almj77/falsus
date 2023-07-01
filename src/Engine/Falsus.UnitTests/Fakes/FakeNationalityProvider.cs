namespace Falsus.UnitTests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers;
    using Falsus.Shared.Helpers;
    using Falsus.UnitTests.Fakes.Models;

    public class FakeNationalityProvider : GenericArrayProvider<FakeNationalityModel>
    {
        public const string CountryArgumentName = "country";

        private static FakeNationalityModel[] nationalities;

        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>()
            {
                { CountryArgumentName, typeof(FakeCountryModel) }
            };
        }

        public override FakeNationalityModel GetRowValue(string id)
        {
            return nationalities.FirstOrDefault(u => u.Demonym == id);
        }

        public override string GetValueId(FakeNationalityModel value)
        {
            return value.Demonym;
        }

        public override void Load(DataGeneratorProperty<FakeNationalityModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            nationalities = ResourceReader.ReadContentsFromFile<FakeNationalityModel[]>("Nationalities.json");
        }

        protected override FakeNationalityModel[] GetValues(DataGeneratorContext context)
        {
            if (context.HasArgumentValue(CountryArgumentName))
            {
                FakeCountryModel countryModel = context.GetArgumentValue<FakeCountryModel>(CountryArgumentName);
                if (countryModel != null && !string.IsNullOrEmpty(countryModel.Alpha2))
                {
                    FakeNationalityModel nationalityModel = nationalities.FirstOrDefault(u => u.CountryAlpha2 == countryModel.Alpha2);
                    if (nationalityModel != null)
                    {
                        return new FakeNationalityModel[1] { nationalityModel };
                    }
                }

                return Array.Empty<FakeNationalityModel>();
            }

            return this.GetValues();
        }

        protected override FakeNationalityModel[] GetValues()
        {
            return nationalities;
        }
    }
}
