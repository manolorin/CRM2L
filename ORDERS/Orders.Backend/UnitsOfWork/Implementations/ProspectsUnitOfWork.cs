using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Implementations;

public class ProspectsUnitOfWork : GenericUnitOfWork<Prospect>, IProspectsUnitOfWork
{
    private readonly IProspectsRepository _prospectsRepository;
    public ProspectsUnitOfWork(IGenericRepository<Prospect> repository, IProspectsRepository prospectsRepository): base(repository)
    {
        _prospectsRepository = prospectsRepository;
    }
    public async Task<IEnumerable<Prospect>> GetComboAsync() => await _prospectsRepository.GetComboAsync();

    public override async Task<ActionResponse<IEnumerable<Prospect>>> GetAsync(PaginationDTO pagination) => await _prospectsRepository.GetAsync(pagination);

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _prospectsRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<Prospect>> AddAsync(Prospect prospect) => await  _prospectsRepository.AddAsync(prospect);
    

    public override async Task<ActionResponse<Prospect>> UpdateAsync(Prospect prospect) => await _prospectsRepository.UpdateAsync(prospect);

    public override async Task<ActionResponse<Prospect>> GetAsync(int Id) => await _prospectsRepository.GetAsync(Id);


}
