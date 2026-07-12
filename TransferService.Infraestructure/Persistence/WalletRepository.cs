using Microsoft.EntityFrameworkCore;
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

        public WalletRepository(TransferDbContext context)
        {
            _context = context;
        }

        public Task<Wallet?> GetWalletByIdAsync(int id) =>
            _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);

        public async Task CreateWalletAsync(Wallet wallet)
        {
            await _context.AddAsync(wallet);

        }

        public Task UpdateWalletAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            return Task.CompletedTask;
        }
    }
}
