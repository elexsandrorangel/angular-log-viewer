using System;

namespace LogViewer.Infrastructure.Exceptions
{
    public class LogViewerException : Exception
    {
        public ExceptionType ExceptionType { get; set; } = ExceptionType.Error;

        public LogViewerException(ExceptionType exceptionType = ExceptionType.Error)
        {
            ExceptionType = exceptionType;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Exception" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public LogViewerException(string message) : this(message, null)
        {
        }

        public LogViewerException(Exception exception)
            : this("Houston, we got a problem!", exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception" />
        /// class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        ///  if no inner exception is specified.
        /// </param>
        public LogViewerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
