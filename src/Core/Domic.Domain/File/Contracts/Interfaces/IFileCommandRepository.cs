using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.File.Contracts.Interfaces;

public interface IFileCommandRepository : ICommandRepository<Entities.File, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<Entities.File> FindByArticleIdAsync(string id, CancellationToken cancellationToken) 
        => throw new NotImplementedException();
}