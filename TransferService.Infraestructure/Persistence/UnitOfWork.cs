using Microsoft.EntityFrameworkCore.Storage;
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
        private IDbContextTransaction? _transaction;


        public UnitOfWork(TransferDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync() =>
              _transaction = await _context.Database.BeginTransactionAsync();
        public async Task CommitAsync() =>
               await _transaction!.CommitAsync();
        public async Task RollbackAsync() =>
                    await _transaction!.RollbackAsync();
    }
}
