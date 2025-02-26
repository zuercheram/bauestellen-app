﻿namespace Baustellen.App.Client.Authentication.MSALClient;

/// <summary>
/// Static class for a few extension methods related to Exception throwing and handling
/// </summary>
/// <typeparam name="TException">The type of the exception to throw/work with.</typeparam>
public static class Exception<TException> where TException : Exception, new()
{
    /// <summary>
    /// Throws and exception of the requested type, if the predicate is true
    /// </summary>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="message">The message to pass to the raised exception.</param>
    /// <autogeneratedoc />
    public static void ThrowOn(Func<bool> predicate, string message = null)
    {
        if (predicate())
        {
            TException toThrow = Activator.CreateInstance(typeof(TException), message) as TException;
            throw toThrow;
        }
    }
}
