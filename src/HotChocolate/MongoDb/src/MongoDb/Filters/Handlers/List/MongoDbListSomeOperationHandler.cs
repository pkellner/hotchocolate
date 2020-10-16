using System;
using HotChocolate.Data.Filters;
using HotChocolate.Language;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HotChocolate.MongoDb.Data.Filters
{
    public class MongoDbListSomeOperationHandler : MongoDbListOperationHandlerBase
    {
        protected override int Operation => DefaultOperations.Some;

        protected override FilterDefinition<BsonDocument> HandleListOperation(
            MongoDbFilterVisitorContext context,
            IFilterField field,
            ObjectFieldNode node,
            Type closureType,
            MongoDbFilterScope scope,
            string path,
            BsonDocument? bsonDocument)
        {
            return context.Builder.ElemMatch(
                path,
                GetFilters(context, scope));
        }
    }
}