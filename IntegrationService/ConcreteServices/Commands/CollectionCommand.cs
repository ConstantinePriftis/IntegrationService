using IntegrationService.ConcreteServices.Helper;
using IntegrationService.IServices.ICommand;
using IntegrationService.IServices.IRepository;
using IntegrationService.Models.Channels;
using IntegrationService.Models.Collection;
using IntegrationService.Models.Imports;
using IntegrationService.Models.User;
using IntegrationService.Services.Repository;
using System.Linq;

namespace IntegrationService.ConcreteServices.Commands
{
    public class CollectionCommand : ICollectionCommand
    {
        private readonly IImportRepository _importRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly ICollectionRepository _collectionRepository;
        public CollectionCommand(IImportRepository importRepository,
            IChannelRepository channelRepository,
            ICollectionRepository collectionRepository)
        {
            _importRepository = importRepository;
            _channelRepository = channelRepository;
            _collectionRepository = collectionRepository;
        }
        private void AddCollections(IEnumerable<Import> collection, List<Channel> channels)
        {
            var psc = collection.Select(x => new
            {
                Sku = x.Sku,
                StoreCode = x.StoreCode,
                Channels = GetChannelsWithValues(x, channels.Select(x => x.ChannelName).ToList())
            });
            var channelProductStore = channels.SelectMany(y => psc.Select(x => new Collection
            {
                Sku = x.Sku,
                StoreCode = x.StoreCode,
                ChannelId = y.Id,
                ModifiedOn = y.ModifiedOn,
                Value = x.Channels[y.ChannelName]
            }));

            _collectionRepository.AddRange(channelProductStore);
            _collectionRepository.Save();
        }
        private void UpdateCollections(IEnumerable<Import> collection, IEnumerable<Channel> channels)
        {
            var imports = _importRepository.GetAll().Select(x => new { Sku = x.Sku, StoreCode = x.StoreCode, Digital = x.Digital, Efood = x.Efood }).ToList();
            var importFromFile = collection.Select(x => new { Sku = x.Sku, StoreCode = x.StoreCode, Digital = x.Digital, Efood = x.Efood }).ToList();

            var differences = importFromFile.Except(imports, new Comparer())
                //.Union(imports2.Except(imports, new Comparer()))
                .ToList();

            var propertyDifferences = new Dictionary<string, string>();
            var updateCollections = new List<Collection>();
            foreach (var difference in differences)
            {
                var import = imports.Where(x => x.Sku == (string)difference.GetType().GetProperty(nameof(Import.Sku))?.GetValue(difference)
                                    && x.StoreCode == (string)difference.GetType().GetProperty(nameof(Import.StoreCode))?.GetValue(difference)).FirstOrDefault();

                var properties = typeof(Import).GetProperties();
                foreach (var property in properties)
                {
                    if (channels.Select(x => x.ChannelName).Contains(property.Name))
                    {
                        var differenceValue = (string)difference.GetType().GetProperty(property.Name)?.GetValue(difference);
                        var importValue = (import != null) ? (string)import.GetType().GetProperty(property.Name)?.GetValue(import) : differenceValue;//property.GetValue(import)?.ToString();
                        var value = differenceValue != importValue ? differenceValue : importValue;

                        updateCollections.Add(new Collection
                        {
                            Sku = (string)difference.GetType().GetProperty(nameof(Import.Sku))?.GetValue(difference),
                            StoreCode = (string)difference.GetType().GetProperty(nameof(Import.StoreCode))?.GetValue(difference),
                            ChannelId = channels.Where(x => x.ChannelName.Equals(property.Name)).First().Id,
                            Value = value,
                            ModifiedOn = DateTime.Now
                        });
                    }
                }
            }

            foreach (var updateCollection in updateCollections)
            {
                var collectionChannel = _collectionRepository.FindBy(x =>
                                    x.Sku == updateCollection.Sku && x.StoreCode == updateCollection.StoreCode && x.ChannelId == updateCollection.ChannelId).FirstOrDefault();
                if (collectionChannel != null)
                {
                    collectionChannel.Value = updateCollection.Value;
                    collectionChannel.ModifiedOn = DateTime.Now;
                    _collectionRepository.Update(collectionChannel);
                }
                else
                {
                    _collectionRepository.Add(updateCollection);
                }
            }

            _collectionRepository.Save();
        }

        private static Dictionary<string, string> GetChannelsWithValues(object obj, List<string> channels)
        {
            var dict = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties.Where(x => channels.Contains(x.Name)))
            {
                var value = property.GetValue(obj)?.ToString();
                dict.Add(property.Name, value);
            }

            return dict;
        }

        public void InsertFromImport(List<Import> collection)
        {
            var collections = _collectionRepository.GetAll();
            var channels = _channelRepository.GetAll().ToList();
            if (collections.Count()>0)
            {

                UpdateCollections(collection, channels);
            }
            else
            {
                AddCollections(collection, channels);
            }
        }

        public Task<int> Insert(Collection item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertRange(IEnumerable<Collection> items, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Guid id, Collection item, ApplicationUser user, List<Guid>? items)
        {
            throw new NotImplementedException();
        }
    }
}
