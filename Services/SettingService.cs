using Microsoft.EntityFrameworkCore;
using Open.ManifestToolkit.API.Data;
using Open.ManifestToolkit.API.Entities;

namespace Open.ManifestToolkit.API.Services
{
    public class SettingService
    {
        private readonly ApplicationDbContext _context;

        public SettingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateEnvironment(string type)
        {
            _context.Environments.Add(new Entities.Environment()
            {
                Type = type,
                Id = Guid.NewGuid().ToString()
            });

            int rows = await _context.SaveChangesAsync();


            return rows > 0;
        }

        public async Task<bool> CreateSceInstance(string instanceName)
        {
            _context.SceInstances.Add(new Entities.SceInstance()
            {
                Instance = instanceName,
                Id = Guid.NewGuid().ToString()
            });

            int rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> DeleteEnvironment(string nameOrId)
        {
            var environment = await _context.Environments.FirstOrDefaultAsync(e => e.Id == nameOrId || e.Type.ToLower() == nameOrId.ToLower());

            if (environment is null)
            {
                return false;
            }

            _context.Environments.Remove(environment);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> DeleteSceInstance(string nameOrId)
        {
            var sceInstance = await _context.SceInstances.FirstOrDefaultAsync(e => e.Id == nameOrId || e.Instance.ToLower() == nameOrId.ToLower());

            if (sceInstance is null)
            {
                return false;
            }

            _context.SceInstances.Remove(sceInstance);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> UpdateSceInstance(string newInstanceName, string nameOrId)
        {
            var sceInstance = await _context.SceInstances.FirstOrDefaultAsync(e => e.Id == nameOrId || e.Instance.ToLower() == nameOrId.ToLower());

            if (sceInstance is null)
            {
                return false;
            }

            sceInstance.Instance = newInstanceName;

            _context.SceInstances.Update(sceInstance);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> UpdateEnvironment(string newEnvironment, string nameOrId)
        {
            var environment = await _context.Environments.FirstOrDefaultAsync(e => e.Id == nameOrId || e.Type.ToLower() == nameOrId.ToLower());

            if (environment is null)
            {
                return false;
            }

            environment.Type = newEnvironment;

            _context.Environments.Update(environment);

            var rows = await _context.SaveChangesAsync();

            return rows > 0;
        }

        public async Task<bool> EnvironmentExists(string nameOrId)
        {
            return await (_context.Environments.FirstOrDefaultAsync(s => s.Type.ToLower() == nameOrId.ToLower() || s.Id == nameOrId)) != null;
        }

        public async Task<bool> SceInstanceExists(string nameOrId)
        {
            return await (_context.SceInstances.FirstOrDefaultAsync(s => s.Instance.ToLower() == nameOrId.ToLower() || s.Id == nameOrId)) != null;
        }

        public async Task<List<Entities.Environment>> GetAllEnvironments()
        {
            return await _context.Environments.ToListAsync();
        }

        public async Task<List<SceInstance>> GetAllInstances()
        {
            return await _context.SceInstances.ToListAsync();
        }
    }
}
