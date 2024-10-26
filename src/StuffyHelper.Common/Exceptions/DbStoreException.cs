﻿namespace StuffyHelper.Common.Exceptions;

public class DbStoreException : Exception
{
    public DbStoreException(string message)
        : base(message)
    {
    }

    public DbStoreException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DbStoreException(Exception innerException)
        : base("The data store operation failed.", innerException)
    {
    }
}