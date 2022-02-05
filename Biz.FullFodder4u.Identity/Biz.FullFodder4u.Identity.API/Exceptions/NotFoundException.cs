﻿using System.Runtime.Serialization;

namespace Biz.FullFodder4u.Identity.API.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException()
    { }

    public NotFoundException(string message) : base(message)
    { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    { }

    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}