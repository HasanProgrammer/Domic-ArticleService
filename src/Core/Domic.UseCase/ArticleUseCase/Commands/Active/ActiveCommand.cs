using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommand : ICommand<string>
{
    public required string Token    { get; init; }
    public required string TargetId { get; init; }
}