
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
            id:1,
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
            id:2,
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
            id:3,
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
            id:4,
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
            id:5,
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
            id:6,
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
            id:7,
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
            id:8,
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
            id:9,
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
            id:10,
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
            id:11,
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
            id:12,
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
            id:13,
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
            id:14,
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
            id:15,
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
            id:16,
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
            id:17,
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
            id:18,
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
            id:19,
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
            id:20,
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
            id:21,
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
            id:22,
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
