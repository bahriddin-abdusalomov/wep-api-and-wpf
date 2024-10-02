namespace Customer.Service.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile image);
        Task<bool> DeleteImageAsync(string imagePath);
    }
}
