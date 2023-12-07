using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.Delete;

public class DeleteCommand : ICommand<string>
{
    public required string Token    { get; init; }
    public required string TargetId { get; init; }
}