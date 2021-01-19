using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerForbiddenException : LogViewerException
    {
        public LogViewerForbiddenException()
            : base("Desculpe! Mas esta página possui acesso restrito!")
        {
        }

        public LogViewerForbiddenException(ExceptionType exceptionType = ExceptionType.Error)
            : base(exceptionType)
        {
        }

        public LogViewerForbiddenException(string message)
            : base(message)
        {
        }

        public LogViewerForbiddenException(Exception exception)
            : base(exception)
        {
        }

        public LogViewerForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}