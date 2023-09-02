using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Open.ManifestToolkit.API.Dtos;
using Open.ManifestToolkit.API.Entities;
using Open.ManifestToolkit.API.Helpers;
using Open.ManifestToolkit.API.Services;

namespace Open.ManifestToolkit.API.Controllers
{
    [Route("api/manifests")]
    [ApiController]
    public class ManifestsController : ControllerBase
    {
        private readonly ManifestService _manifestService;
        private readonly GitService _gitService;

        public ManifestsController(ManifestService manifestService, GitService gitService)
        {
            _manifestService = manifestService;
            _gitService = gitService;
        }

        [HttpGet]
        public async Task<IActionResult> ListManifest()
        {
            return Ok(new
            {
                Manifests = await _manifestService.GetAllManifests()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateManifest([FromBody] CreateManifestRequestDto request)
        {
            var (owner, name) = GithubHelpers.GetInformationFromGithubUrl(request.GithubUrl);

            if (await _manifestService.Exists(name))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"El manifiesto {name} fue registrado anteriormente"
                });
            }


            var saved = await _manifestService.CreateManifest(new ManifestDto()
            {
                GithubUrl = request.GithubUrl,
                Owner = owner,
                Name = name,
            });


            if (!saved)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo registrar el manifiesto"
                });
            }

            var path = _gitService.Clone(request.GithubUrl);

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = $"Se registro el manifiesto con nombre '{name}' forma exitosa. Ruta {path}"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManifest([FromRoute] string id)
        {
            var manifest = await _manifestService.GetById(id);

            if (manifest is null)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar el manifiesto con id '{id}'"
                });
            }

            var deleted = await _manifestService.DeleteManifest(id);

            if (!deleted)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo eliminar el manifiesto '{manifest.Name}'"
                });
            }

            _gitService.DeleteLocalRepository(manifest.Name, false);

            return Ok(new ResponseDto()
            {
                IsSuccess = deleted,
                Message = $"Manifiesto '{manifest.Name}' eliminado"
            });
        }

        [HttpGet("{name}/repositories/branches")]
        public async Task<IActionResult> GetAllManifestBranches([FromRoute] string name, [FromQuery] GitBranchScope scope = GitBranchScope.All)
        {
            if (!await _manifestService.Exists(name))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar el manifiesto '{name}'"
                });
            }

            _gitService.InitializeLocalRepository(name, false);

            return Ok(new
            {
                Branches = _gitService.GetAllBranches(scope)
            });
        }

        [HttpPost("{name}/repositories")]
        public async Task<IActionResult> CloneManifestRepository([FromRoute] string name, [FromQuery] bool overrideRepository = false)
        {
            var manifest = await _manifestService.GetByName(name);

            if (manifest is null)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar el manifiesto con nombre '{name}'"
                });
            }

            if (_gitService.ExistLocalRepository(name) && !overrideRepository)
            {
                return Ok(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "El repositorio habia sido clonado anteriormente"
                });
            }

            if (_gitService.ExistLocalRepository(name) && overrideRepository)
            {
                _gitService.DeleteLocalRepository(name, false);
            }

            var path = _gitService.Clone(manifest.GithubUrl);

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = $"Se clono el repositorio del manifiesto con nombre '{name}' forma exitosa. Ruta local {path}"
            });
        }
    }
}
