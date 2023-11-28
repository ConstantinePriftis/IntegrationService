namespace IntegrationService.IServices
{
    public interface ISqlClientService
    {
        int ExecuteProcedure(string procedureName);
    }
}
