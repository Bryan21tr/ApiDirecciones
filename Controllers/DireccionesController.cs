using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIDirecciones.Model;
using APIDirecciones.Model.DAO;
using APIDirecciones.Model.Entities;
using APIDirecciones.Model.IDAO.IServiceDAO;

namespace APIDirecciones.Api.V1.Controller
{
    [ApiController]
    [Route("apiDirecciones/[controller]")]
    public class DireccionesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceDirecciones _iServiceDirecciones;

        public DireccionesController(
            
            IConfiguration configuration,
            IServiceDirecciones iServiceDirecciones)
        {
           
            _configuration = configuration;
            _iServiceDirecciones = iServiceDirecciones;

        }


        [HttpGet("GetByID")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _iServiceDirecciones.ObtenerporID(id);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.Messages });
            }
        }
        
        [HttpGet("Get")]
        public async Task<IActionResult> GetDirecciones()
        {
            var result = await _iServiceDirecciones.ObtenerAll();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.Messages });
            }
        }


        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] DireccionEntidad direccion)
        {
            var result = await _iServiceDirecciones.Add(direccion);

            if (result.Success)
            {
                return Ok(result);         
                   }
            else
            {
                return BadRequest(new { message = result.Messages });
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Patch([FromBody] DireccionEntidad direccion, int Id)
        {
            direccion.Id = Id;
            var result = await _iServiceDirecciones.UpdateAsync(direccion, Id);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.Messages });
            }
        }
      
         [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _iServiceDirecciones.DeleteAsync(Id);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.Messages });
            }
        }
        
    }
}

