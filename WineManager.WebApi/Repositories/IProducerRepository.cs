using WineManager.EntityModels;

namespace WineManager.WebApi.Repositories
{
    /// <summary>
    /// Interface for managing the CRUD methods of the producer entity model
    /// </summary>
    public interface IProducerRepository
    {
        Task<Producer?> CreateAsync(Producer producer);
        Task<Producer[]> RetrieveAllAsync();
        Task<Producer?> RetrieveAsync(string name);
        Task<Producer?> UpdateAsync(Producer producer);
        Task<bool?> DeleteAsync(string name);
    }
}
