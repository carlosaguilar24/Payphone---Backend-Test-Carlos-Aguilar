using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Entities;

namespace TransferService.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetWalletByIdAsync(int id);
        Task CreateWalletAsync(Wallet wallet);
        Task UpdateWalletAsync(Wallet wallet);
    }
}
