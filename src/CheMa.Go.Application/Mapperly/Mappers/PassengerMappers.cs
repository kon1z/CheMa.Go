using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace CheMa.Go.Mapperly.Mappers;

[Mapper]
public partial class PassengerMappers : MapperBase<Passenger, PassengerDto>
{
    public override partial PassengerDto Map(Passenger source);

    public override partial void Map(Passenger source, PassengerDto destination);
}