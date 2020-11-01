using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MiCarpeta.Repository.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiCarpeta.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private static readonly RegionEndpoint regionEndpoint = RegionEndpoint.SAEast1;
        public IConfiguration Configuration { get; }

        public Repository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual bool Add(TEntity obj)
        {
            using (DynamoDBContext context = GetContext())
            {
                var task = context.SaveAsync<TEntity>(obj);
                task.Wait();
                return task.IsCompletedSuccessfully;
            }
        }


        public virtual bool Update(TEntity obj)
        {
            using (DynamoDBContext context = GetContext())
            {
                context.SaveAsync<TEntity>(obj).Wait();
                return true;
            }
        }

        public List<TEntity> GetByList(List<FilterQuery> valuesAtributeScanOperator)
        {
            List<TEntity> listEntity;
            using (DynamoDBContext context = GetContext())
            {
                List<ScanCondition> scanFilter = GetScanCondition(valuesAtributeScanOperator);
                AsyncSearch<TEntity> query = context.ScanAsync<TEntity>(scanFilter);
                List<TEntity> partialResultDynamo = new List<TEntity>();
                List<TEntity> totalResultDynamo = new List<TEntity>();
                do
                {
                    partialResultDynamo.Clear();
                    partialResultDynamo = query.GetNextSetAsync().GetAwaiter().GetResult();
                    totalResultDynamo.AddRange(partialResultDynamo);
                } while (!query.IsDone);

                listEntity = totalResultDynamo;

            }
            return listEntity;
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        #region Métodos privados
        private DynamoDBContext GetContext()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(Configuration["MiCarpeta:Dynamo:AccessKey"], Configuration["MiCarpeta:Dynamo:SecretKey"], regionEndpoint);
            AWSConfigsDynamoDB.Context.TableNamePrefix = Configuration["MiCarpeta:Dynamo:TableNamePrefix"];
            DynamoDBContext context = new DynamoDBContext(client);
            return context;
        }

        private List<ScanCondition> GetScanCondition(List<FilterQuery> valuesAtributeScanOperator)
        {
            List<ScanCondition> scanCondition = new List<ScanCondition>();

            foreach (FilterQuery valueAtributeScanOperator in valuesAtributeScanOperator)
            {
                Type attributeClass = valueAtributeScanOperator.ValueAtribute.GetType();
                if ((ScanOperator)valueAtributeScanOperator.Operator == ScanOperator.Between)
                {
                    if (attributeClass.Equals(typeof(sbyte)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (byte)valueAtributeScanOperator.ValueAtribute, (byte)valueAtributeScanOperator.ValueAtributeFinal));
                    else if (attributeClass.Equals(typeof(int)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (int)valueAtributeScanOperator.ValueAtribute, (int)valueAtributeScanOperator.ValueAtributeFinal));
                    else if (attributeClass.Equals(typeof(long)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (long)valueAtributeScanOperator.ValueAtribute, (long)valueAtributeScanOperator.ValueAtributeFinal));
                    else if (attributeClass.Equals(typeof(double)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (double)valueAtributeScanOperator.ValueAtribute, (double)valueAtributeScanOperator.ValueAtributeFinal));
                    else if (attributeClass.Equals(typeof(DateTime)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (DateTime)valueAtributeScanOperator.ValueAtribute, (DateTime)valueAtributeScanOperator.ValueAtributeFinal));
                    else if (attributeClass.Equals(typeof(string)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (string)valueAtributeScanOperator.ValueAtribute, (string)valueAtributeScanOperator.ValueAtributeFinal));
                }
                else if ((ScanOperator)valueAtributeScanOperator.Operator == ScanOperator.In)
                {
                    if (attributeClass.Equals(typeof(List<string>)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, ((List<string>)valueAtributeScanOperator.ValueAtribute).ToArray()));
                }
                else
                {
                    if (attributeClass.Equals(typeof(sbyte)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (byte)valueAtributeScanOperator.ValueAtribute));
                    else if (attributeClass.Equals(typeof(int)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (int)valueAtributeScanOperator.ValueAtribute));
                    else if (attributeClass.Equals(typeof(long)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (long)valueAtributeScanOperator.ValueAtribute));
                    else if (attributeClass.Equals(typeof(double)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (double)valueAtributeScanOperator.ValueAtribute));
                    else if (attributeClass.Equals(typeof(DateTime)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (DateTime)valueAtributeScanOperator.ValueAtribute));
                    else if (attributeClass.Equals(typeof(string)))
                        scanCondition.Add(new ScanCondition(valueAtributeScanOperator.AtributeName, (ScanOperator)valueAtributeScanOperator.Operator, (string)valueAtributeScanOperator.ValueAtribute));
                }
            }
            return scanCondition;
        }
        #endregion
    }
}
