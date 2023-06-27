namespace Falsus.UnitTests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers;

    public class FakeEmailProvider : DataGeneratorProvider<string>
    {
        public const string TextArgumentsName = "text";

        private FakeEmailProviderConfiguration configuration;

        public FakeEmailProvider(FakeEmailProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>()
            {
                { TextArgumentsName, typeof(string) }
            };
        }

        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FakeEmailProvider)} does not support ranged row values.");
        }

        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FakeEmailProvider)} does not support ranged row values.");
        }

        public override string GetRowValue(string id)
        {
            return id;
        }

        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string value = string.Empty;
            string domain = string.Empty;

            if (context.HasArgumentValue(TextArgumentsName))
            {
                string[] texts = context.GetArgumentValues(TextArgumentsName).Select(u => u.ToString()).ToArray();
                value = string.Join(".", texts).ToLowerInvariant();
                value = System.Text.RegularExpressions.Regex.Replace(value, @"[^\w]+", ".");
                if (value.EndsWith("."))
                {
                    value = value.Substring(0, value.Length - 1);
                }
            }

            domain = this.GetDomain().ToLower();

            int equalCount = excludedObjects.Count(u => u == string.Concat(value, "@", domain));

            if (equalCount > 0)
            {
                return string.Concat(value, equalCount + 1, "@", domain);
            }

            return string.Concat(value, "@", domain);
        }

        public override string GetValueId(string value)
        {
            return value;
        }

        public override void Load(DataGeneratorProperty<string> property, int rowCount)
        {
            base.Load(property, rowCount);

            this.ValidateConfiguration();
        }

        private string GetDomain()
        {
            if (this.configuration.Domains.Length == 1)
            {
                return this.configuration.Domains.First();
            }
            else
            {
                return this.configuration.Domains[this.Randomizer.Next(0, this.configuration.Domains.Length)];
            }
        }

        private void ValidateConfiguration()
        {
            if (this.configuration.Domains == null || !this.configuration.Domains.Any())
            {
                throw new InvalidOperationException($"{nameof(FakeEmailProvider)} requires at least one domain.");
            }
        }
    }
}