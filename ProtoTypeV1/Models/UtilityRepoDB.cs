using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class UtilityRepoDB : IGenericRepo<Utility>
    {
        private DbSet<Utility> table;
        private DbContext _context;

        public UtilityRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<Utility>();
        }
        public int Add(Utility obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.UtilityID;
        }

        public List<Utility> GetAll()
        {
            return table.ToList();
        }

        public Utility GetByID(int ID)
        {
            return table.Find(ID);
        }

        public Utility Remove(Utility obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        public void UpdateItem(Utility obj)
        {
            table.Update(obj);
            _context.SaveChanges();
        }
    }
}
