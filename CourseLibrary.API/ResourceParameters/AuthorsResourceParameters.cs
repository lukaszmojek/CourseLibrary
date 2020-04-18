using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.ResourceParameters
{
    public class AuthorsResourceParameters
    {
        const int maxAuthorsPageSize = 20;

        public string MainCategory { get; set; }

        public string SearchQuery { get; set; }

        public int PageNumber { get; set; } = 1;


        private int _authorsPageSize = 10;

        public int PageSize 
        {
            get => _authorsPageSize;
            set => _authorsPageSize = (value > maxAuthorsPageSize) ? maxAuthorsPageSize : value;
        }
    }
}
