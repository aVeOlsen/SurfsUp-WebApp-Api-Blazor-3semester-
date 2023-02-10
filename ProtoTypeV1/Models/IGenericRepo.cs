using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public interface IGenericRepo<T>
    {
        public int Add(T obj);

        public List<T> GetAll();

        public T GetByID(int ID);

        public void UpdateItem(T obj);

        public T Remove(T obj);
    }
}
