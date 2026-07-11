using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;
using TransferService.Application.ModelResponse;
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
        public async Task<Wallet> CreateWalletAsync(CreateWalletRequest request, CancellationToken ct = default)
        {
            var wallet = Wallet.Create(request.DocumentId, request.Name, request.InitialBalance);
            await _walletRepository.CreateWalletAsync(wallet);
            return wallet;
        }

        public async Task<Wallet?> GetWalletByIdAsync(int id, CancellationToken ct = default)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(id)
                ?? throw new WalletNotFoundException(id);

            return wallet;

        }

    }
}
