﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_3_1
{
    public class NoDetectChangesConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration
    {
        public string Name { get { return "No Auto Detect Changes"; } }

        public string Technology { get { return "Entity Framework 4.3.1"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public NoDetectChangesConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            Database.SetInitializer<TestContext>(null);// so it doesnt think db is different and try and recreate/migrate it
            _context = new TestContext(_connectionString);
            _context.Configuration.AutoDetectChangesEnabled = false;
        }

        public void Add(Models.TestEntity entity)
        {
            _context.TestEntities.Add(entity);
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            var entity = _context.TestEntities.Single(t => t.Id == id);
            entity.TestDate = testDateTime;
            entity.TestInt = testInt;
            entity.TestString = testString;
        }

        public void Commit()
        {
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
