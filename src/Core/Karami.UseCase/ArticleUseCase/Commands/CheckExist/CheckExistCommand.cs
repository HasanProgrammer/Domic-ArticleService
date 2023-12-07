using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.CheckExist;

public class CheckExistCommand : IQuery<bool>
{
    public required string ArticleId { get; set; }
}