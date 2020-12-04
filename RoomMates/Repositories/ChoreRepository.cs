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
        public Chore GetById(int id) 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Here is a null chore because it's how Adam did it.
                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();
                    return chore;
                }
            }
        }
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassignedChores() 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Name FROM RoommateChore rc
                                        RIGHT JOIN Chore c
                                        ON rc.ChoreId = c.Id
                                        WHERE rc.ChoreId IS NULL;";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPos = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPos);


                        int nameColumnPos = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPos);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        chores.Add(chore);
                    }
                    reader.Close();
                    return chores;
                }
            }
        }
    }
}
