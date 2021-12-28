using System;

namespace MircoLog.Lama.Client.Services;

public class ServiceException : Exception
{
    public ExceptionCause Cause { get; set; } = ExceptionCause.UNKNOWN;

    public ServiceException()
    {
    }

    public ServiceException(ExceptionCause cause)
    {
        Cause = cause;
    }

    public ServiceException(string message, ExceptionCause cause = ExceptionCause.UNKNOWN)
        : base(message)
    {
        Cause = cause;
    }

    public ServiceException(string message, Exception innerException, ExceptionCause cause = ExceptionCause.UNKNOWN)
        : base(message, innerException)
    {
        Cause = cause;
    }
}

public enum ExceptionCause
{
    UNKNOWN,
    BAD_REQUEST
}