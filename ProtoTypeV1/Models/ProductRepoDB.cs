using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class ProductRepoDB : IGenericRepo<Product>
    {
        private DbContext _context;
        private DbSet<Product> table;

        public ProductRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<Product>();
        }

        public int Add(Product obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.ProductID;
        }

        public List<Product> GetAll()
        {
            return table.ToList();
        }

        public Product GetByID(int ID)
        {
            return table.Find(ID);
        }
        public string GetByUserMail(string id)
        {
            return table.Find(id).ByUser.UserName;
        }



        public byte[] GetImageByID(int id)
        {
            return table.Find(id).ProductImage;
        }


        public Product Remove(Product obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        //Detacher vores local entitystate, da denne ellers trackes 2 gange ved edit product,
        //når vi ikke opdatere med nyt billede, da vi henter det gamle ind.
        public void UpdateItem(Product obj)
        {
            var local = _context.Set<Product>()
                    .Local
                    .FirstOrDefault(entry => entry.ProductID.Equals(obj.ProductID));
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            table.Update(obj);
            _context.SaveChanges();
        }
    }
}
