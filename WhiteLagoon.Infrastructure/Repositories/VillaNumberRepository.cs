using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repositories
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly AppDbContext _db;

        public VillaNumberRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaNumber villaNumber)
        {
            _db.Update(villaNumber);
        }
    }
}
