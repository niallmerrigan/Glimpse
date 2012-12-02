﻿using System;
using System.Reflection;

namespace Glimpse.Core.Message
{
    public interface IMessage
    {
        Guid Id { get; }

        Type ExecutedType { get; }

        MethodInfo ExecutedMethod { get; } 
    }
}