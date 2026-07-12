using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Wallets;
using TransferService.Domain.Entities;

namespace TransferService.Application.Interfaces
{
    public interface IWalletService
    {
        Task<WalletResponse> CreateWalletAsync(CreateWalletRequest request, CancellationToken ct = default);
        Task<WalletResponse> GetWalletByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyCollection<MovementResponse>> GetMovementsByWallet(int id, CancellationToken ct = default);    

    }
}
