using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace CheMa.Go.Mapperly.Mappers;

[Mapper]
public partial class OrderMappers : MapperBase<CreateOrderInput, Order>
{
    public override partial Order Map(CreateOrderInput source);

    public override partial void Map(CreateOrderInput source, Order destination);
}