namespace Falsus.UnitTests.Fakes
{
    using System;
    using Falsus.GeneratorProperties;
    using Falsus.Providers;

    public class FakeGuidProvider : DataGeneratorProvider<Guid>
    {
        public override Guid GetRangedRowValue(Guid minValue, Guid maxValue, Guid[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FakeGuidProvider)} does not support ranged row values.");
        }

        public override Guid GetRowValue(DataGeneratorContext context, WeightedRange<Guid>[] excludedRanges, Guid[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FakeGuidProvider)} does not support ranged row values.");
        }

        public override Guid GetRowValue(string id)
        {
            return Guid.Parse(id);
        }

        public override Guid GetRowValue(DataGeneratorContext context, Guid[] excludedObjects)
        {
            byte[] bytes = new byte[16];
            this.Randomizer.NextBytes(bytes);
            return new Guid(bytes);
        }

        public override string GetValueId(Guid value)
        {
            return value.ToString();
        }
    }
}
