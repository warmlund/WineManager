using WineManager.EntityModels;

namespace WineManager.WebApi.Repositories
{
    /// <summary>
    /// Interface for managing the CRUD methods of the wine entity model
    /// </summary>
    public interface IWineRepository
    {
        Task<Wine?> CreateAsync(Wine wine);
        Task<Wine[]> RetrieveAllAsync();
        Task<Wine?> RetrieveAsync(int id);
        Task<Wine?> UpdateAsync(Wine wine);
        Task<bool?> DeleteAsync(int id);
    }
}
