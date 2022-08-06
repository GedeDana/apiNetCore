using Dapper;
using DelegasiAPI.Models;
using System.Data.SqlClient;

namespace DelegasiAPI.Repositories
{
    public class BaseRepository
    {
        public enum OrderByType
        {
            ASC,
            DESC
        }

        protected readonly string _connectionString;

        public BaseRepository(IConfiguration configuration, string connectionName)
        {
            _connectionString = configuration.GetConnectionString(connectionName);
        }

        protected async Task<PageResult<T>> GetPaginationAsync<T>(string query, string orderBy, OrderByType orderByType, PageFilter filter, Dictionary<string, object> parameters = null) where T : new()
        {
            var pageIndex = filter.PageIndex;
            var pageSize = filter.PageSize;

            var countQuery = $"SELECT COUNT (*) FROM ({query}) ALIAS";

            var paginationQuery = $@"
                    SELECT
                        ROW_NUMBER() OVER (ORDER BY {orderBy} {orderByType}) AS RowNumber,
                        *
                    FROM ({query}) AS ALIAS
                ";

            paginationQuery = $@"
                    WITH Alias AS ({paginationQuery})
                    SELECT TOP {pageSize}
                        *
                    FROM
                        Alias
                    WHERE
                        RowNumber > (@pageIndex * @pageSize)
                    ";

            parameters.Add("pageIndex", pageIndex);
            parameters.Add("pageSize", pageSize);

            using var conn = new SqlConnection(_connectionString);

            var count = await conn.ExecuteScalarAsync<int>(countQuery, parameters);

            var result = await conn.QueryAsync<T>(paginationQuery, parameters);

            return new PageResult<T>(count, result, filter);
        }

        protected string ToWhere(List<string> wheres)
        {
            if (wheres.Count == 0) return "";
            var where = string.Join(" AND ", wheres);
            return "WHERE " + where;
        }
    }
}
