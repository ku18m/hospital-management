namespace Hospital_Management.Repository
{
    public interface IRepository<Entity>
    {
        public List<Entity> GetAll();
        public List<Entity> GetPage(int page);
        public int GetTotalPages(int pageSize);
        public List<Entity> Search(string searchString, string searchProperty);
        public Entity GetById(int id);
        public void Insert(Entity entity);
        public void Update(Entity entity);
        public void Delete(int id);
        public void Save();
       
    }
}
