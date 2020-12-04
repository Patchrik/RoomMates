using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMates.Models
{
    // C# representation of the Roommate table
    public class RoommateChore
    {
        public int Id { get; set; }
        public int RoommateId { get; set; }
        public int ChoreId { get; set; }
    }
}
