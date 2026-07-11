using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Enums;

namespace TransferService.Application.ModelResponse
{
    public class BaseResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
