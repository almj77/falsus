namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar;

    [TestClass]
    public class AvatarUrlProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new AvatarUrlProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            AvatarUrlProvider provider = new AvatarUrlProvider(new AvatarUrlProviderConfiguration());

            // Act
            provider.GetRangedRowValue(null, null, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueWithExcludedRangesThrowsException()
        {
            // Arrange
            AvatarUrlProvider provider = new AvatarUrlProvider(new AvatarUrlProviderConfiguration());

            // Act
            provider.GetRowValue(default, Array.Empty<WeightedRange>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueWithIdThrowsException()
        {
            // Arrange
            var expected = "https://avataaars.io/?accessoriesType=Presciption01";

            AvatarUrlProvider provider = new AvatarUrlProvider(new AvatarUrlProviderConfiguration());

            // Act
            var actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 552367;
            string expected = "https://avataaars.io/?avatarStyle=Circle&accessoriesType=Kurt&clotheColor=PastelGreen&clotheType=ShirtCrewNeck&graphicType=Hola&eyebrowType=Angry&eyeType=Wink&facialHairColor=BrownDark&facialHairType=BeardLight&hairColor=Auburn&hatColor=Blue03&mouthType=Vomit&skinColor=Pale&topType=ShortHairShortWaved";

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");
            AvatarUrlProvider provider = new AvatarUrlProvider(new AvatarUrlProviderConfiguration());
            provider.InitializeRandomizer(seed);
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000000
            };

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");

            List<string> generatedValues = new List<string>();
            AvatarUrlProvider provider = new AvatarUrlProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1000000);

            Dictionary<string, object>[] rows = new Dictionary<string, object>[1000000];

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(rows[i], i, expected.RowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u)),
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            var expected = "https://avataaars.io/?accessoriesType=Presciption01";

            AvatarUrlProvider provider = new AvatarUrlProvider(new AvatarUrlProviderConfiguration());

            // Act
            var actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            AvatarUrlProvider provider = new AvatarUrlProvider(new AvatarUrlProviderConfiguration());

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            // Assert
            Assert.IsTrue(arguments.Count == 0);
        }

        [TestMethod]
        public void GetRowValueWithOneValidAccessoryReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidAccessories = new Shared.Avatar.Enums.AvatarAccessory[]
                {
                    Shared.Avatar.Enums.AvatarAccessory.Prescription01
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.AccessoriesTypeUrlParameter}={configuration.ValidAccessories[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheColorReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidClotheColors = new Shared.Avatar.Enums.AvatarClotheColor[]
                {
                    Shared.Avatar.Enums.AvatarClotheColor.Gray02
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.ClotheColorUrlParameter}={configuration.ValidClotheColors[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidClotheTypes = new Shared.Avatar.Enums.AvatarClotheType[]
                {
                    Shared.Avatar.Enums.AvatarClotheType.Hoodie
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.ClotheTypeUrlParameter}={configuration.ValidClotheTypes[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheGraphicTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidClotheTypes = new Shared.Avatar.Enums.AvatarClotheType[]
                {
                    Shared.Avatar.Enums.AvatarClotheType.GraphicShirt
                },
                ValidClotheGraphicTypes = new Shared.Avatar.Enums.AvatarClotheGraphicType[]
                {
                    Shared.Avatar.Enums.AvatarClotheGraphicType.Bear
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.ClotheGraphicTypeUrlParameter}={configuration.ValidClotheGraphicTypes[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyebrowTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidEyebrowTypes = new Shared.Avatar.Enums.AvatarEyebrowType[]
                {
                    Shared.Avatar.Enums.AvatarEyebrowType.FlatNatural
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.EyebrowTypeUrlParameter}={configuration.ValidEyebrowTypes[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyeTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidEyeTypes = new Shared.Avatar.Enums.AvatarEyeType[]
                {
                    Shared.Avatar.Enums.AvatarEyeType.WinkWacky
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.EyeTypeUrlParameter}={configuration.ValidEyeTypes[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidFacialHairTypes = new Shared.Avatar.Enums.AvatarFacialHairType[]
                {
                    Shared.Avatar.Enums.AvatarFacialHairType.BeardMajestic
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.FacialHairTypeUrlParameter}={configuration.ValidFacialHairTypes[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairColorReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidFacialHairTypes = new Shared.Avatar.Enums.AvatarFacialHairType[]
                {
                    Shared.Avatar.Enums.AvatarFacialHairType.BeardMajestic
                },
                ValidFacialHairColors = new Shared.Avatar.Enums.AvatarFacialHairColor[]
                {
                    Shared.Avatar.Enums.AvatarFacialHairColor.Platinum
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.FacialHairColorUrlParameter}={configuration.ValidFacialHairColors[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidHairColorReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidTops = new Shared.Avatar.Enums.AvatarTop[]
                {
                    Shared.Avatar.Enums.AvatarTop.ShortHairShortCurly
                },
                ValidHairColors = new Shared.Avatar.Enums.AvatarHairColor[]
                {
                    Shared.Avatar.Enums.AvatarHairColor.BlondeGolden
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.HairColorUrlParameter}={configuration.ValidHairColors[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidHatColorReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidTops = new Shared.Avatar.Enums.AvatarTop[]
                {
                    Shared.Avatar.Enums.AvatarTop.Hat
                },
                ValidHatColors = new Shared.Avatar.Enums.AvatarHatColor[]
                {
                    Shared.Avatar.Enums.AvatarHatColor.PastelRed
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.HatColorUrlParameter}={configuration.ValidHatColors[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidMouthTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidMouthTypes = new Shared.Avatar.Enums.AvatarMouthType[]
                {
                    Shared.Avatar.Enums.AvatarMouthType.Twinkle
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.MouthTypeUrlParameter}={configuration.ValidMouthTypes[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidSkinColorReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidSkinColors = new Shared.Avatar.Enums.AvatarSkinColor[]
                {
                    Shared.Avatar.Enums.AvatarSkinColor.Brown
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.SkinColorUrlParameter}={configuration.ValidSkinColors[0]}"));
        }

        [TestMethod]
        public void GetRowValueWithOneValidTopReturnsExpectedValue()
        {
            // Arrange
            AvatarUrlProviderConfiguration configuration = new AvatarUrlProviderConfiguration()
            {
                ValidTops = new Shared.Avatar.Enums.AvatarTop[]
                {
                    Shared.Avatar.Enums.AvatarTop.LongHairStraight2
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarUrlProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains($"{AvatarUrlProvider.TopTypeUrlParameter}={configuration.ValidTops[0]}"));
        }

        private ProviderResult CreateProvider(
            int rowCount = 1,
            AvatarUrlProviderConfiguration? configuration = default,
            int? seed = default)
        {
            if (configuration == null)
            {
                configuration = new AvatarUrlProviderConfiguration();
            }

            AvatarUrlProvider provider = new AvatarUrlProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar")
                .FromProvider(provider);

            provider.Load(property, rowCount);

            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            return new ProviderResult()
            {
                Provider = provider,
                Property = property,
                Context = context
            };
        }

        private struct ProviderResult
        {
            public AvatarUrlProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
