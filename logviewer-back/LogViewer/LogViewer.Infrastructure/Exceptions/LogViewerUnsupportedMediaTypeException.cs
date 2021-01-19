using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerUnsupportedMediaTypeException : LogViewerException
    {
        public LogViewerUnsupportedMediaTypeException() : base("Unsupported Media Type")
        {
        }

        public LogViewerUnsupportedMediaTypeException(ExceptionType exceptionType = ExceptionType.Error)
            : base(exceptionType)
        {
        }

        public LogViewerUnsupportedMediaTypeException(string message) : base(message)
        {
        }

        public LogViewerUnsupportedMediaTypeException(Exception exception) : base(exception)
        {
        }

        public LogViewerUnsupportedMediaTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}