﻿using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.File.ValueObjects;

public class Extension : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Extension() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Extension(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد فرمت فایل الزامی می باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}