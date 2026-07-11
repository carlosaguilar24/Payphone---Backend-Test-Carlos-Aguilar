using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Entities;

namespace TransferService.Application.Interfaces
{
    public interface IMovementRepository
    {
        Task AddMovementAsync(Movement movement);
        Task<IReadOnlyList<Movement>> GetMovementByWalletIdAsync(int walletId);
        Task<IReadOnlyList<Movement>> GetMovementByIdTransactionAsync(Guid transferId);

    }
}
