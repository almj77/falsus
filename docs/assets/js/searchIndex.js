
var camelCaseTokenizer = function (builder) {

  var pipelineFunction = function (token) {
    var previous = '';
    // split camelCaseString to on each word and combined words
    // e.g. camelCaseTokenizer -> ['camel', 'case', 'camelcase', 'tokenizer', 'camelcasetokenizer']
    var tokenStrings = token.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
      var current = cur.toLowerCase();
      if (acc.length === 0) {
        previous = current;
        return acc.concat(current);
      }
      previous = previous.concat(current);
      return acc.concat([current, previous]);
    }, []);

    // return token for each string
    // will copy any metadata on input token
    return tokenStrings.map(function(tokenString) {
      return token.clone(function(str) {
        return tokenString;
      })
    });
  }

  lunr.Pipeline.registerFunction(pipelineFunction, 'camelCaseTokenizer')

  builder.pipeline.before(lunr.stemmer, pipelineFunction)
}
var searchModule = function() {
    var documents = [];
    var idMap = [];
    function a(a,b) { 
        documents.push(a);
        idMap.push(b); 
    }

    a(
        {
            id:0,
            title:"SemanticVersionModel",
            content:"SemanticVersionModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/SemanticVersionModel',
            title:"SemanticVersionModel",
            description:""
        }
    );
    a(
        {
            id:1,
            title:"CoordinatesModel",
            content:"CoordinatesModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/CoordinatesModel',
            title:"CoordinatesModel",
            description:""
        }
    );
    a(
        {
            id:2,
            title:"RegexProvider",
            content:"RegexProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/RegexProvider',
            title:"RegexProvider",
            description:""
        }
    );
    a(
        {
            id:3,
            title:"WordProviderConfiguration",
            content:"WordProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/WordProviderConfiguration',
            title:"WordProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:4,
            title:"PasswordProviderConfiguration",
            content:"PasswordProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/PasswordProviderConfiguration',
            title:"PasswordProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:5,
            title:"FileNameProvider",
            content:"FileNameProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Sys/FileNameProvider',
            title:"FileNameProvider",
            description:""
        }
    );
    a(
        {
            id:6,
            title:"MimeTypeModel",
            content:"MimeTypeModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/MimeTypeModel',
            title:"MimeTypeModel",
            description:""
        }
    );
    a(
        {
            id:7,
            title:"FloatProvider",
            content:"FloatProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/FloatProvider',
            title:"FloatProvider",
            description:""
        }
    );
    a(
        {
            id:8,
            title:"DataGeneratorConfigurationProperty",
            content:"DataGeneratorConfigurationProperty",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Configuration/DataGeneratorConfigurationProperty',
            title:"DataGeneratorConfigurationProperty",
            description:""
        }
    );
    a(
        {
            id:9,
            title:"MimeTypeProviderConfiguration",
            content:"MimeTypeProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/MimeTypeProviderConfiguration',
            title:"MimeTypeProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:10,
            title:"ContinentModel",
            content:"ContinentModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/ContinentModel',
            title:"ContinentModel",
            description:""
        }
    );
    a(
        {
            id:11,
            title:"WeightedRange",
            content:"WeightedRange",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.GeneratorProperties/WeightedRange_1',
            title:"WeightedRange<T>",
            description:""
        }
    );
    a(
        {
            id:12,
            title:"EmailProviderConfiguration",
            content:"EmailProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/EmailProviderConfiguration',
            title:"EmailProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:13,
            title:"LastNameProvider",
            content:"LastNameProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Person/LastNameProvider',
            title:"LastNameProvider",
            description:""
        }
    );
    a(
        {
            id:14,
            title:"GuidProvider",
            content:"GuidProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/GuidProvider',
            title:"GuidProvider",
            description:""
        }
    );
    a(
        {
            id:15,
            title:"AvatarEyeType",
            content:"AvatarEyeType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarEyeType',
            title:"AvatarEyeType",
            description:""
        }
    );
    a(
        {
            id:16,
            title:"AvatarStyle",
            content:"AvatarStyle",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarStyle',
            title:"AvatarStyle",
            description:""
        }
    );
    a(
        {
            id:17,
            title:"StreetNameProvider",
            content:"StreetNameProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/StreetNameProvider',
            title:"StreetNameProvider",
            description:""
        }
    );
    a(
        {
            id:18,
            title:"AvatarUrlProviderConfiguration",
            content:"AvatarUrlProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/AvatarUrlProviderConfiguration',
            title:"AvatarUrlProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:19,
            title:"AvatarProvider",
            content:"AvatarProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/AvatarProvider',
            title:"AvatarProvider",
            description:""
        }
    );
    a(
        {
            id:20,
            title:"AvatarProviderConfiguration",
            content:"AvatarProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/AvatarProviderConfiguration',
            title:"AvatarProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:21,
            title:"LoremIpsumProviderConfiguration",
            content:"LoremIpsumProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/LoremIpsumProviderConfiguration',
            title:"LoremIpsumProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:22,
            title:"PostalCodeProvider",
            content:"PostalCodeProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/PostalCodeProvider',
            title:"PostalCodeProvider",
            description:""
        }
    );
    a(
        {
            id:23,
            title:"AvatarSkinColor",
            content:"AvatarSkinColor",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarSkinColor',
            title:"AvatarSkinColor",
            description:""
        }
    );
    a(
        {
            id:24,
            title:"RegionProvider",
            content:"RegionProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/RegionProvider',
            title:"RegionProvider",
            description:""
        }
    );
    a(
        {
            id:25,
            title:"DoubleProvider",
            content:"DoubleProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/DoubleProvider',
            title:"DoubleProvider",
            description:""
        }
    );
    a(
        {
            id:26,
            title:"AvatarHatColor",
            content:"AvatarHatColor",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarHatColor',
            title:"AvatarHatColor",
            description:""
        }
    );
    a(
        {
            id:27,
            title:"WordType",
            content:"WordType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/WordType',
            title:"WordType",
            description:""
        }
    );
    a(
        {
            id:28,
            title:"AvatarMouthType",
            content:"AvatarMouthType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarMouthType',
            title:"AvatarMouthType",
            description:""
        }
    );
    a(
        {
            id:29,
            title:"GenericArrayProvider",
            content:"GenericArrayProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers/GenericArrayProvider_1',
            title:"GenericArrayProvider<T>",
            description:""
        }
    );
    a(
        {
            id:30,
            title:"DateProvider",
            content:"DateProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/DateProvider',
            title:"DateProvider",
            description:""
        }
    );
    a(
        {
            id:31,
            title:"WordProvider",
            content:"WordProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/WordProvider',
            title:"WordProvider",
            description:""
        }
    );
    a(
        {
            id:32,
            title:"IDataGeneratorProvider",
            content:"IDataGeneratorProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers/IDataGeneratorProvider_1',
            title:"IDataGeneratorProvider<T>",
            description:""
        }
    );
    a(
        {
            id:33,
            title:"LoremIpsumProvider",
            content:"LoremIpsumProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/LoremIpsumProvider',
            title:"LoremIpsumProvider",
            description:""
        }
    );
    a(
        {
            id:34,
            title:"CountryProvider",
            content:"CountryProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/CountryProvider',
            title:"CountryProvider",
            description:""
        }
    );
    a(
        {
            id:35,
            title:"AvatarSvgProviderConfiguration",
            content:"AvatarSvgProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/AvatarSvgProviderConfiguration',
            title:"AvatarSvgProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:36,
            title:"NationalityModel",
            content:"NationalityModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/NationalityModel',
            title:"NationalityModel",
            description:""
        }
    );
    a(
        {
            id:37,
            title:"CountryModel",
            content:"CountryModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/CountryModel',
            title:"CountryModel",
            description:""
        }
    );
    a(
        {
            id:38,
            title:"CityProvider",
            content:"CityProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/CityProvider',
            title:"CityProvider",
            description:""
        }
    );
    a(
        {
            id:39,
            title:"HexColorProvider",
            content:"HexColorProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/HexColorProvider',
            title:"HexColorProvider",
            description:""
        }
    );
    a(
        {
            id:40,
            title:"GenderProvider",
            content:"GenderProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Person/GenderProvider',
            title:"GenderProvider",
            description:""
        }
    );
    a(
        {
            id:41,
            title:"IDataGeneratorProvider",
            content:"IDataGeneratorProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers/IDataGeneratorProvider',
            title:"IDataGeneratorProvider",
            description:""
        }
    );
    a(
        {
            id:42,
            title:"PasswordProvider",
            content:"PasswordProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/PasswordProvider',
            title:"PasswordProvider",
            description:""
        }
    );
    a(
        {
            id:43,
            title:"MimeTypeProvider",
            content:"MimeTypeProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/MimeTypeProvider',
            title:"MimeTypeProvider",
            description:""
        }
    );
    a(
        {
            id:44,
            title:"AvatarClotheColor",
            content:"AvatarClotheColor",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarClotheColor',
            title:"AvatarClotheColor",
            description:""
        }
    );
    a(
        {
            id:45,
            title:"AvatarEyebrowType",
            content:"AvatarEyebrowType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarEyebrowType',
            title:"AvatarEyebrowType",
            description:""
        }
    );
    a(
        {
            id:46,
            title:"AvatarFacialHairColor",
            content:"AvatarFacialHairColor",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarFacialHairColor',
            title:"AvatarFacialHairColor",
            description:""
        }
    );
    a(
        {
            id:47,
            title:"ResourceReader",
            content:"ResourceReader",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Helpers/ResourceReader',
            title:"ResourceReader",
            description:""
        }
    );
    a(
        {
            id:48,
            title:"AvatarTop",
            content:"AvatarTop",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarTop',
            title:"AvatarTop",
            description:""
        }
    );
    a(
        {
            id:49,
            title:"AvatarModel",
            content:"AvatarModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar/AvatarModel',
            title:"AvatarModel",
            description:""
        }
    );
    a(
        {
            id:50,
            title:"EmailProvider",
            content:"EmailProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/EmailProvider',
            title:"EmailProvider",
            description:""
        }
    );
    a(
        {
            id:51,
            title:"AvatarUrlProvider",
            content:"AvatarUrlProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/AvatarUrlProvider',
            title:"AvatarUrlProvider",
            description:""
        }
    );
    a(
        {
            id:52,
            title:"WeightedRange",
            content:"WeightedRange",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.GeneratorProperties/WeightedRange',
            title:"WeightedRange",
            description:""
        }
    );
    a(
        {
            id:53,
            title:"CityModel",
            content:"CityModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/CityModel',
            title:"CityModel",
            description:""
        }
    );
    a(
        {
            id:54,
            title:"NationalityProvider",
            content:"NationalityProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Person/NationalityProvider',
            title:"NationalityProvider",
            description:""
        }
    );
    a(
        {
            id:55,
            title:"WeightedDataGeneratorProperty",
            content:"WeightedDataGeneratorProperty",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.GeneratorProperties/WeightedDataGeneratorProperty_1',
            title:"WeightedDataGeneratorProperty<T>",
            description:""
        }
    );
    a(
        {
            id:56,
            title:"FileNameProviderConfiguration",
            content:"FileNameProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Sys/FileNameProviderConfiguration',
            title:"FileNameProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:57,
            title:"CoordinatesProvider",
            content:"CoordinatesProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/CoordinatesProvider',
            title:"CoordinatesProvider",
            description:""
        }
    );
    a(
        {
            id:58,
            title:"TimeProvider",
            content:"TimeProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/TimeProvider',
            title:"TimeProvider",
            description:""
        }
    );
    a(
        {
            id:59,
            title:"LoremIpsumFragmentType",
            content:"LoremIpsumFragmentType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/LoremIpsumFragmentType',
            title:"LoremIpsumFragmentType",
            description:""
        }
    );
    a(
        {
            id:60,
            title:"DataGeneratorProviderArgument",
            content:"DataGeneratorProviderArgument",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers/DataGeneratorProviderArgument',
            title:"DataGeneratorProviderArgument",
            description:""
        }
    );
    a(
        {
            id:61,
            title:"RegionModel",
            content:"RegionModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/RegionModel',
            title:"RegionModel",
            description:""
        }
    );
    a(
        {
            id:62,
            title:"BooleanProvider",
            content:"BooleanProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/BooleanProvider',
            title:"BooleanProvider",
            description:""
        }
    );
    a(
        {
            id:63,
            title:"DataGeneratorProvider",
            content:"DataGeneratorProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers/DataGeneratorProvider_1',
            title:"DataGeneratorProvider<T>",
            description:""
        }
    );
    a(
        {
            id:64,
            title:"DataGeneratorConfiguration",
            content:"DataGeneratorConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Configuration/DataGeneratorConfiguration',
            title:"DataGeneratorConfiguration",
            description:""
        }
    );
    a(
        {
            id:65,
            title:"SemanticVersionProviderConfiguration",
            content:"SemanticVersionProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Sys/SemanticVersionProviderConfiguration',
            title:"SemanticVersionProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:66,
            title:"AvatarFacialHairType",
            content:"AvatarFacialHairType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarFacialHairType',
            title:"AvatarFacialHairType",
            description:""
        }
    );
    a(
        {
            id:67,
            title:"DataGeneratorContext",
            content:"DataGeneratorContext",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus/DataGeneratorContext',
            title:"DataGeneratorContext",
            description:""
        }
    );
    a(
        {
            id:68,
            title:"IntegerProvider",
            content:"IntegerProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.DataTypes/IntegerProvider',
            title:"IntegerProvider",
            description:""
        }
    );
    a(
        {
            id:69,
            title:"SemanticVersionProvider",
            content:"SemanticVersionProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Sys/SemanticVersionProvider',
            title:"SemanticVersionProvider",
            description:""
        }
    );
    a(
        {
            id:70,
            title:"FileTypeModel",
            content:"FileTypeModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/FileTypeModel',
            title:"FileTypeModel",
            description:""
        }
    );
    a(
        {
            id:71,
            title:"CurrencyProvider",
            content:"CurrencyProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Finance/CurrencyProvider',
            title:"CurrencyProvider",
            description:""
        }
    );
    a(
        {
            id:72,
            title:"DataGeneratorProperty",
            content:"DataGeneratorProperty",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.GeneratorProperties/DataGeneratorProperty_1',
            title:"DataGeneratorProperty<T>",
            description:""
        }
    );
    a(
        {
            id:73,
            title:"FirstNameProvider",
            content:"FirstNameProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Person/FirstNameProvider',
            title:"FirstNameProvider",
            description:""
        }
    );
    a(
        {
            id:74,
            title:"AvatarClotheGraphicType",
            content:"AvatarClotheGraphicType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarClotheGraphicType',
            title:"AvatarClotheGraphicType",
            description:""
        }
    );
    a(
        {
            id:75,
            title:"AvatarClotheType",
            content:"AvatarClotheType",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarClotheType',
            title:"AvatarClotheType",
            description:""
        }
    );
    a(
        {
            id:76,
            title:"RangedDataGeneratorProperty",
            content:"RangedDataGeneratorProperty",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.GeneratorProperties/RangedDataGeneratorProperty_1',
            title:"RangedDataGeneratorProperty<T>",
            description:""
        }
    );
    a(
        {
            id:77,
            title:"DataGenerator",
            content:"DataGenerator",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus/DataGenerator',
            title:"DataGenerator",
            description:""
        }
    );
    a(
        {
            id:78,
            title:"AvatarHairColor",
            content:"AvatarHairColor",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarHairColor',
            title:"AvatarHairColor",
            description:""
        }
    );
    a(
        {
            id:79,
            title:"AvatarAccessory",
            content:"AvatarAccessory",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Avatar.Enums/AvatarAccessory',
            title:"AvatarAccessory",
            description:""
        }
    );
    a(
        {
            id:80,
            title:"AvatarSvgProvider",
            content:"AvatarSvgProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Internet/AvatarSvgProvider',
            title:"AvatarSvgProvider",
            description:""
        }
    );
    a(
        {
            id:81,
            title:"IDataGeneratorProperty",
            content:"IDataGeneratorProperty",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.GeneratorProperties/IDataGeneratorProperty',
            title:"IDataGeneratorProperty",
            description:""
        }
    );
    a(
        {
            id:82,
            title:"RandomExtensions",
            content:"RandomExtensions",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Extensions/RandomExtensions',
            title:"RandomExtensions",
            description:""
        }
    );
    a(
        {
            id:83,
            title:"ContinentProvider",
            content:"ContinentProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Location/ContinentProvider',
            title:"ContinentProvider",
            description:""
        }
    );
    a(
        {
            id:84,
            title:"RegexProviderConfiguration",
            content:"RegexProviderConfiguration",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Text/RegexProviderConfiguration',
            title:"RegexProviderConfiguration",
            description:""
        }
    );
    a(
        {
            id:85,
            title:"CurrencyModel",
            content:"CurrencyModel",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Shared.Models/CurrencyModel',
            title:"CurrencyModel",
            description:""
        }
    );
    a(
        {
            id:86,
            title:"FileTypeProvider",
            content:"FileTypeProvider",
            description:'',
            tags:''
        },
        {
            url:'/api/Falsus.Providers.Sys/FileTypeProvider',
            title:"FileTypeProvider",
            description:""
        }
    );
    var idx = lunr(function() {
        this.field('title');
        this.field('content');
        this.field('description');
        this.field('tags');
        this.ref('id');
        this.use(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
        documents.forEach(function (doc) { this.add(doc) }, this)
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();
