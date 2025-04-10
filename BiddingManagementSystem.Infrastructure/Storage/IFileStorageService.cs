using System.IO;
using System.Threading.Tasks;

namespace BiddingManagementSystem.Infrastructure.Storage
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(string folderPath, string fileName, Stream fileStream);
        Task DeleteFileAsync(string filePath);
        bool FileExists(string filePath);
    }
}