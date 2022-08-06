using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;

namespace Mahas.Components
{
    public static class DapperConnectionExtensions
    {
        private enum AutoDbState { Insert, Update, Delete }

        private class AutoDbProperties
        {
            public string Table { get; set; }
            public List<string> Keys { get; set; }
            public List<string> Columns { get; set; }
            public IDictionary<string, object> Parameters { get; set; }
        }

        private static AutoDbProperties AutoDbGetProperties<T>(T model, AutoDbState state) where T : class, new()
        {
            var r = new AutoDbProperties
            {
                Table = typeof(T).GetCustomAttribute<DbTableAttribute>()?.Name ?? typeof(T).Name,
                Keys = new List<string>(),
                Columns = new List<string>(),
                Parameters = new Dictionary<string, object>()
            };

            foreach (var prop in typeof(T).GetProperties())
            {
                var keyAtt = prop.GetCustomAttribute<DbKeyAttribute>();
                var columnAtt = prop.GetCustomAttribute<DbColumnAttribute>();
                var key = keyAtt?.Key ?? false;
                var autoIncrement = keyAtt?.AutoIncrement ?? false;
                var column = columnAtt?.Name ?? prop.Name;
                var create = columnAtt?.Create ?? false;
                var update = columnAtt?.Update ?? false;
                if (state == AutoDbState.Update)
                {
                    if (key)
                    {
                        r.Keys.Add(column);
                        r.Parameters.Add($"@{column}", prop.GetValue(model));
                    }
                    else
                    {
                        if (update)
                        {
                            r.Columns.Add(column);
                            r.Parameters.Add($"@{column}", prop.GetValue(model));
                        }
                    }
                }
                else if (state == AutoDbState.Insert)
                {
                    if (!autoIncrement && create)
                    {
                        r.Columns.Add(column);
                        r.Parameters.Add($"@{column}", prop.GetValue(model));
                    }
                }
                else
                {
                    if (key)
                    {
                        r.Keys.Add(column);
                        r.Parameters.Add($"@{column}", prop.GetValue(model));
                    }
                }
            }
            return r;
        }

        public static async Task<int> InsertAsync<T>(this SqlConnection sql, T model, bool autoIncrement = false, IDbTransaction transaction = null) where T : class, new()
        {
            var m = AutoDbGetProperties(model, AutoDbState.Insert);
            var query = $"INSERT INTO {m.Table} ({string.Join(", ", m.Columns)}) VALUES ({string.Join(", ", m.Columns.Select(x => "@" + x))})";
            if (autoIncrement) query += "; SELECT SCOPE_IDENTITY();";

            if (autoIncrement)
                return (int)(decimal)await sql.ExecuteScalarAsync(query, m.Parameters, transaction);
            else
                return await sql.ExecuteAsync(query, m.Parameters, transaction);
        }

        public static async Task UpdateAsync<T>(this SqlConnection sql, T model, IDbTransaction transaction = null) where T : class, new()
        {
            var m = AutoDbGetProperties(model, AutoDbState.Update);
            var query = $"UPDATE {m.Table} SET {string.Join(", ", m.Columns.Select(x => $"{x}=@{x}"))} WHERE {string.Join(" AND ", m.Keys.Select(x => $"{x}=@{x}"))}";
            
            await sql.ExecuteAsync(query, m.Parameters, transaction);
        }

        public static async Task DeleteAsync<T>(this SqlConnection sql, T model, IDbTransaction transaction = null) where T : class, new()
        {
            var m = AutoDbGetProperties(model, AutoDbState.Delete);

            var query  = $"DELETE FROM {m.Table} WHERE {string.Join(" AND ", m.Keys.Select(x => $"{x}=@{x}"))}";
            
            await sql.ExecuteAsync(query, m.Parameters, transaction);
        }
    }
}
