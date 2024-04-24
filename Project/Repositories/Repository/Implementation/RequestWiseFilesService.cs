using Entities.DataContext;
using Microsoft.AspNetCore.Hosting;
using Repositories.Repository.Interface;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;

namespace Repositories.Repository.Implementation
{
    public class RequestWiseFilesService:IRequestWiseFilesServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment env;

        public RequestWiseFilesService(HalloDocDbContext _context, IHostingEnvironment env)
        {
            this._context = _context;
            this.env = env;
        }

        public string GetPath(int docId)
        {
            Requestwisefile? file = _context.Requestwisefiles.Where(a => a.Requestwisefileid == docId).FirstOrDefault();

            string? uploads = Path.Combine(env.WebRootPath, "uploads");
            string? filePath = Path.Combine(uploads, file.Filename);

            return filePath;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }

        public void AddFiles(List<IFormFile> files, Entities.Models.Request request)
        {
            if (files is not null)
            {

                foreach (var item in files)
                {
                    IFormFile? file = item;
                    string? uniqueFileName = GetUniqueFileName(file.FileName);
                    string? uploads = Path.Combine(env.WebRootPath, "uploads");
                    string? filePath = Path.Combine(uploads, uniqueFileName);
                    file.CopyTo(new FileStream(filePath, FileMode.Create));

                    Requestwisefile? requestWiseFile = new Requestwisefile
                    {
                        Createddate = DateTime.Now,
                        Filename = uniqueFileName,
                        Isdeleted = false,
                        Request = request,
                    };
                    _context.Requestwisefiles.Add(requestWiseFile);
                    _context.SaveChanges();
                }
            }
        }

        public void Delete(int docId)
        {
            Requestwisefile? file = _context.Requestwisefiles.Where(a => a.Requestwisefileid == docId).FirstOrDefault();

            if (file != null)
            {
                file.Isdeleted = true;
                file.Createddate = DateTime.Now;
            }
            _context.Requestwisefiles.Update(file);
            _context.SaveChanges();
        }

        public void DeleteSelectedFiles(List<int> fileIds)
        {
            foreach (var docId in fileIds)
            {
                Requestwisefile? file = _context.Requestwisefiles.FirstOrDefault(a => a.Requestwisefileid == docId);

                if (file != null)
                {
                    file.Isdeleted = true;
                    file.Createddate = DateTime.Now;

                    _context.Requestwisefiles.Update(file);
                }
            }
            _context.SaveChanges();
        }
    }
}
