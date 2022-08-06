using Dapper;
using DelegasiAPI.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mahas.Components;
using DelegasiAPI.Exceptions;
using DelegasiAPI.Repositories;

namespace DelegasiAPI.Repository
{
    public class EmployeeRepository : BaseRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration, "MainConnection")
        {

        }

        public async Task<EmployeeModel> PostAsync(EmployeeModel model)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            model.Id = await conn.InsertAsync(model, true, transaction);

            await transaction.CommitAsync();

            return model;
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = "SELECT TOP 1 * FROM EmployeeTable WHERE Id = @id";



            var result = await conn.QueryFirstOrDefaultAsync<EmployeeModel>(query, new { Id = id });

            return result;

        }

        public async Task<PageResult<EmployeeModel>> GetAsync(string? name, PageFilter filter)
        {
            Dictionary<string, object> param = new();
            List<string> where = new();

            if (!string.IsNullOrEmpty(name))
            {
                where.Add("EmployeeName LIKE @name");
                param.Add("name", $"%{name}%");
            }

            var query = $"SELECT * FROM EmployeeTable {ToWhere(where)}";

            var result = await GetPaginationAsync<EmployeeModel>(query, "EmployeeName", OrderByType.ASC, filter, param);

            return result;
        }

        public async Task<EmployeeModel> PutAsync(EmployeeModel model)
        {

            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            var query = "SELECT TOP 1 * FROM EmployeeTable WHERE Id = @id";

            var dbModel = await conn.QueryFirstOrDefaultAsync<EmployeeModel>(query, new { model.Id });

            if (dbModel == null) throw new DefaultException("Data tidak ditemukan", model);


            using var transaction = conn.BeginTransaction();

            await conn.UpdateAsync(model, transaction);

            await transaction.CommitAsync();

            return model;
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            var query = "SELECT TOP 1 * FROM EmployeeTable WHERE Id = @id";

            var model = conn.QueryFirstOrDefault<EmployeeModel>(query, new { id });

            if (model == null) throw new DefaultException("Data tidak ditemukan", model);

            using var transaction = conn.BeginTransaction();

            await conn.DeleteAsync(model, transaction);

            await transaction.CommitAsync();


        }

    }
}
