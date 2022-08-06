using Microsoft.AspNetCore.Mvc;
using Dapper;
using DelegasiAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Mahas.Components;

namespace DelegasiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {

        private readonly string _connectionString;

        public SampleController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MainConnection");
        }

        [HttpPost]
        public async Task<ActionResult<SampleModel>> PostAsync([FromBody] SampleModel model)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            model.Id = await conn.InsertAsync(model, true, transaction);

            await transaction.CommitAsync();

            var uri = Url.Action("Get", new { id = model.Id });

            return Created(uri, model);
        }

        [HttpGet]
        public ActionResult<List<SampleModel>> Get(string? nama = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                List<SampleModel> result;

                var query = "SELECT * FROM SampleTable";

                Dictionary<string, object> param = new();

                if (!string.IsNullOrEmpty(nama))
                {
                    query += " WHERE Name LIKE @nama";

                    param.Add("nama", $"%{nama}%");


                }

                result = conn.Query<SampleModel>(query, param).ToList();

                if(result == null)
                {
                    return NotFound($"Data Tidak diketemukan berdasarakan nama {nama}");
                }

                return Ok(result);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<SampleModel> Get(int? id = null)
        {
            using(var conn = new SqlConnection(_connectionString))
            {

             
                var query = "SELECT TOP 1 * FROM SampleTable WHERE Id = @id";

                var result = conn.QueryFirstOrDefault<SampleModel>(query, new { id });

               

                return Ok(result);


                 //Version version
                //List<SampleModel> result;

                //var query = "SELECT * FROM SampleTable";

                //Dictionary<string, object> param = new();

                //if (id.HasValue)
                //{

                //    query += " WHERE Id LIKE @Id";
                //    param.Add("id", $"{id}");

                //}
                //result = conn.Query<SampleModel>(query, param).ToList();
                //if (id == null || id == 0)
                //{
                //    return Ok(result);
                //}
                //if (result.Count == 0)
                //{
                //    return NotFound(new { message = "Data Tidak Diketemukan" });

                //}

                //return Ok(result);
            }
       
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SampleModel>> PutAsync([FromBody] SampleModel model, int id)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            var query = "SELECT TOP 1 * FROM SampleTable WHERE Id = @id";

            var exctModel = conn.QueryFirstOrDefault<SampleModel>(query, new { id });

            if(exctModel == null)
            {
                return NotFound();
            }
            exctModel.Name = model.Name;
            using var transaction = conn.BeginTransaction();


            await conn.UpdateAsync(exctModel, transaction);

            await transaction.CommitAsync();

            return Ok(exctModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            var query = "SELECT TOP 1 * FROM SampleTable WHERE Id = @id";

            var model = conn.QueryFirstOrDefault<SampleModel>(query, new { id });

            if (model == null) return NotFound();

            using var transaction = conn.BeginTransaction();

            await conn.DeleteAsync(model, transaction);

            await transaction.CommitAsync();

            return NoContent();
        }
    }
}
