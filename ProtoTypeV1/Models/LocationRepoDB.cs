using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class LocationRepoDB : IGenericRepo<Location>
    {
        private DbSet<Location> table;
        private DbContext _context;

        public LocationRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<Location>();
        }
        public int Add(Location obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.LocationID;
        }

        public List<Location> GetAll()
        {
            return table.ToList();
        }

        public Location GetByID(int ID)
        {
            return table.Find(ID);
        }

        public Location Remove(Location obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        public void UpdateItem(Location obj)
        {
            table.Update(obj);
            _context.SaveChanges();
        }
    }
}
