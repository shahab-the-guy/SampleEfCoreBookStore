namespace SampleEfCoreBookStore.Domain.Abstractions
{
    public interface ICanBeSoftDeleted
    {
        bool IsDeleted { get; }
    }
}