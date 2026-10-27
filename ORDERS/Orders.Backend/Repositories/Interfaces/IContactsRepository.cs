using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface IContactsRepository
    {
        Task<ActionResponse<Contact>> AddAsync(Contact contact);
        Task<IEnumerable<Contact>> GetComboAsync(int prospectId);
        Task<ActionResponse<IEnumerable<Contact>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
        Task<ActionResponse<Contact>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<Contact>>> GetAsync();
        Task<ActionResponse<Contact>> UpdateAsync(Contact contact);
    }
}
