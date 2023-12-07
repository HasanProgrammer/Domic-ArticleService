using Karami.Core.UseCase.DTOs.ViewModels;

namespace Karami.UseCase.FileUseCase.DTOs.ViewModels;

public class FilesViewModel : ViewModel
{
    public string Id        { get; set; }
    public string Path      { get; set; }
    public string Name      { get; set; }
    public string Extension { get; set; }
}