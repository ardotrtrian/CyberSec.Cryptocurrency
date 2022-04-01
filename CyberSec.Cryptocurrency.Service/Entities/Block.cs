using CyberSec.Cryptocurrency.Service.Helpers;
using System.Security.Cryptography;

namespace CyberSec.Cryptocurrency.Service.Entities;

public class Block
{
    public int Height { get; set; }                     //a sequence number of blocks
    public long TimeStamp { get; set; }                 // the time when the block was created
    public byte[] PrevHash { get; set; }                //hash of the previous block
    public byte[] Hash { get; set; }                    // the hash of the block. The hash can be imagined as the unique identity of the block
    public Transaction[] Transactions { get; set; }     //collections of transactions that occur
    public string Creator { get; set; }                 //who creates the block identified by the public key.

    public Block(int height, byte[] prevHash, List<Transaction> transactions, string creator)
    {
        Height = height;
        PrevHash = prevHash;
        TimeStamp = DateTime.Now.Ticks;
        Transactions = transactions.ToArray();
        Hash = GenerateHash();
        Creator = creator;
    }

    // generate hash of current block
    private byte[] GenerateHash()
    {
        var sha = SHA256.Create();
        byte[] timeStamp = BitConverter.GetBytes(TimeStamp);

        var transactionHash = Transactions.ConvertToByte();

        byte[] headerBytes = new byte[timeStamp.Length + PrevHash.Length + transactionHash.Length];

        Buffer.BlockCopy(timeStamp, 0, headerBytes, 0, timeStamp.Length);
        Buffer.BlockCopy(PrevHash, 0, headerBytes, timeStamp.Length, PrevHash.Length);
        Buffer.BlockCopy(transactionHash, 0, headerBytes, timeStamp.Length + PrevHash.Length, transactionHash.Length);

        byte[] hash = sha.ComputeHash(headerBytes);

        return hash;

    }
}