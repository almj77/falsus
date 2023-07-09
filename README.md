<div align="center">
  <h1>Falsus</h1>
  <p>A .Net data generation framework designed to generate realistic fake data for various purposes, inspired by the functionality of <a href="https://github.com/faker-js/faker">faker.js</a>.</p>
</div>

## 🚀 Features
### Unleash the power of data generation with unparalleled control.
- Take charge by defining fields that are not allowed to be null.
- Ensure uniqueness by specifying fields that require distinct values.
- Tailor the output to your exact needs by assigning percentage to ranges of data to generate diverse data sets (e.g., effortlessly create a list of 1000 countries where a minimum of 25% must belong to the European continent).
- Seamlessly establish dependencies between fields, enabling logical connections (e.g., when a continent is already generated, generate a country that perfectly aligns with its continent).

### Data Generators
#### Data Types
- Boolean
- Date
- Double
- Float
- Guid
- Integer
- Time
#### Finance
- Currency
#### Location
- Continent
- Country
- Region
- City
- Street
- Postal Code
- Coordinates
#### Text
- Lorem Ipsum
- Regular Expression
- Word (Dictionary)
#### Person
- First name
- Last name
- Nationality
- Gender
#### Internet
- Email address
- Hexadecimal color
- MIME type
- Avatar
- Password
#### System
- File type
- Semantic Version

## 🕹 Usage
### Basic
```C#
using Falsus;
using Falsus.Providers.Number;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize Generator
            DataGenerator generator = new DataGenerator();

            // Create new instance of the provider of integer numbers
            IntegerProvider idProvider = new IntegerProvider();

            // Instruct genertor that we want an "Id" property
            // provided by the idProvider and only unique
            // values are to be generated
            generator.WithProperty<int>("Id")
                .FromProvider(idProvider)
                .Unique();

            // Generate 100 values.
            // The dataset variable will contain 100 entries, each of them with
            // a key value pair of string key and int value.
            // The keys will match the ones specified for each property
            // in this case it's "Id"
            Dictionary<string, object>[] dataset = generator.Generate(100);

            foreach (var item in dataset)
            {
                int id = Convert.ToInt32(item["Id"]);
                Console.WriteLine(id);
            }

            Console.ReadKey();
        }
    }
}
```
The above code generates 100 random integer numbers ranging from -2,147,483,648 to 2,147,483,647.
It initializes the Data Generator Engine, configures one property with data generated by the Integer Provider and generates 100 values.

### Weigthed Range
Considering the above scenario where we are generating unique Ids in the form of integer numbers, these are usually not negative.
With a simple change we can instruct the Engine to allow only values within a specific range (in this case, between 0 and 2,147,483,647).
```C#
using Falsus;
using Falsus.Providers.Number;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize Generator
            DataGenerator generator = new DataGenerator();

            // Create new instance of the provider of integer numbers
            IntegerProvider idProvider = new IntegerProvider();

            // Instruct genertor that we want an "Id" property
            // provided by the idProvider and only unique, positive
            // values are to be generated
            generator.WithRangedProperty<int>("Id")**
                .FromProvider(idProvider)
                .WithWeightedRanges(new Falsus.GeneratorProperties.WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = int.MaxValue,
                    Weight = 1 // Weight should be between 0 and 1.
                })
                .Unique();

            // Generate 100 values.
            // The dataset variable will contain 100 entries, each of them with
            // a key value pair of string key and int value.
            // The keys will match the ones specified for each property
            // in this case it's "Id"
            Dictionary<string, object>[] dataset = generator.Generate(100);

            foreach (var item in dataset)
            {
                int id = Convert.ToInt32(item["Id"]);
                Console.WriteLine(id);
            }

            Console.ReadKey();
        }
    }
}
````

Now, assume you want to ensure that at least 25% of your dataset contains values below 1000, 25% above 2000000000 and the remaining 50% should be within any range except those two and remain a positive number.
```C#
using Falsus;
using Falsus.Providers.Number;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize Generator
            DataGenerator generator = new DataGenerator();

            // Create new instance of the provider of integer numbers
            IntegerProvider idProvider = new IntegerProvider();

            // Instruct genertor that we want an "Id" property
            // provided by the idProvider and only unique, positive
            // values are to be generated
            generator.WithRangedProperty<int>("Id")
                .FromProvider(idProvider)
                .WithWeightedRanges(new Falsus.GeneratorProperties.WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = 1000,
                    Weight = 0.25f
                }, new Falsus.GeneratorProperties.WeightedRange<int>()
                {
                    MinValue = 2000000000,
                    MaxValue = int.MaxValue,
                    Weight = 0.25f
                },
                new Falsus.GeneratorProperties.WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = int.MaxValue,
                    Weight = 0.5f
                })
                .Unique();

            // Generate 100 values.
            // The dataset variable will contain 100 entries, each of them with
            // a key value pair of string key and int value.
            // The keys will match the ones specified for each property
            // in this case it's "Id"
            Dictionary<string, object>[] dataset = generator.Generate(100);

            foreach (var item in dataset)
            {
                int id = Convert.ToInt32(item["Id"]);
                Console.WriteLine(id);
            }

            Console.ReadKey();
        }
    }
}
```

### Property Dependency
```C#
using Falsus;
using Falsus.GeneratorProperties;
using Falsus.Providers.Number;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize Generator
            DataGenerator generator = new DataGenerator();

            // Create new instance of the provider of countries.
            CountryProvider countryProvider = new CountryProvider();

            // Crate a new instance of the provider of continents.
            ContinentProvider continentProvider = new ContinentProvider();

            // Instruct generator that we want a "Continent" property
            // provided by the continent provider .
            DataGeneratorProperty<ContinentModel> continentProperty = generator
                .WithProperty<ContinentModel>("Continent")
                .FromProvider(continentProvider);

            // Instruct genertor that we want a "Country" property
            // provided by the countryProvider that belongs to the 
            // continent generated by the continentProvider for
            // the same row.
            generator.WithProperty<CountryModel>("Country")
                .FromProvider(countryProvider)
                .WithArgument(CountryProvider.ContinentArgumentName, continentProperty);


            // Generate 100 values.
            // The dataset variable will contain 100 entries, each of them with
            // a key value pair of string key and int value.
            // The keys will match the ones specified for each property
            // in this case it's "Continent" and "Country
            Dictionary<string, object>[] dataset = generator.Generate(100);

            foreach (var item in dataset)
            {
                ContinentModel continent = (ContinentModel)item["Continent"]);
                CountryModel country = (CountryModel)item["Country"]);
                Console.WriteLine(string.Concat(continent.Name, ", ", country.Name);
            }

            Console.ReadKey();
        }
    }
}
```

The above code instructs the engine to generate 100 objects containing two properties (Country and Continent).
Given the declared dependency between the country and continent properties, the Continents will be generated first and the generated Country will always match the specified continent.

## 📘 Credits
Thanks to all contributors of:
- [NLipsum](https://github.com/alexcpendleton/NLipsum)
- [Fare - Finite Automata and Regular Expressions](https://github.com/moodmosaic/Fare)
