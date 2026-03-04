using CheMa.Go.Applications.Dtos;
using CheMa.Go.Domain.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace CheMa.Go.Mapperly.Mappers;

[Mapper]
public partial class CreateOrderInputToOrderMappers : MapperBase<CreateOrderInput, Order>
{
    public override partial Order Map(CreateOrderInput source);

    public override partial void Map(CreateOrderInput source, Order destination);
}

[Mapper]
public partial class OrderToOrderDtoMappers : MapperBase<Order, OrderDto>
{
    public override partial OrderDto Map(Order source);

    public override partial void Map(Order source, OrderDto destination);
}

[Mapper]
public partial class OrderDtoToUpdateOrderInputMappers : MapperBase<OrderDto, UpdateOrderInput>
{
    public override partial UpdateOrderInput Map(OrderDto source);

    public override partial void Map(OrderDto source, UpdateOrderInput destination);
}

[Mapper]
public partial class UpdateOrderInputToOrderInputMappers : MapperBase<UpdateOrderInput, Order>
{
    public override partial Order Map(UpdateOrderInput source);

    public override partial void Map(UpdateOrderInput source, Order destination);
}