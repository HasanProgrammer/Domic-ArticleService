using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Exceptions;

namespace Domic.Domain.Article.ValueObjects;

public class Body : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Body() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Body(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد متن الزامی می باشد !");

        if (value.Length < 200)
            throw new DomainException("فیلد متن نباید کمتر از 200 عبارت داشته باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}