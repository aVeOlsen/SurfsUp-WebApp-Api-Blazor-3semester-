using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class ReviewRepoDB : IGenericRepo<Review>
    {
        private DbContext _context;
        private DbSet<Review> table;
        public ReviewRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<Review>();
        }

        public int Add(Review obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.ReviewID;
        }

        public List<Review> GetAll()
        {
            return table.ToList();
        }

        public Review GetByID(int ID)
        {
            return table.Find(ID);
        }

        public Review Remove(Review obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
            return obj;

        }

        public void UpdateItem(Review obj)
        {
            table.Update(obj);
            _context.SaveChanges();

        }
    }
}
