using Xunit;

namespace Presentation.API;

public class ArticleRpcTests : IClassFixture<IntegrationTestBase>
{
    private readonly IntegrationTestBase _TestBase;

    public ArticleRpcTests(IntegrationTestBase TestBase)
    {
        _TestBase = TestBase;
    }
}