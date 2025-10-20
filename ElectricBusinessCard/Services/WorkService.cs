using ElectricBusinessCard.Services.EntityFramework.Models;
using ElectricBusinessCard.Repository;

namespace ElectricBusinessCard.Services
{
    public class WorkService(
        CategoryRepository _categoryRepos,
        WorkRepository _workRepos)
    {
        public async Task<ElectroWork?> GetWork(int workIndex, int categoryIndex)
        {
            return await _workRepos.GetWork(workIndex, categoryIndex);
        }

        public async Task EditWork(ElectroWork dto, int categoryIndex)
        {
            await _workRepos.EditWork( dto,  categoryIndex);
        }

        public async Task DeleteWorkAsync(int workIndex, int categoryIndex)
        {
            await _workRepos.DeleteWorkAsync(workIndex, categoryIndex);
        }

        internal async Task<List<ElectroWork>> GetAllWorksAsync()
        {
            return await _workRepos.GetAllWorksAsync();
        }

        internal async Task MoveUpWork(int workIndex, int categoryIndex)
        {
            await _workRepos.MoveUpWork(workIndex, categoryIndex);
        }

        internal async Task MoveDownWork(int workIndex, int categoryIndex)
        {
            await _workRepos.MoveDownWork(workIndex, categoryIndex);
        }

        internal async Task CreateElectroWorkAsync(ElectroWork dto, int categoryIndex)
        {
            await _workRepos.AddWorkAsync(dto, categoryIndex);
        }
    }
}
