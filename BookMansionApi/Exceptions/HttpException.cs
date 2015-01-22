using System.Runtime.InteropServices;

namespace BookMansionApi.Exceptions
{
    class HttpException : COMException
    {
        #region > Constructor

        public HttpException(BookManExceptionType type, string message = null)
            : base(message, (int)type) { }

        #endregion
    }
}
