﻿using Roommates.Repositories;
using RoomMates.Models;
using System;
using System.Collections.Generic;

namespace RoomMates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        Console.Clear();
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Clear();
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Clear();
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);
                        Console.Clear();
                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        Console.Clear();
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case ("Search for chore"):
                        Console.Clear();
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Clear();
                        Console.Write("Chore Name: ");
                        string ChoreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = ChoreName
                        };

                        choreRepo.Insert(choreToAdd);
                        Console.Clear();
                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Get Unassigned Chores"):
                        Console.Clear();
                        PrintUnassignedChores(choreRepo);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search Roommate"):
                        Console.Clear();
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());
                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.WriteLine($"{roommate.Firstname} - Rent Portion: {roommate.RentPortion} - Room: {roommate.Room.Name}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign Chore"):
                        Console.Clear();
                        PrintUnassignedChores(choreRepo);
                        Console.Write("Select an option: ");
                        int chosenChoreId = int.Parse(Console.ReadLine());
                        Console.Clear();
                        PrintRoommates(roommateRepo);
                        Console.Write("Select an option: ");
                        int chosenRoommateId = int.Parse(Console.ReadLine());
                        Chore chosenChore = choreRepo.GetById(chosenChoreId);
                        Roommate chosenRoomate = roommateRepo.GetById(chosenRoommateId);
                        RoommateChore assignedChore = choreRepo.AssignChore(chosenRoommateId, chosenChoreId);
                        Console.Clear();
                        Console.WriteLine($"{chosenRoomate.Firstname} has been assigned: {chosenChore.Name} - id: {assignedChore.Id}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();

                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static void PrintUnassignedChores(ChoreRepository choreRepo) {
            List<Chore> unassedChores = choreRepo.GetUnassignedChores();
            foreach (Chore c in unassedChores)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }
        }

        static void PrintRoommates(RoommateRepository roommateRepo)
        {
            List<Roommate> roommates = roommateRepo.GetAll();
            foreach (Roommate rm in roommates)
            {
                Console.WriteLine($"{rm.Id} - {rm.Firstname} - {rm.RentPortion}");
            }
        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Get Unassigned Chores",
            "Search Roommate",
            "Assign Chore",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
                Console.WriteLine("------------------");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];

                }
                catch (Exception)
                {

                    continue;
                }
            }

        }
    }
}
