namespace Jam.Domain.SeedWork
{
    public abstract class Entity
    {
        int _id;
        public virtual int Id
        {
            get => _id;
            protected set => _id = value;
        }
        public bool IsTransient()
        {
            return Id == default;
        }
    }
}
