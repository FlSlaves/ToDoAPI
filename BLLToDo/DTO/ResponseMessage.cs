using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLToDo.DTO
{
    public class ResponseMessage
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
