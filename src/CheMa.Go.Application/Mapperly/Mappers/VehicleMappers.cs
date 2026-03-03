using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace CheMa.Go.Mapperly.Mappers;

[Mapper]
public partial class VehicleToVehicleDtoMappers : MapperBase<Vehicle, VehicleDto>
{
    public override partial VehicleDto Map(Vehicle source);

    public override partial void Map(Vehicle source, VehicleDto destination);
}

[Mapper]
public partial class CreateVehicleInputToVehicleDtoMappers : MapperBase<CreateVehicleInput, Vehicle>
{
    public override partial Vehicle Map(CreateVehicleInput source);

    public override partial void Map(CreateVehicleInput source, Vehicle destination);
}

[Mapper]
public partial class UpdateVehicleInputToVehicleDtoMappers : MapperBase<UpdateVehicleInput, VehicleDto>
{
    public override partial VehicleDto Map(UpdateVehicleInput source);

    public override partial void Map(UpdateVehicleInput source, VehicleDto destination);
}

[Mapper]
public partial class UpdateVehicleInputToVehicleMappers : MapperBase<UpdateVehicleInput, Vehicle>
{
    public override partial Vehicle Map(UpdateVehicleInput source);

    public override partial void Map(UpdateVehicleInput source, Vehicle destination);
}

[Mapper]
public partial class VehicleDtoToUpdateVehicleInputMappers : MapperBase<VehicleDto, UpdateVehicleInput>
{
    public override partial UpdateVehicleInput Map(VehicleDto source);

    public override partial void Map(VehicleDto source, UpdateVehicleInput destination);
}