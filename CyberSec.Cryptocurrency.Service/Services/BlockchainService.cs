using CyberSec.Cryptocurrency.Service.Entities;
using CyberSec.Cryptocurrency.Service.Interfaces;
using CyberSec.Cryptocurrency.Service.Persistence;

namespace CyberSec.Cryptocurrency.Service.Services;

internal class BlockchainService
{
    private readonly CryptocurrencyContext _context;
    private readonly IDate _date;

    public BlockchainService(
        CryptocurrencyContext context,
        IDate date)
    {
        _context = context;
        _date = date;
    }

    public async Task AddBlockAsync(Transaction[] transactions)
    { 

    }

    public async Task<Block> CreateGenesisBlockAsync()
    {
        return null;
    }

    public async Task<double> GetBalanceForAccountAsync(string userId)
    {
        return 0.0;
    }

    public async Task<IList<Transaction>> GetTransactionHistoryAsync(string userId)
    {
        return null;
    }
}