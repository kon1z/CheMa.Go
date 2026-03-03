using CheMa.Go.Samples;
using Xunit;

namespace CheMa.Go.EntityFrameworkCore.Applications;

[Collection(GoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<GoEntityFrameworkCoreTestModule>
{

}
