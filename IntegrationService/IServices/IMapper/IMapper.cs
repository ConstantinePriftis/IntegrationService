namespace IntegrationService.IServices.IMapper
{
    public interface IMapper<Tsource, Tdestination>
        where Tsource : class, new() where Tdestination : class, new()
    {
    }
}
