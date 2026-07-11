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

        public Task AddMovementAsync(Movement movement)
        {
            return null;
        }

        public Task<IReadOnlyList<Movement>> GetMovementByWalletIdAsync(int walletId)
        {
            return null;
        }

        public Task<IReadOnlyList<Movement>> GetMovementByIdTransactionAsync(Guid transactionId)
        {
            return null;
        }
    }
}
