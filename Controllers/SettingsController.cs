using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Open.ManifestToolkit.API.Dtos;
using Open.ManifestToolkit.API.Extensions;
using Open.ManifestToolkit.API.Services;

namespace Open.ManifestToolkit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingService _settingService;

        public SettingsController(SettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("environments")]
        public async Task<IActionResult> ListAllEnvironments()
        {
            return (Ok(new
            {
                Environments = await _settingService.GetAllEnvironments()
            }));
        }

        [HttpGet("sce-instances")]
        public async Task<IActionResult> ListAllSceInstances()
        {
            return (Ok(new
            {
                SceInstances = await _settingService.GetAllInstances()
            }));
        }

        [HttpPost("environments")]
        public async Task<IActionResult> CreateEnvironment([FromBody] CreateEnvironmentRequestDto request)
        {
            if (await _settingService.EnvironmentExists(request.Type))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"El entorno {request.Type} ya existe."
                });
            }

            var saved = await _settingService.CreateEnvironment(request.Type);

            if (!saved)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "No se pudo crear el entorno"
                });
            }


            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = $"Se creo el entorno {request.Type} exitosamente"
            });
        }

        [HttpPost("sce-instances")]
        public async Task<IActionResult> CreateSceInstances([FromBody] CreateInstanceRequestDto request)
        {
            if (await _settingService.SceInstanceExists(request.Instance))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"La instancia {request.Instance} ya existe."
                });
            }

            var saved = await _settingService.CreateSceInstance(request.Instance);

            if (!saved)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "No se pudo crear la instancia del sce"
                });
            }


            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = $"Se creo la instancia {request.Instance} exitosamente"
            });
        }

        [HttpDelete("environments/{id}")]
        public async Task<IActionResult> DeleteEnvironment([FromRoute] string id)
        {
            if (!await _settingService.EnvironmentExists(id))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar el entorno con id {id}"
                });
            }

            var deleted = await _settingService.DeleteEnvironment(id);

            if (!deleted)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo eliminar el entorno con id {id}"
                });
            }

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = "Se elimino el entorno con id @id".Format(new
                {
                    id = id
                })
            });
        }

        [HttpDelete("sce-instances/{id}")]
        public async Task<IActionResult> DeleteSceInstance([FromRoute] string id)
        {
            if (!await _settingService.SceInstanceExists(id))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar la instancia con id {id}"
                });
            }

            var deleted = await _settingService.DeleteSceInstance(id);

            if (!deleted)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo eliminar la instancia con id {id}"
                });
            }

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = "Se elimino la instancia con id @id".Format(new
                {
                    id = id
                })
            });
        }

        [HttpPut("sce-instances/{id}")]
        public async Task<IActionResult> UpdateSceInstance([FromRoute] string id, [FromBody] UpdateSceInstanceRequestDto request)
        {
            if (!await _settingService.SceInstanceExists(id))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar la instancia con id {id}"
                });
            }

            var udpated = await _settingService.UpdateSceInstance(request.newInstance, id);

            if (!udpated)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo actualizar la instancia con id {id}"
                });
            }

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = "Se actualizo la instancia con id @id".Format(new
                {
                    id = id
                })
            });
        }

        [HttpPut("environments/{id}")]
        public async Task<IActionResult> UpdateEnvironment([FromRoute] string id, [FromBody] UpdateEnvironmentRequestDto request)
        {
            if (!await _settingService.EnvironmentExists(id))
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo encontrar el entorno con id {id}"
                });
            }

            var udpated = await _settingService.UpdateEnvironment(request.newEnvironment, id);

            if (!udpated)
            {
                return BadRequest(new ResponseDto()
                {
                    IsSuccess = false,
                    Message = $"No se pudo actualizar el entorno con id {id}"
                });
            }

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = "Se actualizo el entorno con id @id".Format(new
                {
                    id = id
                })
            });
        }
    }
}
