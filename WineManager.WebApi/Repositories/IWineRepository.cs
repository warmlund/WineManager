using WineManager.EntityModels;

namespace WineManager.WebApi.Repositories
{
    public interface IWineRepository
    {
        Task<Wine?> CreateAsync(Wine wine);
        Task<Wine[]> RetrieveAllAsync();
        Task<Wine?> RetrieveAsync(int id);
        Task<Wine?> UpdateAsync(Wine wine);
        Task<bool?> DeleteAsync(int id);
    }
}
