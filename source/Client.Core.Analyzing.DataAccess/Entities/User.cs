using System.Data.SqlClient;

namespace Client.Core.Analyzing.DataAccess.Entities;

public class User
{
    public long Id { get; set; }
    public string? Token { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserPlan? Plan { get; set; }

    public static User? Current { get; set; }

    public async static Task<User> LoginAsync(string username, string password)
    {
        var user = new User();
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_login @username, @password";
        command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
        command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = password;
        await connection.OpenAsync();
        var reader = command.ExecuteReader();

        await reader.ReadAsync();

        if (reader.FieldCount == 1) throw new Exception(reader.GetString(0));

        user.Id = reader.GetInt64(0);
        user.Token = reader.GetString(2);
        user.Username = username;
        user.FirstName = reader.GetString(4);
        user.LastName = reader.GetString(5);

        switch (reader.GetByte(1))
        {
            case 0: user.Plan = UserPlan.Starter; break;
            case 1: user.Plan = UserPlan.Console; break;
            case 2: user.Plan = UserPlan.Professional; break;
        }

        await reader.CloseAsync();
        await connection.CloseAsync();

        return user;
    }
    public async static Task<User> RegisterAsync(string firstName, string lastName, string username, string password)
    {
        var user = new User();
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_register @firstName, @lastName, @username, @password";
        command.Parameters.AddWithValue("firstName", firstName);
        command.Parameters.AddWithValue("lastName", lastName);
        command.Parameters.AddWithValue("username", username);
        command.Parameters.AddWithValue("password", password);
        await connection.OpenAsync();
        var reader = command.ExecuteReader();

        await reader.ReadAsync();

        if (reader.GetName(0) == "message") throw new Exception(reader.GetString(0));
        user.Id = reader.GetInt64(0);
        user.Token = reader.GetString(2);
        user.Username = reader.GetString(3);
        user.FirstName = reader.GetString(4);
        user.LastName = reader.GetString(5);

        switch (reader.GetInt16(1))
        {
            case 0: user.Plan = UserPlan.Starter; break;
            case 1: user.Plan = UserPlan.Console; break;
            case 2: user.Plan = UserPlan.Professional; break;
        }

        await reader.CloseAsync();
        await connection.CloseAsync();

        return user;
    }
    public async Task<bool> DropTokenAsync()
    {
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_drop_token @user_id, @token";
        command.Parameters.AddWithValue("user_id", Id);
        command.Parameters.AddWithValue("token", Token);

        await connection.OpenAsync();
        var reader = command.ExecuteReader();

        await reader.ReadAsync();
        if (reader.GetName(0) == "message") return false;
        Token = reader.GetString(2);

        await reader.CloseAsync();
        await connection.CloseAsync();

        return true;
    }
    public async Task<bool> ModifyAsync(string? password, string? firstName, string lastName)
    {
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_modify_user @user_id, @token, @firstName, @lastName, @password";
        command.Parameters.AddWithValue("user_id", Id);
        command.Parameters.AddWithValue("token", Token);
        command.Parameters.AddWithValue("firstName", firstName);
        command.Parameters.AddWithValue("lastName", lastName);
        command.Parameters.AddWithValue("password", password);

        await connection.OpenAsync();
        var reader = command.ExecuteReader();

        await reader.ReadAsync();
        if (reader.GetName(0) == "message") return false;
        FirstName = reader.GetString(1);
        LastName = reader.GetString(1);

        await reader.CloseAsync();
        await connection.CloseAsync();

        return true;
    }
    public async Task<bool> ReportAsync(long recieved, long addresses, long applications)
    {
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"EXEC dbo.ddef_modify_user @user_id, @token, @recieved, @addresses, @applications";
        command.Parameters.AddWithValue("user_id", Id);
        command.Parameters.AddWithValue("token", Token);
        command.Parameters.AddWithValue("recieved", recieved);
        command.Parameters.AddWithValue("addresses", addresses);
        command.Parameters.AddWithValue("applications", applications);

        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();

        return reader.GetString(0) == "Success";
    }
}