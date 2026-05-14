using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class FileService : IFileService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string?> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString() +
                           Path.GetExtension(file.FileName);

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public void Delete(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images", fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string GetImageUrl(string? fileName)
        {

            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            // إذا URL كامل
            if (Uri.TryCreate(fileName, UriKind.Absolute, out var uri))
            {
                // لو فيه localhost → نصلحه
                if (uri.Host.Contains("localhost"))
                    fileName = Path.GetFileName(uri.LocalPath);
                else
                    return fileName; 
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";

            return $"{baseUrl}/images/{fileName}";
        }
    }
    }
