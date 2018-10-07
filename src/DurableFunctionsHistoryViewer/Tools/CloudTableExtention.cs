using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DurableFunctionsHistoryViewer.Tools
{
    public static class CloudTableExtention
    {
        public static async Task<IEnumerable<T>> Query<T>(this CloudTable table, Func<T, bool> predicate,
            int maxResults = int.MaxValue)
            where T : ITableEntity, new()
        {
            if (null == predicate) throw new ArgumentNullException("predicate");

            if (0 >= maxResults) throw new InvalidOperationException("maxResults: must be above 0.");

            var items = await Query(table, new TableQuery<T>()).ConfigureAwait(false);

            return items.Where(predicate).Take(maxResults);
        }

        public static async Task<IEnumerable<T>> Query<T>(this CloudTable table, TableQuery<T> query)
            where T : ITableEntity, new()
        {
            if (null == query) throw new ArgumentNullException("query");

            var entities = new List<T>();
            TableContinuationToken token = null;

            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (null != token);

            return entities;
        }

        public static async Task<T> Get<T>(this CloudTable table, string partitionKey, string rowKey)
            where T : ITableEntity, new()
        {
            var partitionFilter =
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            var rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
            var filter = TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowFilter);
            var query = new TableQuery<T>().Where(filter);

            var result = await Query(table, query).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public static async Task<IEnumerable<T>> GetByPartition<T>(this CloudTable table, string partitionKey)
            where T : ITableEntity, new()
        {
            var query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return await Query(table, query).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<T>> GetByRow<T>(this CloudTable table, string rowKey)
            where T : ITableEntity, new()
        {
            var query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
            return await Query(table, query).ConfigureAwait(false);
        }

        public static async Task DeleteByPartition(this CloudTable table, string partitionKey)
        {
            var entities = await GetByPartition<TableEntity>(table, partitionKey).ConfigureAwait(false);
            if (null != entities && entities.Any()) await Delete(table, entities).ConfigureAwait(false);
        }

        public static async Task DeleteByRow(this CloudTable table, string rowKey)
        {
            var entities = await GetByRow<TableEntity>(table, rowKey).ConfigureAwait(false);
            if (null != entities && entities.Any())
                foreach (var entity in entities)
                    await Delete(table, entity).ConfigureAwait(false);
        }

        public static async Task Delete(this CloudTable table, string partitionKey, string rowKey)
        {
            var entity = await Get<TableEntity>(table, partitionKey, rowKey).ConfigureAwait(false);

            if (null != entity) await Delete(table, entity).ConfigureAwait(false);
        }

        public static async Task<TableResult> Delete(this CloudTable table, ITableEntity entity)
        {
            if (null == entity) throw new ArgumentNullException("entity");

            return await table.ExecuteAsync(TableOperation.Delete(entity)).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<TableResult>> Delete(this CloudTable table,
            IEnumerable<ITableEntity> entities)
        {
            if (null == entities) throw new ArgumentNullException("entities");
            if (!entities.Any()) return Array.Empty<TableResult>();

            var result = new List<TableResult>();

            foreach (var batch in Batch(entities))
            {
                var batchOperation = new TableBatchOperation();
                batch.ToList().ForEach(e => batchOperation.Delete(e));
                var r = await table.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
                result.AddRange(r);
            }

            return result;
        }

        public static Task<TableResult> Insert(this CloudTable table, ITableEntity entity)
        {
            return table.ExecuteAsync(TableOperation.Insert(entity));
        }

        public static async Task<IEnumerable<TableResult>> Insert(this CloudTable table,
            IEnumerable<ITableEntity> entities)
        {
            var result = new List<TableResult>();

            foreach (var batch in Batch(entities))
            {
                var batchOperation = new TableBatchOperation();
                batch.ToList().ForEach(e => batchOperation.Insert(e));
                var r = await table.ExecuteBatchAsync(batchOperation);
                result.AddRange(r);
            }

            return result;
        }

        public static Task<TableResult> InsertOrMearge(this CloudTable table, ITableEntity entity)
        {
            return table.ExecuteAsync(TableOperation.InsertOrMerge(entity));
        }

        public static async Task<IEnumerable<TableResult>> InsertOrMerge(this CloudTable table,
            IEnumerable<ITableEntity> entities)
        {
            var result = new List<TableResult>();

            foreach (var batch in Batch(entities))
            {
                var batchOperation = new TableBatchOperation();
                batch.ToList().ForEach(e => batchOperation.InsertOrMerge(e));
                var r = await table.ExecuteBatchAsync(batchOperation);
                result.AddRange(r);
            }

            return result;
        }

        public static Task<TableResult> InsertOrReplace(this CloudTable table, ITableEntity entity)
        {
            return table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public static async Task<IEnumerable<TableResult>> InsertOrReplace(this CloudTable table,
            IEnumerable<ITableEntity> entities)
        {
            var result = new List<TableResult>();

            foreach (var batch in Batch(entities))
            {
                var batchOperation = new TableBatchOperation();
                batch.ToList().ForEach(e => batchOperation.InsertOrReplace(e));
                var r = await table.ExecuteBatchAsync(batchOperation);
                result.AddRange(r);
            }

            return result;
        }

        private static IEnumerable<IEnumerable<ITableEntity>> Batch(IEnumerable<ITableEntity> entities)
        {
            return entities.GroupBy(en => en.PartitionKey).SelectMany(Chunk);
        }

        private static IEnumerable<IEnumerable<IDictionary<string, object>>> Batch(
            IEnumerable<IDictionary<string, object>> entities)
        {
            return entities.GroupBy(en => en["PartitionKey"]).SelectMany(Chunk);
        }

        private static IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> entities)
        {
            return entities.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / 100)
                .Select(x => x.Select(v => v.Value));
        }
    }
}
