namespace FCore.Foundations
{
    public interface IGetViewModel<TEntity>
        where TEntity: class
    {
        void FillViewModel(TEntity entity);
    }
}
