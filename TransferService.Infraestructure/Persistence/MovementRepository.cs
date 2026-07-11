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
    public class MovementRepository : IMovementRepository
    {
        private readonly TransferDbContext _context;

        public MovementRepository(TransferDbContext context)
        {
            _context = context;
        }

        public async Task AddMovementAsync(Movement movement)
        {
            _context.Add(movement);
            await _context.SaveChangesAsync();  
        }

        public async Task<IReadOnlyList<Movement>> GetMovementByWalletIdAsync(int walletId) =>
            await _context.Movements
                .Where(m => m.WalletId == walletId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<Movement>> GetMovementByIdTransactionAsync(Guid transferId) =>
              await _context.Movements
               .Where(m => m.TransferId == transferId)
               .OrderByDescending(m => m.CreatedAt)
               .ToListAsync();
    }
}
