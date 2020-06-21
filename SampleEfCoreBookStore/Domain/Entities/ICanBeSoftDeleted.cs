namespace SampleEfCoreBookStore.Domain.Entities
{
    public interface ICanBeSoftDeleted
    {
        bool IsDeleted { get; }
    }
}