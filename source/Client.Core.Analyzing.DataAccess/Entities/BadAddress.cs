using System.Data.SqlClient;

namespace Client.Core.Analyzing.DataAccess.Entities;

public class BadAddress
{
    public long Id { get; set; }
    public string Host { get; set; }
    public string Reason { get; set; }
    public string Message { get; set; }

    public async static Task<List<BadAddress>> GetAllAsync(User user)
    {
        var applications = new List<BadAddress>();
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_get_bad_addresses @user_id, @token";
        command.Parameters.Add("@user_id", System.Data.SqlDbType.BigInt).Value = user.Id;
        command.Parameters.Add("@token", System.Data.SqlDbType.VarChar, 100).Value = user.Token;
        await connection.OpenAsync();
        var reader = command.ExecuteReader();

        while (await reader.ReadAsync())
        {
            applications.Add(new BadAddress()
            {
                Id = reader.GetInt64(0),
                Host = reader.GetString(1),
                Reason = reader.GetString(2),
                Message = reader.GetString(3)
            });
        }

        await reader.CloseAsync();
        await connection.CloseAsync();

        return applications;
    }
}