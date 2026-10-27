using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
    public interface IPhasesUnitOfWork
    {
        Task<IEnumerable<Phase>> GetComboAsync();

        Task<ActionResponse<IEnumerable<Phase  >>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
    }
}
