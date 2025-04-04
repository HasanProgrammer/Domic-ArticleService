using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Active;

public class ActiveCommand : ICommand<string>
{
    public required string Id { get; init; }
}