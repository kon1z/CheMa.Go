using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace CheMa.Go.Mapperly.Mappers;

[Mapper]
public partial class PassengerToPassengerDtoMappers : MapperBase<Passenger, PassengerDto>
{
    public override partial PassengerDto Map(Passenger source);

    public override partial void Map(Passenger source, PassengerDto destination);
}

[Mapper]
public partial class CreatePassengerInputToPassengerMappers : MapperBase<CreatePassengerInput, Passenger>
{
    public override partial Passenger Map(CreatePassengerInput source);

    public override partial void Map(CreatePassengerInput source, Passenger destination);
}

[Mapper]
public partial class PassengerDtoToUpdatePassengerInputMappers : MapperBase<PassengerDto, UpdatePassengerInput>
{
    public override partial UpdatePassengerInput Map(PassengerDto source);

    public override partial void Map(PassengerDto source, UpdatePassengerInput destination);
}

[Mapper]
public partial class UpdatePassengerInputToPassengerMappers : MapperBase<UpdatePassengerInput, Passenger>
{
    public override partial Passenger Map(UpdatePassengerInput source);

    public override partial void Map(UpdatePassengerInput source, Passenger destination);
}