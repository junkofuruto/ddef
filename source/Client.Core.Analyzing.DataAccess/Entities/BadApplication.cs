using System.Data.SqlClient;

namespace Client.Core.Analyzing.DataAccess.Entities;

public class BadApplication
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Reason { get; set; }
    public string Message { get; set; }

    public async static Task<List<BadApplication>> GetAllAsync(User user)
    {
        var applications = new List<BadApplication>();
        var connection = new SqlConnection(Connection.ConnectionString); 
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_get_bad_applications @user_id, @token";
        command.Parameters.Add("@user_id", System.Data.SqlDbType.BigInt).Value = user.Id;
        command.Parameters.Add("@token", System.Data.SqlDbType.VarChar, 100).Value = user.Token;
        await connection.OpenAsync();
        var reader = command.ExecuteReader();

        while (await reader.ReadAsync())
        {
            applications.Add(new BadApplication()
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                Reason = reader.GetString(2),
                Message = reader.GetString(3)
            });
        }

        await reader.CloseAsync();
        await connection.CloseAsync();

        return applications;
    }
}