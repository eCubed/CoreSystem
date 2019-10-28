namespace FCore.Foundations
{
    public interface ISaveViewModel<TEntity>
        where TEntity : class
    {        
        void UpdateEntity(TEntity entity);
    }
}
