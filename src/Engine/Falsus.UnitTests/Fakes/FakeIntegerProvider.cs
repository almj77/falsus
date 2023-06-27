namespace Falsus.UnitTests.Fakes
{
    using Falsus.GeneratorProperties;
    using Falsus.Providers;

    public class FakeIntegerProvider : DataGeneratorProvider<int>
    {
        private readonly long datasetLength;

        private readonly int maxAttempts;

        public FakeIntegerProvider()
            : base()
        {
            this.datasetLength = Math.BigMul(int.MaxValue, 2);
            this.maxAttempts = Convert.ToInt32(this.datasetLength * 0.001);
        }

        public override int GetRangedRowValue(int minValue, int maxValue, int[] excludedObjects)
        {
            int value = this.Randomizer.Next(minValue, maxValue);
            if (excludedObjects.Any() && excludedObjects.All(u => u >= minValue && u <= maxValue) && excludedObjects.Length == maxValue - minValue)
            {
                throw new InvalidOperationException($"{nameof(FakeIntegerProvider)} cannot generate another unique value.");
            }

            int attemptCount = 0;
            while (excludedObjects.Contains(value))
            {
                value = this.Randomizer.Next(minValue, maxValue);
                attemptCount++;

                if (attemptCount == this.maxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(FakeIntegerProvider)} was unable to generate unique value after {attemptCount} attempts.");
                }
            }

            return value;
        }

        public override int GetRowValue(DataGeneratorContext context, WeightedRange<int>[] excludedRanges, int[] excludedObjects)
        {
            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(int MinValue, int MaxValue)> ranges = new List<(int MinValue, int MaxValue)>();
                WeightedRange<int>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > int.MinValue)
                    {
                        ranges.Add((int.MinValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < int.MaxValue)
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, int.MaxValue));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(FakeIntegerProvider)} was unable to find a range to generate values.");
                }

                (int MinValue, int MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];

                while (ranges.Count > 1 && excludedObjects.Any() && excludedObjects.Count(u => u > randomRange.MinValue && u < randomRange.MaxValue) >= randomRange.MaxValue - randomRange.MinValue - 1)
                {
                    ranges.Remove(randomRange);
                    randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(FakeIntegerProvider)} was unable to find a range to generate values.");
                }

                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        public override int GetRowValue(string id)
        {
            return int.Parse(id);
        }

        public override int GetRowValue(DataGeneratorContext context, int[] excludedObjects)
        {
            if (excludedObjects.Length == this.datasetLength)
            {
                throw new InvalidOperationException($"{nameof(FakeIntegerProvider)} cannot generate another unique value.");
            }

            int value = this.Randomizer.Next(int.MinValue, int.MaxValue);
            if (excludedObjects.Any())
            {
                int attemptCount = 0;
                while (excludedObjects.Contains(value))
                {
                    value = this.Randomizer.Next(int.MinValue, int.MaxValue);
                    attemptCount++;

                    if (attemptCount == this.maxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(FakeIntegerProvider)} was unable to generate unique value after {attemptCount} attempts.");
                    }
                }
            }

            return value;
        }

        public override string GetValueId(int value)
        {
            return value.ToString();
        }
    }
}