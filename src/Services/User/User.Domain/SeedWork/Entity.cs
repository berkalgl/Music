namespace User.Domain.SeedWork
{
    public abstract class Entity
    {
        int _Id;
        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }
    }
}
