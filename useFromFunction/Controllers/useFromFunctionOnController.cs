using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using useFromFunction.Models;

namespace useFromFunction.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class useFromFunctionOnController : ControllerBase
    {
        private string connectionString = WebApplication.CreateBuilder().Configuration.GetConnectionString("DefaultConnection");
        [HttpPost]
        public  IActionResult forExecuteScalar()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Regist";
                SqlCommand sqlcommand = new SqlCommand(query, connection);
                var tookExecuteScalar =   sqlcommand.ExecuteScalar();

                return Ok(tookExecuteScalar);
            }
        }
        [HttpPost]
        public async ValueTask<IActionResult> toExecuteScalarAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Regist";
                SqlCommand sqlcommand = new SqlCommand(query, connection);
                var tookExecuteScalar = await sqlcommand.ExecuteScalarAsync();

                return Ok(tookExecuteScalar);
            }
        }
        [HttpPost]
        public IActionResult forGenericQueryFirst()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "select * from Regist where FirstName = 'Quvvat'";
                var sqlcommand = connection.QuerySingle<Users>(query);
                
                return Ok(sqlcommand);
            }
        }
        [HttpPost]
        public IActionResult forGenericExecuteReader()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Regist";
                SqlCommand sqlCommand = new SqlCommand(query,connection);
                SqlDataReader sqlRead = sqlCommand.ExecuteReader();
                List<Users> users = new List<Users>();
                Users user = new Users();
                while(sqlRead.Read())
                {
                    user.email = sqlRead.GetString(0);
                    user.firstName = sqlRead.GetString(1);
                    user.lastName = sqlRead.GetString(2);
                    user.userName = sqlRead.GetString(3);
                    user.parol = sqlRead.GetString(4);
                    users.Add(user);    
                }
                return Ok(users);
            }
        }
        [HttpPost]
        public async ValueTask<IActionResult> forGenericRead()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Regist ; select * from IdIsm;";
                var multipleQuery =await connection.QueryMultipleAsync(query);
                var registItem = multipleQuery.ReadFirst<Users>();
                var IdIsmItem = multipleQuery.Read<IdIsm>();
                return Ok(IdIsmItem);
            }
        }
    }
}
