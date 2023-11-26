using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Analyzing.DataAccess.Entities;

public sealed class UserPlan
{
    private static UserPlan? starterInstance;
    private static UserPlan? consoleInstance;
    private static UserPlan? professionalInstance;

    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool DashboardAccess {  get; set; }
    public bool ConsoleAccess { get; set; }

    public static UserPlan Starter
    {
        get
        {
            if (starterInstance == null)
                starterInstance = new UserPlan(0);
            return starterInstance;
        }
    }
    public static UserPlan Console
    {
        get
        {
            if (consoleInstance == null)
                consoleInstance = new UserPlan(1);
            return consoleInstance;
        }
    }
    public static UserPlan Professional
    {
        get
        {
            if (professionalInstance == null)
                professionalInstance = new UserPlan(2);
            return professionalInstance;
        }
    }

    private UserPlan() { }
    private UserPlan(int id)
    {
        var connection = new SqlConnection(Connection.ConnectionString);
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM dbo.ddef_plan WHERE [id] = @plan_id";
        command.Parameters.AddWithValue("plan_id", id);
        connection.Open();

        using (var reader = command.ExecuteReader())
        {
            reader.Read();
            Id = reader.GetByte(0);
            Title = reader.GetString(1);
            Description = reader.GetString(2);
            DashboardAccess = reader.GetBoolean(3);
            ConsoleAccess = reader.GetBoolean(4);
            reader.Close();
        }

        connection.Close();
    }
}