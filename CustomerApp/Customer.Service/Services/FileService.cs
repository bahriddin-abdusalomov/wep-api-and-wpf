using Customer.Service.Interfaces;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Customer.Service.Services
{
    public class FileService : IFileService
    {
        private readonly string _imagesDirectory = "images";
        private readonly string _rootPath;

        public FileService(IWebHostEnvironment env)
        {
            _rootPath = env.WebRootPath;
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            string fullPath = GetFullPath(imagePath);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                return true;
            }

            return false;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            string newImageName = GenerateUniqueImageName(image.FileName);
            string subPath = Path.Combine(_imagesDirectory, newImageName);
            string fullPath = Path.Combine(_rootPath, subPath);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return subPath;
        }

        private string GetFullPath(string relativePath)
        {
            return Path.Combine(_rootPath, relativePath);
        }

        private static string GenerateUniqueImageName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            string uniqueName = $"IMG_{Guid.NewGuid()}{extension}";
            return uniqueName;
        }

        public static string[] GetSupportedImageExtensions()
        {
            return new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".bmp",
                ".svg"
            };
        }
    }
}
