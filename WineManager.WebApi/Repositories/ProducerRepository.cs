using Microsoft.EntityFrameworkCore.ChangeTracking;
using WineManager.EntityModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace WineManager.WebApi.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly IMemoryCache _memoryCache;

        private readonly MemoryCacheEntryOptions _cacheEntryOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(30) //Inactive cache will be deleted after 30 minutes
        };

        private WineManagerContext _db;

        public ProducerRepository(WineManagerContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="producer"></param>
        /// <returns></returns>
        public async Task<Producer?> CreateAsync(Producer producer)
        {
            EntityEntry<Producer> added = await _db.Producers.AddAsync(producer);
            int affected = await _db.SaveChangesAsync();

            if (affected == 1)
            {
                //If saved to database then it will be stored in cache
                _memoryCache.Set(producer.ProducerId, producer, _cacheEntryOptions);
                return producer;
            }

            return null;
        }

        /// <summary>
        /// Delestes a producer object. if sucessful the cached producer
        /// is deleted as well
        /// </summary>
        /// <param name="id">producer id</param>
        /// <returns>true if sucessfull</returns>
        public async Task<bool?> DeleteAsync(int id)
        {
            Producer? producer = await _db.Producers.FindAsync(id);

            if (producer == null)
                return null;

            _db.Producers.Remove(producer);
            int affected = await _db.SaveChangesAsync();

            if (affected == 1)
            {
                _memoryCache.Remove(producer.ProducerId);
                return true;
            }

            return null;
        }

        /// <summary>
        /// Returns all producers in the database as an array
        /// </summary>
        /// <returns>array of producers</returns>
        public Task<Producer[]> RetrieveAllAsync()
        {
            return _db.Producers.ToArrayAsync();
        }

        /// <summary>
        /// Retrieves a producer based on id
        /// </summary>
        /// <param name="id">producer id</param>
        /// <returns>producer</returns>
        public Task<Producer?> RetrieveAsync(int id)
        {
            //Try to retrieve data from cache for better performance
            if (_memoryCache.TryGetValue(id, out Producer? producerFromCache))
                return Task.FromResult(producerFromCache);

            Producer? producerFromdb = _db.Producers.FirstOrDefault(w => w.ProducerId == id);

            if (producerFromdb == null)
                return Task.FromResult(producerFromdb); //Return null result if the id doesn't exist in the database either

            _memoryCache.Set(producerFromdb.ProducerId, producerFromdb, _cacheEntryOptions); //if id is in database, store in cache
            return Task.FromResult(producerFromdb); //return producer
        }

        /// <summary>
        /// Updates the database with the producer entry
        /// if update is successful, the cached producer is updated as well
        /// </summary>
        /// <param name="producer">the producer updated</param>
        /// <returns>updated producer entry</returns>
        public async Task<Producer?> UpdateAsync(Producer producer)
        {
            _db.Producers.Update(producer);
            int affected = await _db.SaveChangesAsync();

            if (affected == 1)
            {
                _memoryCache.Set(producer.ProducerId, producer, _cacheEntryOptions);
                return producer;
            }

            return null;
        }
    }
}
