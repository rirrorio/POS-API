using Microsoft.EntityFrameworkCore;
using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Services
{
    public class ItemService
    {
        private readonly ApplicationDBContext _dbContext;

        public ItemService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET Items
        public async Task<List<Item>> GetItemsAsync()
        {
            return await _dbContext.Items.ToListAsync();
        }
        
        // GET Item by Id
        public async Task<Item> GetItemByIdAsync(int id)
        {
            Item item = await _dbContext.Items.FindAsync(id);
            if (item!= null)
            {
                return item;
            } else return null;
        } 
        // POST Item
        public async Task<string> CreateItemAsync(ItemCreateDTO itemCreateDto)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", itemCreateDto.Image.FileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await itemCreateDto.Image.CopyToAsync(stream);
                }

                string imageUrl = "/images/" + itemCreateDto.Image.FileName;

                Item item = new Item
                {
                    Name = itemCreateDto.Name,
                    Price = itemCreateDto.Price,
                    Stock = itemCreateDto.Stock,
                    Category = itemCreateDto.Category,
                    ImageUrl = imageUrl,
                };

                _dbContext.Items.Add(item);
                await _dbContext.SaveChangesAsync();

                return $"Item {item.Name} is successfully created";

            }
            catch (Exception ex)
            {

                return "Service layer error when creating item";
            }
        }

        //PUT Item
        public async Task<String> UpdateItemAsync (int id, ItemCreateDTO itemCreateDto)
        {
            try
            {
                Item existingItem = await GetItemByIdAsync(id);
                if (existingItem == null)
                {
                    return "Item id not found";    
                }

                existingItem.Name = itemCreateDto.Name;
                existingItem.Price = itemCreateDto.Price;
                existingItem.Stock = itemCreateDto.Stock;
                existingItem.Category = itemCreateDto.Category;

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", itemCreateDto.Image.FileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await itemCreateDto.Image.CopyToAsync(stream);
                }

                existingItem.ImageUrl = "/images/" + itemCreateDto.Image.FileName;

                _dbContext.Items.Update(existingItem);
                await _dbContext.SaveChangesAsync();

                return $"Item {existingItem.Name} with id {id} is successfully updated";

            }
            catch (Exception ex)
            {
                return "Service layer error when updating item";
            }

        }

        // DELETE item 

        public async Task<string> DeleteItemAsync(int id)
        {
            Item item = await GetItemByIdAsync(id);
            if (item == null)
            {
                return $"Item with id {id} not found";
            }

            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
            return $"Item with item id {id} successfully deleted";
        }

    }
}
