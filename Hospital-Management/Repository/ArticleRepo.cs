using Hospital_Management.Data;
using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Repository
{
    public class ArticleRepo(ApplicationDbContext context) : IArticleRepo
    {
        void IRepository<Article>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        List<Article> IRepository<Article>.GetAll()
        {
            return context.Articles.Include(a => a.Doctor).ToList();
        }

        Article IRepository<Article>.GetById(int id)
        {
            return context.Articles.Include(a => a.Doctor)
                .FirstOrDefault(a => a.Id == id);
        }

        List<Article> IRepository<Article>.GetPage(int page)
        {
            int pageSize = 5;

            return context.Articles.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        int IRepository<Article>.GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Articles.Count() / pageSize);
        }

        void IRepository<Article>.Insert(Article entity)
        {
            context.Articles.Add(entity);
        }

        void IRepository<Article>.Save()
        {
            context.SaveChanges();
        }

        List<Article> IRepository<Article>.Search(string searchString, string searchProperty)
        {
            switch (searchProperty)
            {
                case "Title":
                    return context.Articles.Where(a => a.Title.Contains(searchString)).ToList();
                case "Doctor":
                    return context.Articles.Where(a => a.Doctor.FullName.Contains(searchString)).ToList();

                default:
                    return context.Articles.Where(a => a.Content.Contains(searchString)).ToList();
            }
        }

        void IRepository<Article>.Update(Article entity)
        {
            context.Articles.Update(entity);
        }
    }
}
