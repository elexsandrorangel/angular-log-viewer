using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerConflictException : LogViewerException
    {
        public LogViewerConflictException()
                  : base("Record already exists at the server")
        {
            ExceptionType = ExceptionType.Error;
        }

        public LogViewerConflictException(ExceptionType exceptionType = ExceptionType.Error)
            : base(exceptionType)
        {
        }

        public LogViewerConflictException(string message) : base(message)
        {
        }

        public LogViewerConflictException(Exception exception) : base(exception)
        {
        }

        public LogViewerConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
