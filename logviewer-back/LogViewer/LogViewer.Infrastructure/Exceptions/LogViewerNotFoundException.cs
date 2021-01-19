using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerNotFoundException : LogViewerException
    {
        public LogViewerNotFoundException()
            : base("Ops! Não encontramos esta página! Veja se o endereço está correto! Ou volte para página inicial.")
        {
        }

        public LogViewerNotFoundException(ExceptionType exceptionType = ExceptionType.Error)
            : base(exceptionType)
        {
        }

        public LogViewerNotFoundException(string message)
            : base(message)
        {
        }

        public LogViewerNotFoundException(Exception exception) : base(exception)
        {
        }

        public LogViewerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
