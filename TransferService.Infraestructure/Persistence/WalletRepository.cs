using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;
using TransferService.Domain.Entities;

namespace TransferService.Infraestructure.Persistence
{
    public class WalletRepository : IWalletRepository
    {
        private readonly TransferDbContext _context;

        public Task<Wallet?> GetWalletByIdAsync(int id)
        {
            return null;
        }

        public Task CreateWalletAsync(Wallet wallet)
        {
            return null;
        }
        public Task UpdateWalletAsync(Wallet wallet)
        {
            return null;
        }
    }
}
