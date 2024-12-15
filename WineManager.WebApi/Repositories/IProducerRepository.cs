using WineManager.EntityModels;

namespace WineManager.WebApi.Repositories
{
    public interface IProducerRepository
    {
        Task<Producer?> CreateAsync(Producer producer);
        Task<Producer[]> RetrieveAllAsync();
        Task<Producer?> RetrieveAsync(int id);
        Task<Producer?> UpdateAsync(Producer producer);
        Task<bool?> DeleteAsync(int id);
    }
}
