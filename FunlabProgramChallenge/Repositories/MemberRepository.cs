using FunlabProgramChallenge.Model;
using FunlabProgramChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace FunlabProgramChallenge.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private AppDbContext _context;
        public MemberRepository()
        {
            _context = new AppDbContext();
        }
        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }
        public MemberRepository(IDbContextFactory<AppDbContext> factory)
        {
            _context = factory.CreateDbContext();
        }

        public async Task<Member> GetMemberAsync(int id)
        {
            return await _context.Member.SingleOrDefaultAsync(x => x.MemberId == id);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            return await _context.Member.ToListAsync();
        }

        public async Task<int> InsertOrUpdatetMemberAsync(Member model)
        {
            if (model.MemberId == 0)
            {
                await _context.Member.AddAsync(model);
            }
            else
            {
                _context.Member.Update(model);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> InsertMemberAsync(Member model)
        {
            try
            {
                if (model.MemberId == 0)
                {
                    await _context.Member.AddAsync(model);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateMemberAsync(Member model)
        {
            if (model.MemberId > 0)
            {
                _context.Member.Update(model);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMemberAsync(Member model)
        {
            if (model.MemberId > 0)
            {
                _context.Member.Remove(model);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> InsertMemberAsync(List<Member> modelList)
        {
            try
            {
                // Create an instance and save the entity to the database

                _context.AddRange(modelList);

                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public interface IMemberRepository
    {
        Task<Member> GetMemberAsync(int id);
        Task<IEnumerable<Member>> GetMembersAsync();
        Task<int> InsertOrUpdatetMemberAsync(Member model);
        Task<int> InsertMemberAsync(Member model);
        Task<int> UpdateMemberAsync(Member model);
        Task<int> DeleteMemberAsync(Member model);
        Task<int> InsertMemberAsync(List<Member> modelList);
    }
}
