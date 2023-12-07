using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.ArticleUseCase.Commands.Create;

public class CreateCommand : ICommand<string>
{
    public string Token { get; set; }
    
    //Article
    
    public string UserId     { get; set; }
    public string CategoryId { get; set; }
    public string Title      { get; set; }
    public string Summary    { get; set; }
    public string Body       { get; set; }
    
    //Image
    
    public string FilePath      { get; set; }
    public string FileName      { get; set; }
    public string FileExtension { get; set; }
}