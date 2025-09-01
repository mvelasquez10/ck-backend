using System;

namespace CK.Entities
{
    public sealed class Result<T>
    {
        #region Public Constructors

        public Result(T value)
        {
            Value = value;
        }

        public Result(Exception exception)
        {
            Exception = exception;
        }

        #endregion Public Constructors

        #region Public Properties

        public Exception Exception { get; }

        public bool IsValid => Exception is null;

        public T Value { get; }

        #endregion Public Properties
    }
}