using Microsoft.EntityFrameworkCore;
using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Services
{
    public class TransactionService
    {
        private readonly ApplicationDBContext _dbContext;

        public TransactionService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST Transaction
        public async Task<string> CreateTransactionAsync ( TransactionDTO transactionDTO)
        {
            try
            {
                Item item = await _dbContext.Items.FindAsync(transactionDTO.ItemId);
                if (item == null)
                {
                    return $"Item with item id {transactionDTO.ItemId} not found";
                }

                //validate stock
                if (item.Stock < transactionDTO.Quantity)
                {
                    return "Insufficient Stock for the Item";
                }

                //create transaction
                Transaction transaction = new Transaction
                {
                    ItemId = transactionDTO.ItemId,
                    Quantity = transactionDTO.Quantity,
                    TotalPrice = item.Price * transactionDTO.Quantity,
                    Date = DateTime.UtcNow
                };

                //update stock
                item.Stock -= transactionDTO.Quantity;

                //save transaction to db
                _dbContext.Transactions.Add(transaction);
                //update item stock in db
                _dbContext.Items.Update(item);

                await _dbContext.SaveChangesAsync();

                return "Transaction successfully created";
            }
            catch (Exception ex)
            {
                return $"{ex.Message} : {ex.InnerException}";
            }

        }
       
        //GET all transactions 
        public async Task<List<Transaction>> GetAllTransctionAsync()
        {
            return await _dbContext.Transactions
                .Include(transaction=> transaction.Item)
                .ToListAsync();
        }
    }
}
