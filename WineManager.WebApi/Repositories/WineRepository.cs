using Microsoft.EntityFrameworkCore.ChangeTracking;
using WineManager.EntityModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace WineManager.WebApi.Repositories
{
    public class WineRepository : IWineRepository
    {
        private readonly IMemoryCache _memoryCache;

        private readonly MemoryCacheEntryOptions _cacheEntryOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(30) //Inactive cache will be deleted after 30 minutes
        };

        private WineManagerContext _db; // use instnance data context to prevent it from being cached

        public WineRepository(WineManagerContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Creates a wine object
        /// </summary>
        /// <param name="wine"></param>
        /// <returns></returns>
        public async Task<Wine?> CreateAsync(Wine wine)
        {
            EntityEntry<Wine> added = await _db.Wines.AddAsync(wine);
            int affected = await _db.SaveChangesAsync();

            if(affected==1)
            {
                //If saved to database then it will be stored in cache
                _memoryCache.Set(wine.WineId,wine,_cacheEntryOptions);
                return wine;
            }

            return null;
        }

        /// <summary>
        /// Delestes a wine object. if sucessful the cached wine
        /// is deleted as well
        /// </summary>
        /// <param name="id">wine id</param>
        /// <returns>true if sucessfull</returns>
        public async Task<bool?> DeleteAsync(int id)
        {
            Wine? wine=await _db.Wines.FindAsync(id);

            if(wine==null)
                return null;

            _db.Wines.Remove(wine);
            int affected=await _db.SaveChangesAsync();

            if(affected==1)
            {
                _memoryCache.Remove(wine.WineId);
                return true;
            }

            return null;
        }


        /// <summary>
        /// Returns all wines in the database as an array
        /// </summary>
        /// <returns>array of wines</returns>
        public Task<Wine[]> RetrieveAllAsync()
        {
            return _db.Wines.ToArrayAsync();
        }

        /// <summary>
        /// Retrieves a wine based on id
        /// </summary>
        /// <param name="id">wine id</param>
        /// <returns>wine</returns>
        public Task<Wine?> RetrieveAsync(int id)
        {
            //Try to retrieve data from cache for better performance
            if(_memoryCache.TryGetValue(id, out Wine? wineFromCache))
                return Task.FromResult(wineFromCache);

            Wine? wineFromdb=_db.Wines.FirstOrDefault(w => w.WineId == id);

            if (wineFromdb == null)
                return Task.FromResult(wineFromdb); //Return null result if the id doesn't exist in the database either

            _memoryCache.Set(wineFromdb.WineId, wineFromdb, _cacheEntryOptions); //if id is in database, store in cache
            return Task.FromResult(wineFromdb); //return wine
        }

        /// <summary>
        /// Updates the database with the wine entry
        /// if update is successful, the cached wine is updated as well
        /// </summary>
        /// <param name="wine">the wine updated</param>
        /// <returns>updated wine entry</returns>
        public async Task<Wine?> UpdateAsync(Wine wine)
        {
            _db.Wines.Update(wine);
            int affected = await _db.SaveChangesAsync();

            if (affected == 1)
            {
                _memoryCache.Set(wine.WineId, wine, _cacheEntryOptions);
                return wine;
            }

            return null;
        }
    }
}
