using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.Article.ValueObjects;

public class Summary : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Summary() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Summary(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد توضیحات مختصر الزامی می باشد !");

        if (value.Length is > 500 or < 30)
            throw new DomainException("فیلد توضیحات مختصر نباید بیشتر از 500 و کمتر از 30 عبارت داشته باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}