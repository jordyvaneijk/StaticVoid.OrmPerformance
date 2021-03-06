﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class InsertSingleCommandTextBatched : IRunnableInsertConfiguration
    {
        public string Name { get { return "Insert Single Command Text (batched)"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private List<Models.TestEntity> _entities = new List<Models.TestEntity>();

        public InsertSingleCommandTextBatched(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup() { }

        public void Add(Models.TestEntity entity)
        {
            _entities.Add(entity);
        }

        public void Commit()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString))
            {
                connection.Open();

                var entitySets = ConvertToBatches(_entities, 200);	// limitation of num rows allowed to insert in one call

                foreach (var entitySet in entitySets)
                {
                    string sql = String.Join(" ", entitySet.Select(e => String.Format("INSERT TestEntities(TestDate , TestInt, TestString) VALUES ('{0}',{1},'{2}')", e.TestDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"), e.TestInt, e.TestString)));

                    var cmd = connection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void TearDown()
        {
        }

        // based on/nearly copied from http://www.make-awesome.com/2010/08/batch-or-partition-a-collection-with-linq/
        private IEnumerable<IEnumerable<T>> ConvertToBatches<T>(IEnumerable<T> originalCollection, int batchSize)
        {
            var nextBatch = new List<T>(batchSize);
            foreach (T item in originalCollection)
            {
                nextBatch.Add(item);
                if (nextBatch.Count == batchSize)
                {
                    yield return nextBatch;
                    nextBatch = new List<T>(batchSize);
                }
            }
            if (nextBatch.Count > 0)
                yield return nextBatch;
        }
    }
}
