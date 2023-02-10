using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class SpotRepoDB:IGenericRepo<Spot>
    {
        private DbContext _context;
        private DbSet<Spot> table;

        public SpotRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<Spot>();
        }

        public int Add(Spot obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.SpotID;

        }

        public List<Spot> GetAll()
        {
            return table.ToList();
        }

        public Spot GetByID(int ID)
        {
            return table.Find(ID);
        }
        public byte[] GetImageByID(int id)
        {
            return table.Find(id).SpotImage;
        }


        public Spot Remove(Spot obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        //Detacher vores local entitystate, da denne ellers trackes 2 gange ved edit spot,
        //når vi ikke opdatere med nyt billede, da vi henter det gamle ind.
        public void UpdateItem(Spot obj)
        {
            var local = _context.Set<Spot>()
                    .Local
                    .FirstOrDefault(entry => entry.SpotID.Equals(obj.SpotID));
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            table.Update(obj);
            _context.SaveChanges();
        }
    }
}
