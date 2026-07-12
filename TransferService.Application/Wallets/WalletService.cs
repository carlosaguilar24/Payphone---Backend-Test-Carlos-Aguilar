using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;
using TransferService.Domain.Entities;
using TransferService.Domain.Exceptions;

namespace TransferService.Application.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<WalletResponse> CreateWalletAsync(CreateWalletRequest request, CancellationToken ct = default)
        {
            var wallet = Wallet.Create(request.DocumentId, request.Name, request.InitialBalance);
            await _walletRepository.CreateWalletAsync(wallet);
            await _unitOfWork.SaveChangesAsync(ct);

            return MapToResponseWallet(wallet);
        }

        public async Task<WalletResponse> GetWalletByIdAsync(int id, CancellationToken ct = default)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(id)
                ?? throw new WalletNotFoundException(id);

            return MapToResponseWallet(wallet);

        }

        public async Task<IReadOnlyCollection<MovementResponse>> GetMovementsByWallet(int id, CancellationToken ct = default)
        {
            return null;    
        }

        private static WalletResponse MapToResponseWallet(Wallet wallet) => new()
        {
            Id = wallet.Id,
            DocumentId = wallet.DocumentId,
            Name = wallet.Name,
            Balance = wallet.Balance,
            CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt
        };

    }
}
