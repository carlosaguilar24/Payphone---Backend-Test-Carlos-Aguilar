using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Transfer;

namespace TransferService.Application.Interfaces
{
    public interface ITransferService
    {
        Task<TransferResponse> ExecuteTransferAsync(TransferRequest request, CancellationToken ct = default);
    }
}
