using System.Text.Json;

namespace CK.Rest.Common.Middleware
{
    internal class ErrorDetails
    {
        #region Public Constructors

        public ErrorDetails(string mesage)
        {
            Message = mesage;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Message { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        #endregion Public Methods
    }
}