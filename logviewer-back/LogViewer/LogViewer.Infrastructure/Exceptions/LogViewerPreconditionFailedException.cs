using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerPreconditionFailedException : LogViewerException
    {
        public LogViewerPreconditionFailedException(ExceptionType exceptionType = ExceptionType.Error) : base(exceptionType)
        {
        }

        public LogViewerPreconditionFailedException(string message) : base(message)
        {
        }

        public LogViewerPreconditionFailedException(Exception exception) : base(exception)
        {
        }

        public LogViewerPreconditionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}