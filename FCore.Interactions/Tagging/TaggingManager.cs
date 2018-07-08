using FCore.Foundations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Interactions.Tagging
{
    public class TaggingManager<TTagging, TTag> : ManagerBase<TTagging, long>
        where TTagging : class, ITagging<TTag>, new()
        where TTag : class, ITag, new()
    {
        private ITagStore<TTag> TagStore { get; set; }

        public TaggingManager(ITaggingStore<TTagging, TTag> store, ITagStore<TTag> tagStore) : base(store)
        {
            TagStore = tagStore;
        }

        protected ITaggingStore<TTagging, TTag> GetTaggingStore()
        {
            return (ITaggingStore<TTagging, TTag>)Store;
        }

        private TTag ResolveTag(string tagName)
        {
            TTag tag = TagStore.GetQueryableTags().SingleOrDefault(t => t.Name == tagName);

            if (tag == null)
            {
                tag = new TTag();
                tag.Name = tagName;

                TagStore.CreateAsync(tag).Wait();
            }

            return tag;
        }

        public async Task<TTagging> FindAsync(TTagging tagging)
        {
            return await GetTaggingStore().FindAsync(tagging.SystemObjectId, tagging.RecordId, tagging.TagId, tagging.TaggerId);
        }

        public virtual ManagerResult Tag(string systemObjectName, string recordId, string taggerId, List<BasicListingViewModel<long>> tags)
        {
            ISystemObject systemObject = GetTaggingStore().GetQueryableSystemObjects().SingleOrDefault(t => t.Name == systemObjectName);

            if (systemObject == null)
                return new ManagerResult(InteractionMessages.SystemObjectNotFound);

            tags.ForEach(tagAttempt =>
            {
                // First, resolve the tag.
                TTag tag = ResolveTag(tagAttempt.Name);

                // Then resolve the tagging!
                TTagging tagging = GetTaggingStore().FindAsync(systemObject.Id, recordId, tag.Id, taggerId).Result;

                if (tagging == null)
                {
                    var res = DataUtils.CreateAsync(entity: tagging,
                        store: GetTaggingStore(),
                        findUniqueAsync: FindAsync).Result; // do nothing to the result. If it creates the tagging, good. If not, we didn't want it created anyway.
                    
                    if (res.Success)
                    {
                        tagAttempt.Id = tagging.Id;
                    }
                }
            });

            return new ManagerResult(); // Not sure what tags these are.
        }

        public virtual ManagerResult<List<BasicListingViewModel<long>>> GetTagsOfRecord(string systemObjectName, string recordId, string taggerId)
        {
            ISystemObject systemObject = GetTaggingStore().GetQueryableSystemObjects().SingleOrDefault(t => t.Name == systemObjectName);

            if (systemObject == null)
                return new ManagerResult<List<BasicListingViewModel<long>>>(InteractionMessages.SystemObjectNotFound);

            List<TTagging> taggings = GetTaggingStore().GetQueryableTaggings().Where(tg => tg.SystemObjectId == systemObject.Id && tg.RecordId == recordId &&
                tg.TaggerId == taggerId).OrderBy(tg => tg.Tag.Name).ToList();

            List<BasicListingViewModel<long>> tagList = new List<BasicListingViewModel<long>>();

            taggings.ForEach(tagging =>
            {
                tagList.Add(new BasicListingViewModel<long> { Id = tagging.Id, Name = tagging.Tag.Name });
            });

            return new ManagerResult<List<BasicListingViewModel<long>>>(tagList);
        }

        public ManagerResult<ResultSet<TTagging>> SearchTaggings(string systemObjectName, string taggerId, List<string> keywords,
            int page = 1, int pageSize = 10)
        {
            ISystemObject systemObject = GetTaggingStore().GetQueryableSystemObjects().SingleOrDefault(t => t.Name == systemObjectName);

            if (systemObject == null)
                return new ManagerResult<ResultSet<TTagging>> (InteractionMessages.SystemObjectNotFound);

            var qTaggings = GetTaggingStore().GetQueryableTaggings().Where(tg => tg.SystemObjectId == systemObject.Id &&
                tg.TaggerId == taggerId); // preliminary query

            int keyWordsLength = keywords.Count();
            // filter by tags! GroupBy/First by record id because it will pull multiple entries with the same record id for each tag. This is an and search.
            qTaggings = qTaggings.Where(tg => keywords.Contains(tg.Tag.Name)).GroupBy(tg => tg.RecordId).Where(g => g.Count() == keyWordsLength).Select(g => g.First());

            // This one below turns out to be an or search.
            //qTaggings = qTaggings.Where(tg => keywords.Contains(tg.Tag.Name)).GroupBy(tg => tg.RecordId).Select(g => g.First()).OrderBy(t => t.Tag.Name);

            return new ManagerResult<ResultSet<TTagging>>(
                ResultSetHelper.GetResults<TTagging, long>(qTaggings, page, pageSize));            
        }

        /*
        public virtual ManagerResult<ResultSet<TProjection>> SearchByTags<TEntity, TProjection, TKey>(string systemObjectName, string taggerId, string searchText,
            Func<TTagging, TEntity> findById, Func<List<TEntity>, IOrderedQueryable<TEntity>> convertToOrderedQueryable,
            Func<TEntity, TProjection> convert, int page = 1, int pageSize = 10)
            where TEntity : class, IIdentifiable<TKey>
            where TProjection : class, ITaggable
            where TKey : struct
        {
            List<string> keywords = searchText.Split(' ').ToList();

            var getTaggingsResult = SearchTaggings(systemObjectName, taggerId, keywords, page, pageSize);

            if (!getTaggingsResult.Success)
                return new ManagerResult<ResultSet<TProjection>>(getTaggingsResult.Errors);

            List<TEntity> entitiesList = new List<TEntity>();

            getTaggingsResult.Data.ForEach(tagging =>
            {
                entitiesList.Add(findById.Invoke(tagging));
            });

            var qSortedEntities = convertToOrderedQueryable.Invoke(entitiesList);

            ResultSet<TEntity> entitiesRS = ResultSetHelper.GetResults<TEntity, TKey>(qSortedEntities, page, pageSize);

            ResultSet<TProjection> projectionsRS = ResultSetHelper.Convert(entitiesRS, (entity) =>
            {
                TProjection projection = convert.Invoke(entity);
                projection.Tags = GetTagsOfRecord(systemObjectName, entity.Id.ToString(), taggerId).Data;
                return projection;
            });

            return new ManagerResult<ResultSet<TProjection>>(projectionsRS);
        }
        */

        public virtual async Task<ManagerResult> DeleteTag(long taggingId, string taggerId)
        {
            return await DataUtils.DeleteAsync(id: taggingId,
                store: GetTaggingStore(),
                canDelete: (tagging) =>
                {
                    if (tagging.TaggerId != taggerId)
                        return new ManagerResult(ManagerErrors.Unauthorized);
                    return new ManagerResult();
                });
        }


    }
}
