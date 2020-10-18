using System;
using System.Collections.Generic;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Tests.DataGenerator.Enums;
using Tests.DataGenerator.Models;

namespace Tests.DataGenerator
{
    public class Tests
    {
        private readonly DataContext _context;
        private readonly Faker _faker;

        public Tests()
        {
            _context = new DataContext();
            _faker = new Faker("pl");
        }
        [Test]
        public void RemoveData()
        {
            _context.Emergencies_Teams.RemoveRange(_context.Emergencies_Teams);
            _context.Teams.RemoveRange(_context.Teams);
            _context.Users.RemoveRange(_context.Users);
            _context.Emergencies.RemoveRange(_context.Emergencies);
            _context.EmergencyTypes.RemoveRange(_context.EmergencyTypes);
            _context.SaveChanges();
        }

        [Test]
        public void GenerateData()
        {
            GenerateUsers();
            var teams = GenerateTeams();
            InsertEmergencyTypes();
            GenerateEmergencies(teams);
        }

        private void GenerateUsers()
        {
            for (int i = 0; i < 10; i++)
                _context.Users.Add(new User() { Username = _faker.Name.FirstName(), Password = _faker.Random.Word() });
            _context.SaveChanges();
        }
        private IEnumerable<Team> GenerateTeams()
        {
            var list = new List<Team>();
            for (int i = 0; i < 10; i++)
                list.Add(new Team() { Location = "Baza", Status = TeamStatus.Available });
            _context.Teams.AddRange(list);
            _context.SaveChanges();
            return list;
        }
        private void GenerateEmergencies(IEnumerable<Team> teams)
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (var team in teams)
                {
                    var emergency = new Emergency() { Location = _faker.Address.Locale, Description = _faker.Lorem.Sentence(20, 5), EmergencyTypeId = _faker.Random.Int(1, 9), ModUser = "Generator", ReportTime = _faker.Date.Past(), Status = EmergencyStatus.Done };
                    _context.Emergencies.Add(emergency);
                    _context.Emergencies_Teams.Add(new Emergency_Team() { Team = team, Emergency = emergency });
                }
            }
            _context.SaveChanges();
        }
        private void InsertEmergencyTypes()
        {
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 1, Name = "Wypadek samochodowy" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 2, Name = "Porażenie prądem" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 3, Name = "Utonięcie" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 4, Name = "Pobicie" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 5, Name = "Zachłyśnięcie" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 6, Name = "Oparzenie" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 7, Name = "Zawał" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 8, Name = "Omdlenie" });
            _context.EmergencyTypes.Add(new EmergencyType() { Id = 9, Name = "Wylew" });
            _context.SaveChanges();
        }
    }
}
