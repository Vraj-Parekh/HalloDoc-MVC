using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Repositories.Repository.Interface
{
    public interface IRequestWiseFilesServices
    {
        void AddFiles(List<IFormFile> files, Request request);
        void Delete(int docId);
        void DeleteSelectedFiles(List<int> fileIds);
        string GetPath(int docId);
    }
}