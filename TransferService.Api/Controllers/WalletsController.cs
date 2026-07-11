using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TransferService.Application.Interfaces;
using TransferService.Application.Wallets;
using TransferService.Domain.Entities;

namespace TransferService.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("wallet")]
        public async Task<IActionResult> CreateWallet(
        [FromBody] CreateWalletRequest request,
        [FromServices] IValidator<CreateWalletRequest> validator,
        CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { errors });
            }

            var wallet = await _walletService.CreateWalletAsync(request, ct);
            return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
        }


        [HttpGet("wallet/{id}")]
        public async Task<IActionResult> GetWallet(int id, CancellationToken ct)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id, ct);
            return Ok(wallet);
        }

    }
}
