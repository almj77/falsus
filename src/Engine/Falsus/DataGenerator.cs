namespace Falsus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Falsus.Configuration;
    using Falsus.GeneratorProperties;
    using Falsus.Providers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents the point of entry to the data generation process,
    /// providing methods to configure properties and providers.
    /// </summary>
    public class DataGenerator
    {
        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        private Random randomizer;

        /// <summary>
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// </summary>
        private int? seed;

        /// <summary>
        /// A dictionary of <see cref="IDataGeneratorProperty"/> indexed by
        /// the <see cref="IDataGeneratorProperty.Id"/> property.
        /// </summary>
        private Dictionary<string, IDataGeneratorProperty> properties;

        /// <summary>
        /// A dictionary of <see cref="object"/> collections indexed by
        /// the <see cref="IDataGeneratorProperty.Id"/> property.
        /// Contains a list of pre-generated values, used when a
        /// given property has weighted or ranged value generation.
        /// </summary>
        private Dictionary<string, List<object>> preGeneratedValues;

        /// <summary>
        /// A dictionary of <see cref="object"/> collections indexed by
        /// the <see cref="IDataGeneratorProperty.Id"/> property.
        /// Contains the list of <see cref="object"/>s that have been
        /// generated and that should be excluded from the possible
        /// values in order to maintain the dataset unique.
        /// </summary>
        private Dictionary<string, List<object>> objectsToExclude;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGenerator"/> class.
        /// </summary>
        public DataGenerator()
        {
            this.properties = new Dictionary<string, IDataGeneratorProperty>();
            this.preGeneratedValues = new Dictionary<string, List<object>>();
            this.objectsToExclude = new Dictionary<string, List<object>>();
            this.randomizer = new Random();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGenerator"/> class.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public DataGenerator(int seed)
            : this()
        {
            this.seed = seed;
            this.randomizer = new Random(seed);
        }

        /// <summary>
        /// Occurs before a property is loaded.
        /// </summary>
        public event EventHandler<IDataGeneratorProperty> PropertyLoad;

        /// <summary>
        /// Occurs after a property has been loaded.
        /// </summary>
        public event EventHandler<IDataGeneratorProperty> PropertyLoaded;

        /// <summary>
        /// Occurs after a value is generated for a property.
        /// </summary>
        public event EventHandler<(IDataGeneratorProperty Property, object Value)> ValueGenerated;

        /// <summary>
        /// Occurs after an entire row has been generated.
        /// </summary>
        public event EventHandler<Dictionary<string, object>> RowGenerated;

        /// <summary>
        /// Registers a new property for data generation.
        /// </summary>
        /// <typeparam name="T">The type of the value that the property will hold.</typeparam>
        /// <param name="propertyId">The unique identifier for this property.</param>
        /// <returns>This instance.</returns>
        public DataGeneratorProperty<T> WithProperty<T>(string propertyId)
        {
            if (this.properties.ContainsKey(propertyId))
            {
                throw new InvalidOperationException($"Property with id {propertyId} alread exists.");
            }

            DataGeneratorProperty<T> property = new DataGeneratorProperty<T>(propertyId);

            this.properties.Add(propertyId, property);

            return property;
        }

        /// <summary>
        /// Registers a new weighted property for data generation.
        /// </summary>
        /// <typeparam name="T">The type of the value that the property will hold.</typeparam>
        /// <param name="propertyId">The unique identifier for this property.</param>
        /// <returns>This instance.</returns>
        public WeightedDataGeneratorProperty<T> WithWeightedProperty<T>(string propertyId)
        {
            if (this.properties.ContainsKey(propertyId))
            {
                throw new InvalidOperationException($"Property with id {propertyId} alread exists.");
            }

            WeightedDataGeneratorProperty<T> property = new WeightedDataGeneratorProperty<T>(propertyId);

            this.properties.Add(propertyId, property);

            return property;
        }

        /// <summary>
        /// Registers a new ranged property for data generation.
        /// </summary>
        /// <typeparam name="T">The type of the value that the property will hold.</typeparam>
        /// <param name="propertyId">The unique identifier for this property.</param>
        /// <returns>This instance.</returns>
        public RangedDataGeneratorProperty<T> WithRangedProperty<T>(string propertyId)
            where T : IComparable<T>
        {
            if (this.properties.ContainsKey(propertyId))
            {
                throw new InvalidOperationException($"Property with id {propertyId} alread exists.");
            }

            RangedDataGeneratorProperty<T> property = new RangedDataGeneratorProperty<T>(propertyId);

            this.properties.Add(propertyId, property);

            return property;
        }

        /// <summary>
        /// Configures this instance based on the values from a single class, instead of having
        /// to call any other methods.
        /// </summary>
        /// <param name="configuration">
        /// An instance of <see cref="DataGeneratorConfiguration"/> defining the configuration for this class.
        /// </param>
        /// <returns>This instance.</returns>
        public DataGenerator Configure(DataGeneratorConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configuration.Seed.HasValue)
            {
                this.seed = configuration.Seed.Value;
                this.randomizer = new Random(configuration.Seed.Value);
            }

            if (configuration.Properties != null && configuration.Properties.Any())
            {
                foreach (DataGeneratorConfigurationProperty configurationProperty in configuration.Properties)
                {
                    if (configurationProperty == null)
                    {
                        throw new InvalidOperationException("Configuration property cannot be null.");
                    }

                    if (configurationProperty.WeightedRanges != null && configurationProperty.WeightedRanges.Any()
                        && configurationProperty.Weights != null && configurationProperty.Weights.Any())
                    {
                        throw new InvalidOperationException($"Property '{configurationProperty.Id}' cannot set both {nameof(configurationProperty.WeightedRanges)} and {nameof(configurationProperty.Weights)}.");
                    }

                    if (string.IsNullOrEmpty(configurationProperty.ValueType))
                    {
                        throw new InvalidOperationException($"Value type of configuration property '{configurationProperty.Id}' is required.");
                    }

                    Type valueType = Type.GetType(configurationProperty.ValueType);

                    if (valueType == null)
                    {
                        throw new InvalidOperationException($"Value type '{configurationProperty.ValueType}' could not be found.");
                    }

                    if (string.IsNullOrEmpty(configurationProperty.Provider))
                    {
                        throw new InvalidOperationException($"Provider type of configuration property '{configurationProperty.Id}' is required.");
                    }

                    Type providerType = Type.GetType(configurationProperty.Provider);

                    if (providerType == null)
                    {
                        throw new InvalidOperationException($"Provider type '{configurationProperty.ValueType}' could not be found.");
                    }

                    IDataGeneratorProvider provider = default;

                    if (!string.IsNullOrEmpty(configurationProperty.ProviderConfiguration))
                    {
                        var isArray = JToken.Parse(configurationProperty.ProviderConfiguration) is JArray;

                        foreach (ConstructorInfo ctor in providerType.GetConstructors())
                        {
                            ParameterInfo[] parameters = ctor.GetParameters();

                            if (parameters != null && parameters.Any())
                            {
                                if (parameters.Length > 1 && isArray)
                                {
                                    string[] argArr = JsonConvert.DeserializeObject<string[]>(configurationProperty.ProviderConfiguration);
                                    object[] ctorArgs = new object[parameters.Length];
                                    for (int i = 0; i < parameters.Length; i++)
                                    {
                                        ctorArgs[i] = JsonConvert.DeserializeObject(argArr[i], parameters[i].ParameterType);
                                    }

                                    provider = (IDataGeneratorProvider)Activator.CreateInstance(providerType, ctorArgs);
                                    break;
                                }
                                else
                                {
                                    object ctorArg = JsonConvert.DeserializeObject(configurationProperty.ProviderConfiguration, parameters[0].ParameterType);
                                    if (ctorArg != null)
                                    {
                                        provider = (IDataGeneratorProvider)Activator.CreateInstance(providerType, ctorArg);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        provider = (IDataGeneratorProvider)Activator.CreateInstance(providerType);
                    }

                    if (configurationProperty.WeightedRanges != null && configurationProperty.WeightedRanges.Any())
                    {
                        Type generatorPropertyType = typeof(RangedDataGeneratorProperty<>);
                        Type propertyType = generatorPropertyType.MakeGenericType(valueType);
                        Type genericWeightedRange = typeof(WeightedRange<>);
                        Type weightedRangeType = genericWeightedRange.MakeGenericType(valueType);

                        Array weightedRanges = Array.CreateInstance(weightedRangeType, configurationProperty.WeightedRanges.Length);
                        for (int i = 0; i < configurationProperty.WeightedRanges.Length; i++)
                        {
                            WeightedRange<string> item = configurationProperty.WeightedRanges[i];
                            object range = Activator.CreateInstance(weightedRangeType);

                            object minValue = JsonConvert.DeserializeObject(item.MinValue, valueType);
                            object maxValue = JsonConvert.DeserializeObject(item.MaxValue, valueType);

                            weightedRangeType.InvokeMember(
                                nameof(WeightedRange.MinValue),
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                Type.DefaultBinder,
                                range,
                                new object[1] { minValue });

                            weightedRangeType.InvokeMember(
                                nameof(WeightedRange.MaxValue),
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                Type.DefaultBinder,
                                range,
                                new object[1] { maxValue });

                            weightedRangeType.InvokeMember(
                                nameof(WeightedRange.Weight),
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                Type.DefaultBinder,
                                range,
                                new object[1] { item.Weight });

                            ((WeightedRange)range).MinValue = minValue;
                            ((WeightedRange)range).MaxValue = maxValue;
                            ((WeightedRange)range).Weight = item.Weight;

                            weightedRanges.SetValue(range, i);
                        }

                        IRangedDataGeneratorProperty propertyInstance = (IRangedDataGeneratorProperty)Activator.CreateInstance(
                            propertyType,
                            configurationProperty.Id,
                            configurationProperty.IsUnique,
                            configurationProperty.IsNotNull,
                            provider,
                            weightedRanges);

                        this.properties.Add(configurationProperty.Id, propertyInstance);
                    }
                    else if (configurationProperty.Weights != null && configurationProperty.Weights.Any())
                    {
                        Type generatorPropertyType = typeof(WeightedDataGeneratorProperty<>);
                        Type propertyType = generatorPropertyType.MakeGenericType(valueType);

                        List<(float Weight, object Value)> weights = new List<(float Weight, object Value)>();
                        foreach ((float Weight, string Value) item in configurationProperty.Weights)
                        {
                            object value = JsonConvert.DeserializeObject(item.Value, valueType);
                            weights.Add((item.Weight, value));
                        }

                        IWeightedDataGeneratorProperty propertyInstance = (IWeightedDataGeneratorProperty)Activator.CreateInstance(
                            propertyType,
                            configurationProperty.Id,
                            configurationProperty.IsUnique,
                            configurationProperty.IsNotNull,
                            provider,
                            weights.ToArray());

                        this.properties.Add(configurationProperty.Id, propertyInstance);
                    }
                    else
                    {
                        Type generatorPropertyType = typeof(DataGeneratorProperty<>);
                        Type propertyType = generatorPropertyType.MakeGenericType(valueType);

                        IDataGeneratorProperty propertyInstance = (IDataGeneratorProperty)Activator.CreateInstance(
                            propertyType,
                            configurationProperty.Id,
                            configurationProperty.IsUnique,
                            configurationProperty.IsNotNull,
                            provider);

                        this.properties.Add(configurationProperty.Id, propertyInstance);
                    }
                }

                foreach (DataGeneratorConfigurationProperty configurationProperty in configuration.Properties)
                {
                    if (configurationProperty.Arguments != null && configurationProperty.Arguments.Any())
                    {
                        foreach (KeyValuePair<string, string[]> item in configurationProperty.Arguments)
                        {
                            for (int i = 0; i < item.Value.Length; i++)
                            {
                                if (!this.properties.ContainsKey(item.Value[i]))
                                {
                                    throw new InvalidOperationException($"Property '{item.Value[i]}' is not configured.");
                                }

                                this.properties[configurationProperty.Id].WithArgument(item.Key, this.properties[item.Value[i]]);
                            }
                        }
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Generates <paramref name="rowCount"/> rows with data for all of the
        /// specified properties according to the specified constraints.
        /// </summary>
        /// <param name="rowCount">The total number of rows to generate.</param>
        /// <returns>
        /// An array of <see cref="Dictionary{TKey, TValue}"/>. Each dictionary
        /// represents a row, containing the unique identifier of the property
        /// as the key and the <see cref="object"/> value.
        /// </returns>
        public Dictionary<string, object>[] Generate(int rowCount)
        {
            this.ValidateProperties();

            Dictionary<string, IDataGeneratorProperty> sortedDictionary = this.GetSortedProperties();
            Dictionary<string, object>[] dataset = new Dictionary<string, object>[rowCount];

            foreach (KeyValuePair<string, IDataGeneratorProperty> propertyItem in sortedDictionary)
            {
                this.PropertyLoad?.Invoke(this, propertyItem.Value);

                if (this.seed.HasValue)
                {
                    propertyItem.Value.Provider.InitializeRandomizer(this.seed.Value);
                }
                else
                {
                    propertyItem.Value.Provider.InitializeRandomizer();
                }

                propertyItem.Value.Provider.Load(propertyItem.Value, rowCount);

                this.PropertyLoaded?.Invoke(this, propertyItem.Value);
            }

            foreach (KeyValuePair<string, IDataGeneratorProperty> propertyItem in sortedDictionary)
            {
                IDataGeneratorProperty property = propertyItem.Value;

                this.preGeneratedValues.Add(propertyItem.Key, new List<object>());
                this.objectsToExclude.Add(propertyItem.Key, new List<object>());

                if (property.GetType().GetGenericTypeDefinition() == typeof(WeightedDataGeneratorProperty<>))
                {
                    IWeightedDataGeneratorProperty weightedProperty = (IWeightedDataGeneratorProperty)property;

                    foreach ((float Weight, object Value) item in weightedProperty.Weights.OrderByDescending(u => u.Weight))
                    {
                        int itemCount = Convert.ToInt32(Math.Ceiling(rowCount * item.Weight));

                        while (this.preGeneratedValues[propertyItem.Key].Count + itemCount > rowCount)
                        {
                            itemCount--;
                        }

                        string itemId = weightedProperty.Provider.GetValueId(item.Value);
                        object value = weightedProperty.Provider.GetRowValue(itemId);

                        if (item.Value != null && value == null)
                        {
                            throw new InvalidOperationException($"Provider '{weightedProperty.Provider.GetType().Name}' does not contain an object with identifier '{itemId}'.");
                        }

                        for (int i = 0; i < itemCount; i++)
                        {
                            this.ValueGenerated?.Invoke(this, (propertyItem.Value, value));

                            this.preGeneratedValues[propertyItem.Key].Add(value);
                        }

                        this.objectsToExclude[propertyItem.Key].Add(value);
                    }

                    while (this.preGeneratedValues[propertyItem.Key].Count < rowCount)
                    {
                        this.preGeneratedValues[propertyItem.Key].Add(null);
                    }

                    this.preGeneratedValues[propertyItem.Key] = this.preGeneratedValues[propertyItem.Key].OrderBy<object, int>(u => this.randomizer.Next()).ToList();
                }
                else if (property.GetType().GetGenericTypeDefinition() == typeof(RangedDataGeneratorProperty<>))
                {
                    IRangedDataGeneratorProperty rangedProperty = (IRangedDataGeneratorProperty)property;

                    foreach (WeightedRange item in rangedProperty.WeightedRanges.OrderByDescending(u => u.Weight))
                    {
                        int itemCount = Convert.ToInt32(Math.Ceiling(rowCount * item.Weight));

                        while (this.preGeneratedValues[propertyItem.Key].Count + itemCount > rowCount)
                        {
                            itemCount--;
                        }

                        for (int i = 0; i < itemCount; i++)
                        {
                            object value = rangedProperty.Provider.GetRangedRowValue(
                                item.MinValue,
                                item.MaxValue,
                                rangedProperty.IsUnique ? this.objectsToExclude[propertyItem.Key].ToArray() : Array.Empty<object>());

                            this.preGeneratedValues[propertyItem.Key].Add(value);
                            this.objectsToExclude[propertyItem.Key].Add(value);

                            this.ValueGenerated?.Invoke(this, (propertyItem.Value, value));
                        }

                        this.objectsToExclude[propertyItem.Key].Clear();
                    }

                    while (this.preGeneratedValues[propertyItem.Key].Count < rowCount)
                    {
                        this.preGeneratedValues[propertyItem.Key].Add(null);
                    }

                    this.preGeneratedValues[propertyItem.Key] = this.preGeneratedValues[propertyItem.Key].OrderBy<object, int>(u => this.randomizer.Next()).ToList();
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                dataset[i] = new Dictionary<string, object>();

                foreach (KeyValuePair<string, IDataGeneratorProperty> propertyItem in sortedDictionary)
                {
                    DataGeneratorContext context = new DataGeneratorContext(dataset[i], i, rowCount, propertyItem.Value, propertyItem.Value.Arguments);

                    IDataGeneratorProperty property = propertyItem.Value;
                    object generatedValue = null;

                    bool isWeightedProperty = propertyItem.Value.GetType().GetGenericTypeDefinition() == typeof(WeightedDataGeneratorProperty<>)
                        && ((IWeightedDataGeneratorProperty)propertyItem.Value).Weights.Any();

                    bool isRangedProperty = propertyItem.Value.GetType().GetGenericTypeDefinition() == typeof(RangedDataGeneratorProperty<>)
                        && ((IRangedDataGeneratorProperty)propertyItem.Value).WeightedRanges.Any();

                    if (isWeightedProperty && this.preGeneratedValues[propertyItem.Key] != null)
                    {
                        generatedValue = this.preGeneratedValues[propertyItem.Key][i];

                        if (generatedValue == null)
                        {
                            IWeightedDataGeneratorProperty weightedProperty = (IWeightedDataGeneratorProperty)propertyItem.Value;
                            generatedValue = property.Provider.GetRowValue(context, weightedProperty.Weights.Select(u => u.Value).ToArray());
                        }
                    }
                    else if (isRangedProperty && this.preGeneratedValues[propertyItem.Key] != null)
                    {
                        generatedValue = this.preGeneratedValues[propertyItem.Key][i];

                        if (generatedValue == null)
                        {
                            IRangedDataGeneratorProperty rangedProperty = (IRangedDataGeneratorProperty)propertyItem.Value;
                            generatedValue = property.Provider.GetRowValue(context, rangedProperty.WeightedRanges.ToArray(), this.objectsToExclude[propertyItem.Key].ToArray());
                        }
                    }
                    else if ((isWeightedProperty || property.IsUnique) && this.objectsToExclude.ContainsKey(propertyItem.Key))
                    {
                        generatedValue = property.Provider.GetRowValue(context, this.objectsToExclude[propertyItem.Key].ToArray());
                    }
                    else
                    {
                        generatedValue = property.Provider.GetRowValue(context, Array.Empty<object>());
                    }

                    if (!propertyItem.Value.AllowNull && generatedValue == null)
                    {
                        throw new InvalidOperationException($"Provider '{propertyItem.Value.Provider.GetType().Name}' generated a null value for non-null property '{propertyItem.Key}'.");
                    }

                    if (propertyItem.Value.IsUnique && this.objectsToExclude.ContainsKey(propertyItem.Key) && this.objectsToExclude[propertyItem.Key].Contains(generatedValue))
                    {
                        throw new InvalidOperationException($"Provider '{propertyItem.Value.Provider.GetType().Name}' generated a duplicate value for unique valued property '{propertyItem.Key}'.");
                    }

                    if ((isWeightedProperty || property.IsUnique) && !this.objectsToExclude.ContainsKey(propertyItem.Key))
                    {
                        this.objectsToExclude.Add(propertyItem.Key, new List<object>(new object[1] { generatedValue }));
                    }
                    else
                    {
                        this.objectsToExclude[propertyItem.Key].Add(generatedValue);
                    }

                    this.ValueGenerated?.Invoke(this, (propertyItem.Value, generatedValue));

                    dataset[i].Add(propertyItem.Key, generatedValue);
                }

                this.RowGenerated?.Invoke(this, dataset[i]);
            }

            return dataset;
        }

        /// <summary>
        /// Ensures that all the engine configuration is valid.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when a weighted property is set as unique or
        /// the weight values exceed 100%.
        /// </exception>
        private void ValidateProperties()
        {
            foreach (var property in this.properties)
            {
                if (property.Value.GetType().GetGenericTypeDefinition() == typeof(WeightedDataGeneratorProperty<>))
                {
                    IWeightedDataGeneratorProperty weightedProperty = (IWeightedDataGeneratorProperty)property.Value;

                    if (weightedProperty.Weights == null || !weightedProperty.Weights.Any())
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' is configured as weighted but does not contain weight definition.");
                    }

                    if (weightedProperty.IsUnique && weightedProperty.Weights.Any())
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' cannot be both weighted and unique.");
                    }

                    if (weightedProperty.Weights.Any(u => u.Weight < 0))
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' has weight values that are negative.");
                    }

                    float sumOfWeights = weightedProperty.Weights.Sum(u => u.Weight);
                    if (sumOfWeights < 0 || sumOfWeights > 1)
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' has weight values that exceed 100%.");
                    }
                }
                else if (property.Value.GetType().GetGenericTypeDefinition() == typeof(RangedDataGeneratorProperty<>))
                {
                    IRangedDataGeneratorProperty rangedProperty = (IRangedDataGeneratorProperty)property.Value;

                    if (rangedProperty.WeightedRanges == null || !rangedProperty.WeightedRanges.Any())
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' is configured as ranged but does not contain range definition.");
                    }

                    if (rangedProperty.WeightedRanges.Any(u => u.Weight < 0))
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' has weight values that are negative.");
                    }

                    float sumOfWeights = rangedProperty.WeightedRanges.Sum(u => u.Weight);
                    if (sumOfWeights < 0 || sumOfWeights > 1)
                    {
                        throw new InvalidOperationException($"Property '{property.Key}' has weight values that exceed 100%.");
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the properties according to their arguments/dependencies
        /// so that the engine calculates the value for the arguments before
        /// calculating the value for the property that depends on those arguments.
        /// </summary>
        /// <returns>
        /// A dictionary of <see cref="IDataGeneratorProperty"/> indexed by the
        /// <see cref="IDataGeneratorProperty.Id"/> property.
        /// </returns>
        private Dictionary<string, IDataGeneratorProperty> GetSortedProperties()
        {
            Dictionary<string, IDataGeneratorProperty> dictionary = new Dictionary<string, IDataGeneratorProperty>();
            if (this.properties != null && this.properties.Any())
            {
                HashSet<KeyValuePair<string, IDataGeneratorProperty>> nodes = new HashSet<KeyValuePair<string, IDataGeneratorProperty>>(this.properties);
                HashSet<Tuple<KeyValuePair<string, IDataGeneratorProperty>, KeyValuePair<string, IDataGeneratorProperty>>> edges = new HashSet<Tuple<KeyValuePair<string, IDataGeneratorProperty>, KeyValuePair<string, IDataGeneratorProperty>>>();
                foreach (KeyValuePair<string, IDataGeneratorProperty> property in this.properties)
                {
                    if (property.Value.Arguments != null && property.Value.Arguments.Any())
                    {
                        foreach (KeyValuePair<string, IDataGeneratorProperty[]> argument in property.Value.Arguments)
                        {
                            foreach (IDataGeneratorProperty argumentProperty in argument.Value)
                            {
                                edges.Add(new Tuple<KeyValuePair<string, IDataGeneratorProperty>, KeyValuePair<string, IDataGeneratorProperty>>(property, new KeyValuePair<string, IDataGeneratorProperty>(argumentProperty.Id, argumentProperty)));
                            }
                        }
                    }
                }

                List<KeyValuePair<string, IDataGeneratorProperty>> sortedList = this.TopologicalSort(nodes, edges);
                sortedList.Reverse();

                return sortedList.ToDictionary(u => u.Key, u => u.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// Gets a sorted list of arguments of the specified <paramref name="nodes"/> and <paramref name="edges"/>.
        /// </summary>
        /// <remarks>
        /// This method implements a topological sorting using Kahn's algorithm
        /// https://en.wikipedia.org/wiki/Topological_sorting.
        /// </remarks>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <returns>
        /// A sorted list of <see cref="KeyValuePair{TKey, TValue}"/>
        /// containing the <see cref="IDataGeneratorProperty"/> indexed by the
        /// <see cref="IDataGeneratorProperty.Id"/> property.
        /// </returns>
        private List<KeyValuePair<string, IDataGeneratorProperty>> TopologicalSort(HashSet<KeyValuePair<string, IDataGeneratorProperty>> nodes, HashSet<Tuple<KeyValuePair<string, IDataGeneratorProperty>, KeyValuePair<string, IDataGeneratorProperty>>> edges)
        {
            // Empty list that will contain the sorted elements
            List<KeyValuePair<string, IDataGeneratorProperty>> list = new List<KeyValuePair<string, IDataGeneratorProperty>>();

            // Set of all nodes with no incoming edges
            HashSet<KeyValuePair<string, IDataGeneratorProperty>> set = new HashSet<KeyValuePair<string, IDataGeneratorProperty>>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            // while S is non-empty do
            while (set.Any())
            {
                // remove a node n from S
                var n = set.First();
                set.Remove(n);

                // add n to tail of L
                list.Add(n);

                // for each node m with an edge e from n to m do
                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    var m = e.Item2;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        // insert m into S
                        set.Add(m);
                    }
                }
            }

            // if graph has edges then
            if (edges.Any())
            {
                throw new InvalidOperationException("Cyclic dependency detected.");
            }
            else
            {
                // return L (a topologically sorted order)
                return list;
            }
        }
    }
}
