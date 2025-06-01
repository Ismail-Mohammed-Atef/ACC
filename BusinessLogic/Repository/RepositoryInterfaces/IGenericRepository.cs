﻿namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IGenericRepository<T>
    {
        public IList<T> GetAll();
        public IQueryable<T> GetAllQueryable();
        public T GetById(int id);
        public void Insert(T obj);
        public void Update(T obj);
        public void Delete(T obj);
        public void Save();
    }
}
