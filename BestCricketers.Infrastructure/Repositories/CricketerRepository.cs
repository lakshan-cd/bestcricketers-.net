using BestCricketers.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestCricketers.Infrastructure.Repositories
{
    /// <summary>
    /// This is the concrete implementation of ICricketerRepository.
    /// It uses Dapper to run raw SQL queries against the database.
    ///
    /// Key concept: This class knows HOW to talk to the database.
    /// The rest of the application does not need to know these details.
    /// </summary>
    public class CricketerRepository : ICricketerRepository
    {
        private readonly string _connectionString;

        public CricketerRepository (string connectionString)
        {

            _connectionString = connectionString;
        }


        /// <summary>
        /// Creates a new SQL connection.
        /// We use "using" to ensure the connection is properly closed after use.
        /// Dapper works with standard IDbConnection objects.
        /// </summary>
        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);


        /// <summary>
        /// GET ALL — Returns every cricketer from the database.
        /// Dapper's QueryAsync maps each row to a CricketerProfile object automatically,
        /// matching column names to property names.
        /// </summary> 
        public async Task<IEnumerable<CricketerProfile>> GetAllAsync()
        {
            const string sql = "SELECT ID as Id,Name,ODI,Tests,ODIRuns, TestRuns FROM CricketerProfile";

            using var connection = CreateConnection();
            return await connection.QueryAsync<CricketerProfile>(sql);
        }

        /// <summary>
        /// GET BY ID — Returns a single cricketer, or null if not found.
        /// We use @Id as a parameter — this prevents SQL injection attacks.
        /// Never use string concatenation to build SQL queries!
        /// </summary>
        public async Task<CricketerProfile?> GetByIdAsync(int id)
        {
            const String sql = "SELECT ID as Id,Name,ODI,Tests,ODIRuns, TestRuns FROM CricketerProfile WHERE ID = @id";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CricketerProfile>(sql, new { Id = id });
        }

        /// <summary>
        /// ADD — Inserts a new cricketer and returns the new ID.
        /// SCOPE_IDENTITY() returns the ID of the row we just inserted.
        /// </summary>
        public async Task<int> AddAsync(CricketerProfile cricketer)
        {
            const string sql = @"
            INSERT INTO CricketerProfile (Name, ODI, Tests, ODIRuns, TestRuns)
            VALUES (@Name, @ODI, @Tests, @ODIRuns, @TestRuns);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = CreateConnection();
            return await connection.QuerySingleAsync<int>(sql, cricketer);
        }

        /// <summary>
        /// UPDATE — Updates an existing cricketer's data.
        /// Returns the number of rows affected (should be 1 if successful).
        /// </summary>
        public async Task<int> UpdateAsync(CricketerProfile cricketer)
        {
            const string sql = @"
            UPDATE CricketerProfile
            SET Name     = @Name,
                ODI      = @ODI,
                Tests    = @Tests,
                ODIRuns  = @ODIRuns,
                TestRuns = @TestRuns
            WHERE ID = @Id";

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, cricketer);
        }

        /// <summary>
        /// DELETE — Removes a cricketer by ID.
        /// Returns the number of rows affected (should be 1 if successful).
        /// </summary>
        public async Task<int> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM CricketerProfile WHERE ID = @Id";

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
