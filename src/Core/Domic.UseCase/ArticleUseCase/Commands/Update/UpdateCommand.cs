using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Update;

public class UpdateCommand : ICommand<string>
{
    //Article
    
    public string Id         { get; set; }
    public string CategoryId { get; set; }
    public string Title      { get; set; }
    public string Summary    { get; set; }
    public string Body       { get; set; }
    
    //Image
    
    public string FilePath      { get; set; }
    public string FileName      { get; set; }
    public string FileExtension { get; set; }
}