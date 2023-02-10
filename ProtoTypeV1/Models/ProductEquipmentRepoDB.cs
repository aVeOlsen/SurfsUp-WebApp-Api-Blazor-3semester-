using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class ProductEquipmentRepoDB : IGenericRepo<ProductEquipment>
    {
        private DbSet<ProductEquipment> table;
        private DbContext _context;
        public ProductEquipmentRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<ProductEquipment>();
        }
        public int Add(ProductEquipment obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.EquipmentID;

        }

        public List<ProductEquipment> GetAll()
        {
            return table.ToList();
        }

        public ProductEquipment GetByID(int ID)
        {
            return table.Find(ID);
        }

        public ProductEquipment Remove(ProductEquipment obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        public void UpdateItem(ProductEquipment obj)
        {
            table.Update(obj);
            _context.SaveChanges();
        }
    }
}
