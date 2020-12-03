using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RoomMates.Models;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        //Let's get all the chores this will be typed out following the design of the RoomRepo
        public List<Chore> GetAll() 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPos = reader.GetOrdinal("Id");
                        int choreIdValue = reader.GetInt32(idColumnPos);
                        // Here we'll get the names for the chores
                        int nameColumnPos = reader.GetOrdinal("Name");
                        string choreNameValue = reader.GetString(nameColumnPos);
                        // Now we need to create a c# object (Chore) from the sql data
                        Chore chore = new Chore
                        {
                            Id = choreIdValue,
                            Name = choreNameValue
                        };
                        chores.Add(chore);
                    }
                    // Close the reader - VERY IMPORTANT
                    reader.Close();
                    // Now we want to return our list of chores.
                    return chores;
                }
            }
        }
    
    }
}
