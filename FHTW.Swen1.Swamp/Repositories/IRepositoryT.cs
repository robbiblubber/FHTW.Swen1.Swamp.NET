using System;



namespace FHTW.Swen1.Swamp.Repositories
{
    public interface IRepository<T>
    {
        public T Get(object id);

        public IEnumerable<T> GetAll();

        public void Save(T obj);

        public void Delete(T obj);
    }
}
