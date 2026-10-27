using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Implementations;

public class PhasesUnitOfWork: GenericUnitOfWork<Phase>, IPhasesUnitOfWork
{
    private readonly IPhasesRepository _phasesRepository;

    public PhasesUnitOfWork(IGenericRepository<Phase> repository, IPhasesRepository phasesRepository) : base(repository)
    {
        _phasesRepository = phasesRepository;
    }

    public async Task<IEnumerable<Phase>> GetComboAsync() => await _phasesRepository.GetComboAsync();

    public override async Task<ActionResponse<IEnumerable<Phase>>> GetAsync(PaginationDTO pagination) => await _phasesRepository.GetAsync(pagination);

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _phasesRepository.GetTotalRecordsAsync(pagination);
}