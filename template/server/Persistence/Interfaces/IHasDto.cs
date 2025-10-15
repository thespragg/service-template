namespace Server.Persistence.Interfaces;

internal interface IHasDto<TDto>
{
    TDto ToDto();
}
