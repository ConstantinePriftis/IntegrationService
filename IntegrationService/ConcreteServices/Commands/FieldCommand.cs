using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Channels;
using IntegrationService.Models.Fields;
using IntegrationService.Models.User;
using System.Security.Claims;

namespace IntegrationService.ConcreteServices.Commands
{
    public class FieldCommand : IFieldCommand
    {
        private readonly IFieldRepository _fieldRepository;
        public FieldCommand(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }
        public Task<int> Insert(Field item, ApplicationUser user, List<Guid> items)
        {
            var field = Field.Create(item.Name, item.Type,item.Order, items , user);
            _fieldRepository.Add(field);
            return Task.FromResult(_fieldRepository.Save());
        }

        public Task<int> InsertRange(IEnumerable<Field> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, Field item, ApplicationUser user, List<Guid> items)
        {
            Field? field = _fieldRepository.FindBy(x => x.Id == id).FirstOrDefault();
            if (field != null)
            {
                field.Update(item.Name, item.Type, items, user);
            }
            else
            {
                return Task.FromResult(0);
            }

            return Task.FromResult(_fieldRepository.Save());
        }
    }
}
