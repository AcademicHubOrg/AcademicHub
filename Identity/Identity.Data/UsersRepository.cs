﻿using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public class UsersRepository : IUsersRepository
{
    private readonly IdentityDbContext _context;


    public UsersRepository(IdentityDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(User user)
    {
        _context.Add(user);
        await _context.SaveChangesAsync();
    }
    public async Task<List<User>> ListAsync(int start_index =0, int how_many=10)
    {
        return await _context.Users
            .Skip(start_index)
            .Take(how_many)
            .ToListAsync();
    }

    public async Task<User> FindByEmailAsync(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))!;
    }

    public async Task ChangeUserAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        user!.IsAdmin = true;
        await _context.SaveChangesAsync();
    }
}