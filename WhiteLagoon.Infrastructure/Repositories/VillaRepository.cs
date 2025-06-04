using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly AppDbContext _db;

        public VillaRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Villa villa)
        {
            _db.Update(villa);
        }

   
    }
}
