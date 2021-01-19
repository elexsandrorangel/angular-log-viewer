using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class UnprocessableEntityError : LogViewerException
    {
        public UnprocessableEntityError(ExceptionType exceptionType = ExceptionType.Error) : base(exceptionType)
        {
        }

        public UnprocessableEntityError(string message) : base(message)
        {
        }

        public UnprocessableEntityError(Exception exception) : base(exception)
        {
        }

        public UnprocessableEntityError(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}