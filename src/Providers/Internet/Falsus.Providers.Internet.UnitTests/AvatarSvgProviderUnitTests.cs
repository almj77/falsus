namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar.Enums;

    [TestClass]
    public class AvatarSvgProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new AvatarSvgProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            provider.GetRangedRowValue(null, null, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueWithExcludedRangesThrowsException()
        {
            // Arrange
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            provider.GetRowValue(default, Array.Empty<WeightedRange>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueWithIdThrowsException()
        {
            // Arrange
            var expected = "svg goes here";

            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

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
            string[] ids = new string[]
            {
                "id=\"Clothing/Shirt-Crew-Neck\"",
                "id=\"Vomit-Stuff\"",
                "id=\"Nose/Default\"",
                "id=\"Winky-Wink\"",
                "id=\"Eyebrow/Outline/Angry\"",
                "id=\"Top/Short-Hair/Short-Waved\"",
                "id=\"Lite-Beard\"",
                "id=\"Color/Hair/Brown\"",
                "id=\"Skin/👶🏽-03-Brown\"",
                "id=\"Top/_Resources/Kurt\""
            };
            var expected = new
            {
                ContainsId1 = true,
                ContainsId2 = true,
                ContainsId3 = true,
                ContainsId4 = true,
                ContainsId5 = true,
                ContainsId6 = true,
                ContainsId7 = true,
                ContainsId8 = true,
                ContainsId9 = true,
                ContainsId10 = true
            };

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());
            provider.InitializeRandomizer(seed);
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            string actualSvg = provider.GetRowValue(context, Array.Empty<string>());
            var actual = new
            {
                ContainsId1 = actualSvg.Contains(ids[0]),
                ContainsId2 = actualSvg.Contains(ids[1]),
                ContainsId3 = actualSvg.Contains(ids[2]),
                ContainsId4 = actualSvg.Contains(ids[3]),
                ContainsId5 = actualSvg.Contains(ids[4]),
                ContainsId6 = actualSvg.Contains(ids[5]),
                ContainsId7 = actualSvg.Contains(ids[6]),
                ContainsId8 = actualSvg.Contains(ids[7]),
                ContainsId9 = actualSvg.Contains(ids[8]),
                ContainsId10 = actualSvg.Contains(ids[9])
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Ignore]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000000
            };

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");

            List<string> generatedValues = new List<string>();
            AvatarSvgProvider provider = new AvatarSvgProvider();
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
            var expected = "svg goes here";

            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            var actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            // Assert
            Assert.IsTrue(arguments.Count == 0);
        }

        [TestMethod]
        public void GetRowValueWithOneValidAccessoryReturnsExpectedValue()
        {
            // Arrange
            string expected = "top-resource-prescription01-filter-1";

            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidAccessories = new AvatarAccessory[]
                {
                    AvatarAccessory.Prescription01
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#929598";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.Hoodie
                },
                ValidHatColors = new AvatarHatColor[]
                {
                     AvatarHatColor.Black
                },
                ValidClotheColors = new AvatarClotheColor[]
                {
                    AvatarClotheColor.Gray02
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Clothing/Hoodie\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.Hoodie
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheGraphicTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Clothing/Graphic/Bear\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.GraphicShirt
                },
                ValidClotheGraphicTypes = new AvatarClotheGraphicType[]
                {
                    AvatarClotheGraphicType.Bear
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyebrowTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Eyebrow/Natural/Flat-Natural\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidEyebrowTypes = new AvatarEyebrowType[]
                {
                    AvatarEyebrowType.FlatNatural
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyeTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Eyes/Wink-Wacky-😜\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidEyeTypes = new AvatarEyeType[]
                {
                    AvatarEyeType.WinkWacky
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "Beard-Majestic";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidFacialHairTypes = new AvatarFacialHairType[]
                {
                    AvatarFacialHairType.BeardMajestic
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#ECDCBF";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidHairColors = new AvatarHairColor[]
                {
                    AvatarHairColor.Black
                },
                ValidFacialHairTypes = new AvatarFacialHairType[]
                {
                    AvatarFacialHairType.BeardMajestic
                },
                ValidFacialHairColors = new AvatarFacialHairColor[]
                {
                    AvatarFacialHairColor.Platinum
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidHairColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#D6B370";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidFacialHairColors = new AvatarFacialHairColor[]
                {
                    AvatarFacialHairColor.Black
                },
                ValidTops = new AvatarTop[]
                {
                    AvatarTop.ShortHairShortCurly
                },
                ValidHairColors = new AvatarHairColor[]
                {
                    AvatarHairColor.BlondeGolden
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidHatColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#FFAFB9";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheColors = new AvatarClotheColor[]
                {
                    AvatarClotheColor.Black
                },
                ValidTops = new AvatarTop[]
                {
                    AvatarTop.WinterHat1
                },
                ValidHatColors = new AvatarHatColor[]
                {
                    AvatarHatColor.PastelRed
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidMouthTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Mouth/Twinkle\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidMouthTypes = new AvatarMouthType[]
                {
                    AvatarMouthType.Twinkle
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidSkinColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#D08B5B";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidSkinColors = new AvatarSkinColor[]
                {
                    AvatarSkinColor.Brown
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidTopReturnsExpectedValue()
        {
            // Arrange
            string expected = "<rect id=\"top-long-hair-straight2-path-1\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidTops = new AvatarTop[]
                {
                    AvatarTop.LongHairStraight2
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        private ProviderResult CreateProvider(
            int rowCount = 1,
            AvatarSvgProviderConfiguration? configuration = default,
            int? seed = default)
        {
            if (configuration == null)
            {
                configuration = new AvatarSvgProviderConfiguration();
            }

            AvatarSvgProvider provider = new AvatarSvgProvider(configuration);

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
            public AvatarSvgProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
