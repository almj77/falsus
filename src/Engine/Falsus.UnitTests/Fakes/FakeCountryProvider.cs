namespace Falsus.UnitTests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers;
    using Falsus.UnitTests.Fakes.Models;
    using Falsus.UnitTests.Helpers;

    public class FakeCountryProvider : GenericArrayProvider<FakeCountryModel>
    {
        public const string NationalityArgumentName = "nationality";

        private static FakeCountryModel[] countries;

        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>()
            {
                { NationalityArgumentName, typeof(FakeNationalityModel) }
            };
        }

        public override FakeCountryModel GetRowValue(string id)
        {
            return countries.FirstOrDefault(u => u.Alpha2 == id);
        }

        public override string GetValueId(FakeCountryModel value)
        {
            return value.Alpha2;
        }

        public override void Load(DataGeneratorProperty<FakeCountryModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            countries = ResourceReader.ReadContentsFromFile<FakeCountryModel[]>("Countries.json");
        }

        protected override FakeCountryModel[] GetValues(DataGeneratorContext context)
        {
            if (context.HasArgumentValue(NationalityArgumentName))
            {
                FakeNationalityModel nationalityModel = context.GetArgumentValue<FakeNationalityModel>(NationalityArgumentName);
                if (nationalityModel != null && !string.IsNullOrEmpty(nationalityModel.CountryAlpha2))
                {
                    FakeCountryModel countryModel = countries.FirstOrDefault(u => u.Alpha2 == nationalityModel.CountryAlpha2);
                    if (countryModel != null)
                    {
                        return new FakeCountryModel[1] { countryModel };
                    }
                }

                return Array.Empty<FakeCountryModel>();
            }

            return this.GetValues();
        }

        protected override FakeCountryModel[] GetValues()
        {
            return countries;
        }
    }
}
