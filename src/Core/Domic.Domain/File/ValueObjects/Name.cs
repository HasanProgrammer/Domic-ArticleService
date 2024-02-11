using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Exceptions;

namespace Domic.Domain.File.ValueObjects;

public class Name : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Name() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد نام فایل الزامی می باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}