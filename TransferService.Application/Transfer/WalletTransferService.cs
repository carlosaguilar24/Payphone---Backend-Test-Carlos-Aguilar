using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;
using TransferService.Domain.Entities;
using TransferService.Domain.Enums;
using TransferService.Domain.Exceptions;

namespace TransferService.Application.Transfer
{
    public class WalletTransferService : ITransferService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WalletTransferService(IWalletRepository walletRepository, IMovementRepository movementRepository, IUnitOfWork unitOfWork)
        {
            _movementRepository = movementRepository;
            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
        }

        public async Task<TransferResponse> ExecuteTransferAsync(TransferRequest request, CancellationToken ct = default)
        {
            if (request.FromWalletId == request.ToWalletId)
                throw new InvalidTransferException("The source and destination wallets cannot be the same.");

            
            var existingMovements = await _movementRepository.GetMovementByIdTransactionAsync(request.TransferId);
            if (existingMovements.Any())
            {
                return MapExistingToResult(request.TransferId, existingMovements);
            }

            var origin = await _walletRepository.GetWalletByIdAsync(request.FromWalletId)
                ?? throw new WalletNotFoundException(request.FromWalletId);

            var destination = await _walletRepository.GetWalletByIdAsync(request.ToWalletId)
                ?? throw new WalletNotFoundException(request.ToWalletId);

            origin.Debit(request.Amount);
            destination.Credit(request.Amount);

            await _walletRepository.UpdateWalletAsync(origin);
            await _walletRepository.UpdateWalletAsync(destination);

            var debitMovement = Movement.CreateMovement(origin.Id, request.Amount, MovementType.Debit, request.TransferId);
            var creditMovement = Movement.CreateMovement(destination.Id, request.Amount, MovementType.Credit, request.TransferId);

            await _movementRepository.AddMovementAsync(debitMovement);
            await _movementRepository.AddMovementAsync(creditMovement);

            await _unitOfWork.SaveChangesAsync(ct);

            return new TransferResponse
            {
                TransferId = request.TransferId,
                OriginWalletId = origin.Id,
                DestinationWalletId = destination.Id,
                Amount = request.Amount,
                CreatedAt = debitMovement.CreatedAt
            };

        }

        private static TransferResponse MapExistingToResult(Guid transferId, IReadOnlyList<Movement> movements)
        {
            var debit = movements.First(m => m.Type == MovementType.Debit);
            var credit = movements.First(m => m.Type == MovementType.Credit);

            return new TransferResponse
            {
                TransferId = transferId,
                OriginWalletId = debit.WalletId,
                DestinationWalletId = credit.WalletId,
                Amount = debit.Amount,
                CreatedAt = debit.CreatedAt
            };
        }
    }
}
