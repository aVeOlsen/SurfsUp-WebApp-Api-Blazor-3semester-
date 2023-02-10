using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class AddressRepoDB : IGenericRepo<Address>
    {
        private DbContext _context;
        private DbSet<Address> table;

        public AddressRepoDB(DbContext _context)
        {
            this._context = _context;
            table = _context.Set<Address>();
        }
        public int Add(Address obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj.ID;
        }

        public List<Address> GetAll()
        {
            return table.ToList();
        }

        //Henter addresse data til user ved brugeroplysninger via user.addressid
        public Address GetByID(int ID)
        {
            //Address address = new Address();
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    conn.Open();
            //    string query = "SELECT StreetName, HouseNumber, PostalCode, City, Region, Country FROM Address WHERE ID = @ID";
            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.Clear();
            //        cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = ID;
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        reader.Read();
            //        address.ID =ID;
            //        address.StreetName = reader[0].ToString();
            //        address.HouseNumber = reader[1].ToString();
            //        address.PostalCode = int.Parse(reader[2].ToString());
            //        address.City = reader[3].ToString();
            //        address.Region = reader[4].ToString();
            //        address.Country = reader[5].ToString();

            //        return address;
            //    }
            //}
            return table.Find(ID);
        }

        public Address Remove(Address obj)
        {
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    conn.Open();

            //    string query="Delete FROM Address WHERE ID = @ID";

            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = obj.ID;

            //        cmd.ExecuteNonQuery();

            //        return obj;
            //    }
            //}
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        public void UpdateItem(Address obj)
        {
            table.Update(obj);
            _context.SaveChanges();
        }
    }
}
