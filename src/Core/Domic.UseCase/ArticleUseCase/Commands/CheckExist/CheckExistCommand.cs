using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.CheckExist;

public class CheckExistCommand : IQuery<bool>
{
    public required string ArticleId { get; set; }
}