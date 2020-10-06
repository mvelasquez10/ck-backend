using System;

namespace CK.Repository
{
    public sealed class Filter<T>
    {
        #region Public Constructors

        public Filter(string property, object value)
        {
            Property = string.IsNullOrWhiteSpace(property) ? throw new ArgumentNullException(nameof(property)) : property;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion Public Constructors

        #region Public Properties

        public string Property { get; private set; }

        public object Value { get; private set; }

        #endregion Public Properties
    }
}