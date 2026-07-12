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
        private readonly IMovementRepository _movementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IWalletRepository walletRepository, IUnitOfWork unitOfWork, IMovementRepository movementRepository)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _movementRepository = movementRepository;
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

        public async Task<IReadOnlyCollection<MovementResponse>> GetMovementsByWallet(int walletId, CancellationToken ct = default)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId)
                    ?? throw new WalletNotFoundException(walletId);

            var movements = await _movementRepository.GetMovementsByWalletIdAsync(walletId);

            return movements.Select(MapToResponseMovements).ToList();
        }

        private static WalletResponse MapToResponseWallet(Wallet wallet) => new()
        {
            Id = wallet.Id,
            DocumentId = wallet.DocumentId,
            Name = wallet.Name,
            Balance = wallet.Balance.Amount,
            CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt
        };

        private static MovementResponse MapToResponseMovements(Movement movement) => new()
        {
            WalletId = movement.WalletId,
            Amount = movement.Amount,
            Type = movement.Type.ToString(),
            CreatedAt = movement.CreatedAt,
            TransferId = movement.TransferId
        };

    }
}
