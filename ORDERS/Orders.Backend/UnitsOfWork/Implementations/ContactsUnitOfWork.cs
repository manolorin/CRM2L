using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Implementations;

public class ContactsUnitOfWork: GenericUnitOfWork<Contact>, IContactsUnitOfWork
{
    private readonly IContactsRepository _contactsRepository;
    public ContactsUnitOfWork(IGenericRepository<Contact> repository, IContactsRepository contactsRepository) : base(repository)
    {
        _contactsRepository = contactsRepository;
    }

    public override async Task<ActionResponse<Contact>> AddAsync(Contact contact) => await _contactsRepository.AddAsync(contact);

    public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsync() => await _contactsRepository.GetAsync();
    public override async Task<ActionResponse<Contact>> GetAsync(int Id) => await _contactsRepository.GetAsync(Id);
    public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsync(PaginationDTO pagination) => await _contactsRepository.GetAsync(pagination);
    public async Task<IEnumerable<Contact>> GetComboAsync(int prospectId) => await _contactsRepository.GetComboAsync(prospectId);
    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _contactsRepository.GetTotalRecordsAsync(pagination);
    public override async Task<ActionResponse<Contact>> UpdateAsync(Contact contact) => await _contactsRepository.UpdateAsync(contact);

    
}
