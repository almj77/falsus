namespace Falsus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Defines the context for the data generation of a single property.
    /// </summary>
    /// <remarks>
    /// A new context is created for each row and for each property of that row.
    /// </remarks>
    public class DataGeneratorContext
    {
        /// <summary>
        /// Gets a dictionary of <see cref="object"/> instances
        /// indexed by <see cref="IDataGeneratorProperty.Id"/> property.
        /// </summary>
        private Dictionary<string, object> row;

        /// <summary>
        /// Gets a dictionary of argument <see cref="IDataGeneratorProperty"/>s
        /// indexed by <see cref="IDataGeneratorProperty.Id"/> property.
        /// </summary>
        private Dictionary<string, IDataGeneratorProperty> argumentsByPropertyName;

        /// <summary>
        /// Gets a dictionary of argument <see cref="IDataGeneratorProperty"/>s
        /// indexed by the argument name.
        /// </summary>
        private Dictionary<string, IDataGeneratorProperty[]> argumentsByArgumentName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorContext"/> class.
        /// </summary>
        /// <param name="row">
        /// A dictionary of <see cref="object"/> instances containing the values
        /// already generated for the current row.
        /// </param>
        /// <param name="index">
        /// The index of the current row.
        /// </param>
        /// <param name="rowCount">
        /// The total number of rows. Represents the expected total number of rows.
        /// </param>
        /// <param name="currentProperty">
        /// An implementation of <see cref="IDataGeneratorProperty"/> representing
        /// the property binded to this context.
        /// </param>
        /// <param name="arguments">
        /// The dictionary of <see cref="IDataGeneratorProperty"/>s that can be
        /// fetched from this context.
        /// </param>
        public DataGeneratorContext(
            Dictionary<string, object> row,
            int index,
            int rowCount,
            IDataGeneratorProperty currentProperty,
            Dictionary<string, IDataGeneratorProperty[]> arguments)
        {
            this.row = row;

            if (arguments != null)
            {
                this.argumentsByArgumentName = arguments;
                this.argumentsByPropertyName = arguments.SelectMany(u => u.Value).GroupBy(u => u.Id).ToDictionary(k => k.Key, v => v.First());
            }
            else
            {
                this.argumentsByArgumentName = new Dictionary<string, IDataGeneratorProperty[]>();
                this.argumentsByPropertyName = new Dictionary<string, IDataGeneratorProperty>();
            }

            this.CurrentProperty = currentProperty;
            this.CurrentRowIndex = index;
            this.RequestedRowCount = rowCount;
        }

        /// <summary>
        /// Gets the index of the current row.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the index of the current row.
        /// </value>
        /// <remarks>
        /// The row index starts from 0.
        /// </remarks>
        public int CurrentRowIndex { get; private set; }

        /// <summary>
        /// Gets an implementation of <see cref="IDataGeneratorProperty"/> that
        /// represents the property binded to this context.
        /// </summary>
        /// <value>
        /// An instance that implements the <see cref="IDataGeneratorProperty"/> interface.
        /// </value>
        public IDataGeneratorProperty CurrentProperty { get; private set; }

        /// <summary>
        /// Gets the requested number of rows.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the
        /// expected total number of rows.
        /// </value>
        public int RequestedRowCount { get; private set; }

        /// <summary>
        /// Gets the list of arguments that can be fetched from this context.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the
        /// name of the argument as key and the collection of
        /// <see cref="IDataGeneratorProperty"/> instances as value.
        /// </value>
        public Dictionary<string, IDataGeneratorProperty[]> Arguments
        {
            get
            {
                return new Dictionary<string, IDataGeneratorProperty[]>(this.argumentsByArgumentName);
            }
        }

        /// <summary>
        /// Determines whether or not this context has an argument
        /// with the same name as the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The name of the argument.</param>
        /// <returns>
        /// True if this context contains an argument with the provided
        /// name, otherwise false.
        /// </returns>
        public bool HasArgumentValue(string id)
        {
            return this.argumentsByArgumentName.ContainsKey(id);
        }

        /// <summary>
        /// Retrieves the value for the argument with the specified name.
        /// </summary>
        /// <typeparam name="T">The expected type of the argument value.</typeparam>
        /// <param name="id">The name of the argument.</param>
        /// <returns>
        /// An instance of <typeparamref name="T"/> containing the value of the argument
        /// within the current context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throw if one of the following conditions is met:
        /// <list type="bullet">
        /// <item>There is no argument with the specified name.</item>
        /// <item>The argument with the specified name is null.</item>
        /// <item>Access to the argument property is not allowed.</item>
        /// </list>
        /// </exception>
        public T GetArgumentValue<T>(string id)
        {
            if (this.HasArgumentValue(id))
            {
                if (this.argumentsByArgumentName[id].Any())
                {
                    IDataGeneratorProperty property = this.argumentsByArgumentName[id].First();
                    if (property != null)
                    {
                        return this.GetRowProperty<T>(property.Id);
                    }
                    else
                    {
                        throw new InvalidOperationException($"'{id}' argument configured property cannot be null.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"'{id}' argument has no configured property.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Access to '{id}' row property not allowed.");
            }
        }

        /// <summary>
        /// Retrieves the value for the argument with the specified name.
        /// </summary>
        /// <param name="id">The name of the argument.</param>
        /// <returns>
        /// An instance of <see cref="object"/> containing the value of the argument
        /// within the current context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throw if one of the following conditions is met:
        /// <list type="bullet">
        /// <item>There is no argument with the specified name.</item>
        /// <item>The argument with the specified name is null.</item>
        /// <item>Access to the argument property is not allowed.</item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// If value type is known use <see cref="DataGeneratorContext.GetArgumentValue{T}(string)"/> overload.
        /// </remarks>
        public object GetArgumentValue(string id)
        {
            if (this.HasArgumentValue(id))
            {
                if (this.argumentsByArgumentName[id].Any())
                {
                    IDataGeneratorProperty property = this.argumentsByArgumentName[id].First();
                    if (property != null)
                    {
                        return this.GetRowProperty(property.Id);
                    }
                    else
                    {
                        throw new InvalidOperationException($"'{id}' argument configured property cannot be null.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"'{id}' argument has no configured property.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Access to '{id}' row property not allowed.");
            }
        }

        /// <summary>
        /// Retrieves the values for the argument with the specified name.
        /// </summary>
        /// <typeparam name="T">The expected type of the collection values.</typeparam>
        /// <param name="id">The name of the argument.</param>
        /// <returns>
        /// An array of <typeparamref name="T"/> instances containing the values of the argument
        /// within the current context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throw if one of the following conditions is met:
        /// <list type="bullet">
        /// <item>There is no argument with the specified name.</item>
        /// <item>The argument with the specified name is null.</item>
        /// <item>Access to the argument property is not allowed.</item>
        /// </list>
        /// </exception>
        public T[] GetArgumentValues<T>(string id)
        {
            if (this.HasArgumentValue(id))
            {
                if (this.argumentsByArgumentName[id].Any())
                {
                    IDataGeneratorProperty[] properties = this.argumentsByArgumentName[id];
                    if (properties.Any())
                    {
                        T[] values = new T[properties.Length];
                        for (int i = 0; i < properties.Length; i++)
                        {
                            values[i] = this.GetRowProperty<T>(properties[i].Id);
                        }

                        return values;
                    }
                    else
                    {
                        throw new InvalidOperationException($"'{id}' argument configured property cannot be empty.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"'{id}' argument has no configured property.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Access to '{id}' row property not allowed.");
            }
        }

        /// <summary>
        /// Retrieves the values for the argument with the specified name.
        /// </summary>
        /// <param name="id">The name of the argument.</param>
        /// <returns>
        /// An array of <see cref="object"/> instances containing the values of the argument
        /// within the current context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throw if one of the following conditions is met:
        /// <list type="bullet">
        /// <item>There is no argument with the specified name.</item>
        /// <item>The argument with the specified name is null.</item>
        /// <item>Access to the argument property is not allowed.</item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// If value type is known use <see cref="DataGeneratorContext.GetArgumentValues{T}(string)"/> overload.
        /// </remarks>
        public object[] GetArgumentValues(string id)
        {
            if (this.HasArgumentValue(id))
            {
                if (this.argumentsByArgumentName[id].Any())
                {
                    IDataGeneratorProperty[] properties = this.argumentsByArgumentName[id];
                    if (properties.Any())
                    {
                        object[] values = new object[properties.Length];
                        for (int i = 0; i < properties.Length; i++)
                        {
                            values[i] = this.GetRowProperty(properties[i].Id);
                        }

                        return values;
                    }
                    else
                    {
                        throw new InvalidOperationException($"'{id}' argument configured property cannot be empty.");
                    }
                }
                else
                {
                    throw new InvalidOperationException($"'{id}' argument has no configured property.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Access to '{id}' row property not allowed.");
            }
        }

        /// <summary>
        /// Retrieves the value for the property with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The expected type of the property value.</typeparam>
        /// <param name="id">The unique identifier of the property.</param>
        /// <returns>
        /// An instance of <typeparamref name="T"/> containing the value of the property
        /// within the current context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throw if one of the following conditions is met:
        /// <list type="bullet">
        /// <item>Access to the argument property is not allowed.</item>
        /// <item>The requested property does not hold an instance of <typeparamref name="T"/>.</item>
        /// </list>
        /// </exception>
        public T GetRowProperty<T>(string id)
        {
            if (!this.argumentsByPropertyName.ContainsKey(id))
            {
                throw new InvalidOperationException($"Access to '{id}' row property not allowed.");
            }

            if (this.argumentsByPropertyName.ContainsKey(id) && typeof(T) != this.argumentsByPropertyName[id].ValueType)
            {
                throw new InvalidOperationException($"'{id}' row property was requested with type {typeof(T).FullName}. Expected type is {this.argumentsByPropertyName[id].ValueType.FullName}.");
            }

            if (!this.row.ContainsKey(id))
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(this.row[id], typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Retrieves the value for the property with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property.</param>
        /// <returns>
        /// An instance of <see cref="object"/> containing the value of the property
        /// within the current context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throw if one Access to the argument property is not allowed.
        /// </exception>
        /// <remarks>
        /// If value type is known use <see cref="DataGeneratorContext.GetRowProperty{T}(string)"/> overload.
        /// </remarks>
        public object GetRowProperty(string id)
        {
            if (!this.argumentsByPropertyName.ContainsKey(id))
            {
                throw new InvalidOperationException($"Access to '{id}' row property not allowed.");
            }

            if (this.row.ContainsKey(id))
            {
                return this.row[id];
            }

            return null;
        }
    }
}
