namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar;

    [TestClass]
    public class AvatarProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new AvatarProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            AvatarProvider provider = new AvatarProvider(new AvatarProviderConfiguration());

            // Act
            provider.GetRangedRowValue(null, null, Array.Empty<AvatarModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueWithExcludedRangesThrowsException()
        {
            // Arrange
            AvatarProvider provider = new AvatarProvider(new AvatarProviderConfiguration());

            // Act
            provider.GetRowValue(default, Array.Empty<WeightedRange>(), Array.Empty<AvatarModel>());
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 77955236;
            AvatarModel expectedTyped = new AvatarModel()
            {
                Accessory = Shared.Avatar.Enums.AvatarAccessory.Prescription01,
                ClotheColor = Shared.Avatar.Enums.AvatarClotheColor.Gray02,
                ClotheGraphicType = Shared.Avatar.Enums.AvatarClotheGraphicType.Resist,
                ClotheType = Shared.Avatar.Enums.AvatarClotheType.Hoodie,
                EyebrowType = Shared.Avatar.Enums.AvatarEyebrowType.SadConcerned,
                EyeType = Shared.Avatar.Enums.AvatarEyeType.Dizzy,
                FacialHairColor = Shared.Avatar.Enums.AvatarFacialHairColor.Brown,
                FacialHairType = Shared.Avatar.Enums.AvatarFacialHairType.BeardLight,
                HairColor = Shared.Avatar.Enums.AvatarHairColor.BrownDark,
                HatColor = Shared.Avatar.Enums.AvatarHatColor.Blue01,
                MouthType = Shared.Avatar.Enums.AvatarMouthType.Sad,
                SkinColor = Shared.Avatar.Enums.AvatarSkinColor.Pale,
                Top = Shared.Avatar.Enums.AvatarTop.WinterHat1
            };

            var expected = new
            {
                expectedTyped.Accessory,
                expectedTyped.ClotheColor,
                expectedTyped.ClotheGraphicType,
                expectedTyped.ClotheType,
                expectedTyped.EyebrowType,
                expectedTyped.EyeType,
                expectedTyped.FacialHairColor,
                expectedTyped.FacialHairType,
                expectedTyped.HairColor,
                expectedTyped.HatColor,
                expectedTyped.MouthType,
                expectedTyped.SkinColor,
                expectedTyped.Top
            };

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");
            AvatarProvider provider = new AvatarProvider(new AvatarProviderConfiguration());
            provider.InitializeRandomizer(seed);
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            AvatarModel actualTyped = provider.GetRowValue(context, Array.Empty<AvatarModel>());
            var actual = new
            {
                actualTyped.Accessory,
                actualTyped.ClotheColor,
                actualTyped.ClotheGraphicType,
                actualTyped.ClotheType,
                actualTyped.EyebrowType,
                actualTyped.EyeType,
                actualTyped.FacialHairColor,
                actualTyped.FacialHairType,
                actualTyped.HairColor,
                actualTyped.HatColor,
                actualTyped.MouthType,
                actualTyped.SkinColor,
                actualTyped.Top
            };

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

            DataGeneratorProperty<AvatarModel> property = new DataGeneratorProperty<AvatarModel>("Avatar");

            List<AvatarModel> generatedValues = new List<AvatarModel>();
            AvatarProvider provider = new AvatarProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1000000);

            Dictionary<string, object>[] rows = new Dictionary<string, object>[1000000];

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(rows[i], i, expected.RowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<AvatarModel>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u != null),
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueWithIdThrowsException()
        {
            // Arrange
            AvatarProvider provider = new AvatarProvider(new AvatarProviderConfiguration());

            // Act
            provider.GetRowValue("Avatar");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetValueIdThrowsException()
        {
            // Arrange
            AvatarProvider provider = new AvatarProvider(new AvatarProviderConfiguration());

            // Act
            provider.GetValueId(new AvatarModel());
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            AvatarProvider provider = new AvatarProvider(new AvatarProviderConfiguration());

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            // Assert
            Assert.IsTrue(arguments.Count == 0);
        }

        [TestMethod]
        public void GetRowValueWithOneValidAccessoryReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidAccessories = new Shared.Avatar.Enums.AvatarAccessory[]
                {
                    Shared.Avatar.Enums.AvatarAccessory.Prescription01
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.Accessory == configuration.ValidAccessories[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheColorReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidClotheColors = new Shared.Avatar.Enums.AvatarClotheColor[]
                {
                    Shared.Avatar.Enums.AvatarClotheColor.Gray02
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.ClotheColor == configuration.ValidClotheColors[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidClotheTypes = new Shared.Avatar.Enums.AvatarClotheType[]
                {
                    Shared.Avatar.Enums.AvatarClotheType.Hoodie
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.ClotheType == configuration.ValidClotheTypes[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheGraphicTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
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
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.ClotheGraphicType == configuration.ValidClotheGraphicTypes[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyebrowTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidEyebrowTypes = new Shared.Avatar.Enums.AvatarEyebrowType[]
                {
                    Shared.Avatar.Enums.AvatarEyebrowType.FlatNatural
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.EyebrowType == configuration.ValidEyebrowTypes[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyeTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidEyeTypes = new Shared.Avatar.Enums.AvatarEyeType[]
                {
                    Shared.Avatar.Enums.AvatarEyeType.WinkWacky
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.EyeType == configuration.ValidEyeTypes[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidFacialHairTypes = new Shared.Avatar.Enums.AvatarFacialHairType[]
                {
                    Shared.Avatar.Enums.AvatarFacialHairType.BeardMajestic
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.FacialHairType == configuration.ValidFacialHairTypes[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairColorReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
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
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.FacialHairColor == configuration.ValidFacialHairColors[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidHairColorReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
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
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.HairColor == configuration.ValidHairColors[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidHatColorReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
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
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.HatColor == configuration.ValidHatColors[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidMouthTypeReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidMouthTypes = new Shared.Avatar.Enums.AvatarMouthType[]
                {
                    Shared.Avatar.Enums.AvatarMouthType.Twinkle
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.MouthType == configuration.ValidMouthTypes[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidSkinColorReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidSkinColors = new Shared.Avatar.Enums.AvatarSkinColor[]
                {
                    Shared.Avatar.Enums.AvatarSkinColor.Brown
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.SkinColor == configuration.ValidSkinColors[0]);
        }

        [TestMethod]
        public void GetRowValueWithOneValidTopReturnsExpectedValue()
        {
            // Arrange
            AvatarProviderConfiguration configuration = new AvatarProviderConfiguration()
            {
                ValidTops = new Shared.Avatar.Enums.AvatarTop[]
                {
                    Shared.Avatar.Enums.AvatarTop.LongHairStraight2
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarProvider provider = providerResult.Provider;

            // Act
            AvatarModel actual = provider.GetRowValue(providerResult.Context, Array.Empty<AvatarModel>());

            // Assert
            Assert.IsTrue(actual.Top == configuration.ValidTops[0]);
        }

        private ProviderResult CreateProvider(
            int rowCount = 1,
            AvatarProviderConfiguration? configuration = default,
            int? seed = default)
        {
            if (configuration == null)
            {
                configuration = new AvatarProviderConfiguration();
            }

            AvatarProvider provider = new AvatarProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<AvatarModel> property = new DataGeneratorProperty<AvatarModel>("Avatar")
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
            public AvatarProvider Provider;
            public DataGeneratorProperty<AvatarModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
