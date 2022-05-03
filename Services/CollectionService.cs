// using CharacterDatabaseAPI.Models;
// using Microsoft.Extensions.Options;

// namespace CharacterDatabaseAPI.Services;

// public class CollectionService<TDocumentModel> : ICollectionService<TDocumentModel> where TDocumentModel : IDocumentModel
// {
//     private readonly MongoCollectionProvider _provider;
//     private string _collectionName;

//     private IMongoCollection<TDocumentModel> _Collection 
//     { 
//         get => _provider.GetCollection<TDocumentModel>(_collectionName); 
//     }

//     public CollectionService(string collectionName,
//         MongoCollectionProvider provider)
//     {
//         _collectionName = collectionName;
//         _provider = provider;
//     }

//     public async Task<List<TDocumentModel>> GetAsync() =>
//         await _Collection.Find(_ => true).ToListAsync();

//     public async Task<TDocumentModel?> GetAsync(string id) =>
//         await _Collection.Find(x => x.Id == id).FirstOrDefaultAsync();

//     public async Task CreateAsync(TDocumentModel newDocument) =>
//         await _Collection.InsertOneAsync(newDocument);

//     public async Task CreateMultipleAsync(IEnumerable<TDocumentModel> newDocuments) =>
//         await _Collection.InsertManyAsync(newDocuments);

//     public async Task UpdateAsync(string id, TDocumentModel updatedDocument) =>
//         await _Collection.ReplaceOneAsync(x => x.Id == id, updatedDocument);

//     public async Task CreateOrUpdateAsync(TDocumentModel updatedDocument, string? id = null)
//     {
//         id ??= updatedDocument.Id;
//         if(id == null) 
//         {
//             await CreateAsync(updatedDocument);
//             return;
//         }
//         await _Collection.ReplaceOneAsync(x => x.Id == id, updatedDocument, new ReplaceOptions() { IsUpsert = true });
//     }

//     public async Task CreateOrUpdateManyAsync(IEnumerable<TDocumentModel> updatedDocuments)
//     {
//         List<ReplaceOneModel<TDocumentModel>> replaceModels = new List<ReplaceOneModel<TDocumentModel>>();

//         foreach (TDocumentModel document in updatedDocuments)
//         {
//             FilterDefinition<TDocumentModel> filter = Builders<TDocumentModel>.Filter.Eq(r => r.Id, document.Id);

//             ReplaceOneModel<TDocumentModel> replaceModel = new ReplaceOneModel<TDocumentModel>(filter, document)
//             {
//                 IsUpsert = true
//             };

//             replaceModels.Add(replaceModel);
//         }
//         await _Collection.BulkWriteAsync(replaceModels);
//     }
//     public async Task RemoveAsync(string id) =>
//         await _Collection.DeleteOneAsync(x => x.Id == id);
// }