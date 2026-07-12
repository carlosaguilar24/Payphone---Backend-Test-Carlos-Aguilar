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


        public UnitOfWork(TransferDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
