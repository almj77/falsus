
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
            id:1,
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
            id:2,
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
            id:3,
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
            id:4,
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
            id:5,
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
            id:6,
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
            id:7,
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
            id:8,
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
            id:9,
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
            id:10,
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
            id:11,
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
            id:12,
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
            id:13,
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
            id:14,
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
            id:15,
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
            id:16,
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
            id:17,
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
            id:18,
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
            id:19,
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
            id:20,
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
            id:21,
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
            id:22,
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
            id:23,
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
            id:24,
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
            id:25,
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
            id:26,
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
            id:27,
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
            id:28,
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
            id:29,
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
            id:30,
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
            id:31,
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
            id:32,
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
            id:33,
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
            id:34,
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
            id:35,
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
            id:36,
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
            id:37,
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
            id:38,
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
            id:39,
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
            id:40,
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
            id:41,
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
            id:42,
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
            id:43,
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
            id:44,
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
            id:45,
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
            id:46,
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
            id:47,
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
            id:48,
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
            id:49,
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
            id:50,
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
            id:51,
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
            id:52,
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
            id:53,
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
            id:54,
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
            id:55,
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
            id:56,
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
            id:57,
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
