using Xunit;

namespace CheMa.Go.EntityFrameworkCore;

[CollectionDefinition(GoTestConsts.CollectionDefinitionName)]
public class GoEntityFrameworkCoreCollection : ICollectionFixture<GoEntityFrameworkCoreFixture>
{

}
