using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ConcludeCareDTO
    {
        public string Name { get; set; }
        public int RequestID { get; set; }
        public List<ConcludeFile> Files { get; set; }

    }
    public class ConcludeFile
    {
        public string FileName { get; set; }
    }
}
