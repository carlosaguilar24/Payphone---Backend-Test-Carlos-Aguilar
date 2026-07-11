using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;

namespace TransferService.Infraestructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransferDbContext _context;

        public Task BeginTransactionAsync()
        {
            return null;
        }


        public Task CommitAsync()
        {
            return null;
        }
        public Task RollbackAsync()
        {
            return null;
        }
    }
}
