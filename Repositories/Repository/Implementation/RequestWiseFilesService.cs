using Entities.DataContext;
using Microsoft.AspNetCore.Hosting;
using Repositories.Repository.Interface;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    }
}
