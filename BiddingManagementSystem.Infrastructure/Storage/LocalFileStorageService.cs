using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Infrastructure.Storage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<LocalFileStorageService> _logger;

        public LocalFileStorageService(
            IWebHostEnvironment environment,
            ILogger<LocalFileStorageService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(string folderPath, string fileName, Stream fileStream)
        {
            try
            {
                var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Uploads", folderPath);
                
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Ensure filename is unique
                var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                
                using (var fileStream2 = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStream2);
                }

                // Return relative path from uploads folder
                return Path.Combine("Uploads", folderPath, uniqueFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file {FileName} to {FolderPath}", fileName, folderPath);
                throw;
            }
        }

        public Task DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
                throw;
            }
        }

        public bool FileExists(string filePath)
        {
            var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
            return File.Exists(fullPath);
        }
    }
}