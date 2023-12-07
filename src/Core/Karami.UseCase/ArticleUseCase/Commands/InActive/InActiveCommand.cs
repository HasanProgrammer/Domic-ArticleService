using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.InActive;

public class InActiveCommand : ICommand<string>
{
    public required string Token    { get; init; }
    public required string TargetId { get; init; }
}