using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace CheMa.Go.Mapperly.Mappers;

[Mapper]
public partial class HotelMappers : MapperBase<Hotel, HotelDto>
{
    public override partial HotelDto Map(Hotel source);

    public override partial void Map(Hotel source, HotelDto destination);
}

[Mapper]
public partial class CreateHotelInputToHotelMappers : MapperBase<CreateHotelInput, Hotel>
{
    public override partial Hotel Map(CreateHotelInput source);

    public override partial void Map(CreateHotelInput source, Hotel destination);
}

[Mapper]
public partial class HotelDtoToUpdateHotelInputMappers : MapperBase<HotelDto, UpdateHotelInput>
{
    public override partial UpdateHotelInput Map(HotelDto source);

    public override partial void Map(HotelDto source, UpdateHotelInput destination);
}

[Mapper]
public partial class UpdateHotelInputToHotelMappers : MapperBase<UpdateHotelInput, Hotel>
{
    public override partial Hotel Map(UpdateHotelInput source);

    public override partial void Map(UpdateHotelInput source, Hotel destination);
}