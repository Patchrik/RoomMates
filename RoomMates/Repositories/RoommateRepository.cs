using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using RoomMates.Models;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate GetById(int id) 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, RentPortion, Name FROM Roommate rm
                                        JOIN Room r ON rm.RoomId = r.Id
                                        WHERE rm.Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = new Room
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }

                        };
                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
        public List<Roommate> GetAll() 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, RentPortion, RoomId FROM Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPos = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPos);

                        int nameColumnPos = reader.GetOrdinal("FirstName");
                        string nameValue = reader.GetString(nameColumnPos);

                        int rentColumnPos = reader.GetOrdinal("RentPortion");
                        int rentValue = reader.GetInt32(rentColumnPos);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = nameValue,
                            RentPortion = rentValue
                        };

                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }
        }
    }
}
