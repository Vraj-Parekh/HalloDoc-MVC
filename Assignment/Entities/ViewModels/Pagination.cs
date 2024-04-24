using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class Pagination<T>
    {
        public List<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
