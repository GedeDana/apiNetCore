using Microsoft.AspNetCore.Mvc;
using Dapper;
using DelegasiAPI.Models;
using DelegasiAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Mahas.Components;
using DelegasiAPI.Models.Validator;

namespace DelegasiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController : ControllerBase
    {

        private readonly EmployeeRepository _employeeRepository;
        public EmployeeController(EmployeeRepository employeeRepository)
        {
          
            _employeeRepository = employeeRepository;
        }


        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> PostAsync([FromBody] EmployeeModel model)
        {
            var validationResult = new EmployeeModelValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            var result =  await _employeeRepository.PostAsync(model);

            var uri = Url.Action("Get", new { id = result.Id });

            return Created(uri,result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetAsync(int id)
        {
           
            var result = await _employeeRepository.GetAsync(id);

            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeModel>>> GetAsync([FromQuery] PageFilter filter, string? nama = null)
        {
             var result =  await _employeeRepository.GetAsync(nama, filter);

            return Ok(result); 
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody] EmployeeModel model)
        {

            var validationResult = new EmployeeModelValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var result = await _employeeRepository.PutAsync(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            //using var conn = new SqlConnection(_connectionString);

            //await conn.OpenAsync();

            //var query = "SELECT TOP 1 * FROM EmployeeTable WHERE Id = @id";

            //var model = conn.QueryFirstOrDefault<EmployeeModel>(query, new { id });

            //if (model == null) return NotFound();

            //using var transaction = conn.BeginTransaction();

            //await conn.DeleteAsync(model, transaction);

            //await transaction.CommitAsync();
             await _employeeRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
