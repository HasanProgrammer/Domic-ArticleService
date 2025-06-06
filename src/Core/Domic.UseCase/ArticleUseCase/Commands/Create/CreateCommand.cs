﻿using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ArticleUseCase.Commands.Create;

public class CreateCommand : ICommand<string>
{    
    //Article
    
    public string CategoryId { get; set; }
    public string Title      { get; set; }
    public string Summary    { get; set; }
    public string Body       { get; set; }
    
    //Image
    
    public string FilePath      { get; set; }
    public string FileName      { get; set; }
    public string FileExtension { get; set; }
}