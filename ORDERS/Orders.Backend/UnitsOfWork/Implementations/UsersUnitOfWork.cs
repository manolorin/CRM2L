using Microsoft.AspNetCore.Identity;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.UnitsOfWork.Implementations;

public class UsersUnitOfWork : IUsersUnitOfWork
{
    private readonly IUsersRepository _usersRepository;

    public UsersUnitOfWork(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public async Task<IdentityResult?> AddUserAsync(User user, string password) =>
     await _usersRepository.AddUserAsync(user, password);


    public Task AddUserToRoleAsync(User user, string roleName) =>
        _usersRepository.AddUserToRoleAsync(user, roleName);


    public Task CheckRoleAsync(string roleName) =>
        _usersRepository.CheckRoleAsync(roleName);


    public Task<User> GetUserAsync(string email) =>
        _usersRepository.GetUserAsync(email);


    public Task<bool> IsUserInRoleAsync(User user, string roleName) =>
        _usersRepository.IsUserInRoleAsync(user, roleName);

}