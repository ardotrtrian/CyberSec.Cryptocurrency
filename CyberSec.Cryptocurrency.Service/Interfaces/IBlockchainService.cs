using CyberSec.Cryptocurrency.Service.Entities;

namespace CyberSec.Cryptocurrency.Service.Interfaces;

public interface IBlockchainService
{
    Task AddBlockAsync(Transaction[] transactions);
    Task<Block> CreateGenesisBlockAsync();
    Task<double> GetBalanceForAccountAsync(string userId);
    Task<List<Transaction>> GetTransactionHistoryAsync(string userId);
}