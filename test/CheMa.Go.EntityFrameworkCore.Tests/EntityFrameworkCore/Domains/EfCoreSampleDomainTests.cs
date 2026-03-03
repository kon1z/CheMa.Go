using CheMa.Go.Samples;
using Xunit;

namespace CheMa.Go.EntityFrameworkCore.Domains;

[Collection(GoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<GoEntityFrameworkCoreTestModule>
{

}
