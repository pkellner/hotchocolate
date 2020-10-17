using System;
using System.Threading.Tasks;
using HotChocolate.Data.Sorting;
using HotChocolate.Execution;
using MongoDB.Bson.Serialization.Attributes;
using Squadron;
using Xunit;

namespace HotChocolate.MongoDb.Data.Sorting
{
    public class MongoDbSortVisitorBooleanTests
        : SchemaCache,
          IClassFixture<MongoResource>
    {
        private static readonly Foo[] _fooEntities =
        {
            new Foo { Bar = true }, new Foo { Bar = false }
        };

        private static readonly FooNullable[] _fooNullableEntities =
        {
            new FooNullable { Bar = true },
            new FooNullable { Bar = null },
            new FooNullable { Bar = false }
        };

        public MongoDbSortVisitorBooleanTests(MongoResource resource)
        {
            Init(resource);
        }

        [Fact]
        public async Task Create_Boolean_OrderBy()
        {
            // arrange
            IRequestExecutor tester = CreateSchema<Foo, FooSortType>(_fooEntities);

            // act
            IExecutionResult res1 = await tester.ExecuteAsync(
                QueryRequestBuilder.New()
                    .SetQuery("{ root(order: { bar: ASC}){ bar}}")
                    .Create());

            IExecutionResult res2 = await tester.ExecuteAsync(
                QueryRequestBuilder.New()
                    .SetQuery("{ root(order: { bar: DESC}){ bar}}")
                    .Create());

            // assert
            res1.MatchDocumentSnapshot("ASC");
            res2.MatchDocumentSnapshot("DESC");
        }

        [Fact]
        public async Task Create_Boolean_OrderBy_Nullable()
        {
            // arrange
            IRequestExecutor tester = CreateSchema<FooNullable, FooNullableSortType>(
                _fooNullableEntities);

            // act
            IExecutionResult res1 = await tester.ExecuteAsync(
                QueryRequestBuilder.New()
                    .SetQuery("{ root(order: { bar: ASC}){ bar}}")
                    .Create());

            IExecutionResult res2 = await tester.ExecuteAsync(
                QueryRequestBuilder.New()
                    .SetQuery("{ root(order: { bar: DESC}){ bar}}")
                    .Create());

            // assert
            res1.MatchDocumentSnapshot("ASC");
            res2.MatchDocumentSnapshot("DESC");
        }

        public class Foo
        {
            [BsonId]
            public Guid Id { get; set; } = Guid.NewGuid();

            public bool Bar { get; set; }
        }

        public class FooNullable
        {
            [BsonId]
            public Guid Id { get; set; } = Guid.NewGuid();

            public bool? Bar { get; set; }
        }

        public class FooSortType
            : SortInputType<Foo>
        {
        }

        public class FooNullableSortType
            : SortInputType<FooNullable>
        {
        }
    }
}
