using Microsoft.EntityFrameworkCore;
using Open.ManifestToolkit.API.Data;
using Open.ManifestToolkit.API.Dtos;
using Open.ManifestToolkit.API.Entities;

namespace Open.ManifestToolkit.API.Services
{
    public class ManifestService
    {
        private readonly ApplicationDbContext _context;

        public ManifestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Exists(string name)
        {
            return await (_context.Manifests.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower())) != null;
        }

        public async Task<List<Manifest>> GetAllManifests()
        {
            return await _context.Manifests.ToListAsync();
        }

        public async Task<Manifest> GetById(string id)
        {
            return await _context.Manifests.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Manifest> GetByName(string name)
        {
            return await _context.Manifests.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CreateManifest(ManifestDto dto)
        {
            _context.Manifests.Add(new Manifest()
            {
                GithubUrl = dto.GithubUrl,
                Name = dto.Name,
                Owner = dto.Owner,
                Id = Guid.NewGuid().ToString()
            });

            int rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> DeleteManifest(string Id)
        {
            var manifest = await _context.Manifests.FirstOrDefaultAsync(m => m.Id == Id);

            if (manifest is null)
            {
                return false;
            }

            _context.Manifests.Remove(manifest);

            int rows = await _context.SaveChangesAsync();

            return rows > 0;
        }
    }
}
