using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.File.ValueObjects;

public class Path : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Path() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Path(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد مسیر فایل الزامی می باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}