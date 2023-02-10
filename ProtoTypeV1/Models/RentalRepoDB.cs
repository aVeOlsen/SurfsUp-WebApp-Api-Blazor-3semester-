using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class RentalRepoDB : IGenericRepo<Rental>
    {

        IGenericRepo<Product> _pRepo;
        private DbContext _context;
        private DbSet<Rental> table;

        public RentalRepoDB(DbContext _context)
        {
            this._context = _context;
            _pRepo = new ProductRepoDB(_context);
            table = _context.Set<Rental>();

        }

        //Vi tilføjer her også til fælles tabellen, ProductRental, for at kunne hente productID sammen med RentalID
        public int Add(Rental obj)
        {
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    string query = "INSERT INTO Rentals(RentalID, Depositum, ByDate, ToDate, Description, RentedOutBy, ProductID)" +
            //        "VALUES(@RentalID, @Depositum, @ByDate, @ToDate, @Description, @RentedOutBy, @ProductID)";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.Add("@RentalID", SqlDbType.Int).Value = obj.RentalID;
            //    cmd.Parameters.Add("@Depositum", SqlDbType.Float).Value = obj.Depositum;
            //    cmd.Parameters.Add("@ByDate", SqlDbType.DateTime).Value = obj.ByDate;
            //    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = obj.ToDate;
            //    cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = obj.Description;
            //    cmd.Parameters.Add("@RentedOutBy", SqlDbType.NVarChar).Value = obj.RentedOutBy.Id;
            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //    SqlCommand getID = new SqlCommand("@@IDENTITY as 'identity'", conn);
            //    return Convert.ToInt32(getID.ExecuteScalar());
            table.Add(obj);
            _context.SaveChanges();
            using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            {
                string query = "INSERT INTO ProductRental(RentalsRentalID, ProductID) " +
                    "Values(@RentalsRentalID, @ProductID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@RentalsRentalID", SqlDbType.Int).Value = obj.RentalID;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = obj.ProductID;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return obj.RentalID;
            //}
        }

        public List<Rental> GetAll()
        {
            //List<Rental> rentals = new List<Rental>();
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    string query = "SELECT RentalID, Depositum, ByDate, ToDate, Description, RentedoutBy FROM Rentals";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        Rental r = new Rental();
            //        r.RentalID = reader.GetInt32(0);
            //        r.Depositum = reader.GetDouble(1);
            //        r.ByDate = reader.GetDateTime(2);
            //        r.ToDate = reader.GetDateTime(3);
            //        r.Description = reader.GetString(4);
            //        r.RentedOutBy.Id = reader.GetString(5);
            //        rentals.Add(r);
            //    }
            //    return rentals;
            //}
            return table.ToList();
        }

        public Rental GetByID(int ID)
        {
            //Rental r = new Rental();
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    conn.Open();
            //    string query = "SELECT RentalID, Depositum, ByDate, ToDate, Description, RentedoutBy, ProductID FROM Rentals WHERE RentalID = @RentalID";
            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.Clear();
            //        cmd.Parameters.Add("@RentalID", System.Data.SqlDbType.Int).Value = ID;
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        reader.Read();
            //        r.RentalID = ID;
            //        r.Depositum = reader.GetDouble(0);
            //        r.ByDate = reader.GetDateTime(1);
            //        r.ToDate = reader.GetDateTime(2);
            //        r.Description = reader.GetString(3);
            //        r.RentedOutBy.Id = reader.GetString(4);
            //        r.Product.ProductID = reader.GetInt32(5);
            //        return r;
            //    }
            //}
            return table.Find(ID);
        }

        //Her henter vi fra vores fælles tabel, da vi henter fra denne når vi vil redigere vores rental
        public int GetProductByID(int id)
        {
            Rental r = new Rental();
            using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            {
                conn.Open();
                string query = "SELECT RentalsRentalID, ProductID FROM ProductRental WHERE RentalsRentalID=@RentalsRentalID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@RentalsRentalID", SqlDbType.Int).Value = id;
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.ReadAsync();
                    r.RentalID = id;
                    r.ProductID = reader.GetInt32(1);
                }
                return r.ProductID;
            }
        }


        public Rental Remove(Rental obj)
        {
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    conn.Open();
            //    string query = "DELETE FROM Rentals WHERE RentalID = @RentalID";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.Add("@RentalID", SqlDbType.Int).Value = obj.RentalID;
            //    cmd.ExecuteNonQuery();
            //}
            //return obj;
            table.Remove(obj);
            _context.SaveChanges();
            return obj;
        }

        public void UpdateItem(Rental obj)
        {
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    conn.Open();
            //    string query = "UPDATE Rentals SET Depositum=@Depositum, ByDate=@Bydate, Todate=@Todate, Desciption=@Description, RentedOutBy=@RentedOutBy, ProductID WHERE RentalID = RentalID";

            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.Clear();
            //    cmd.Parameters.Add("@Depositum", SqlDbType.Float).Value = obj.Depositum;
            //    cmd.Parameters.Add("@ByDate", SqlDbType.DateTime).Value = obj.ByDate;
            //    cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = obj.ToDate;
            //    cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = obj.Description;
            //    cmd.Parameters.Add("@RentedOutBy", SqlDbType.NVarChar).Value = obj.RentedOutBy.Id;
            //    cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = obj.Product.ProductID;
            //    cmd.ExecuteNonQuery();
            //}

            table.Update(obj);
            _context.SaveChanges();
            //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
            //{
            //    string query = "UPDATE ProductRental SET RentalsRentalID = @RentalsRentalID, ProductID = @ProductID";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.Add("@RentalsRentalID", SqlDbType.Int).Value = obj.RentalID;
            //    cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = obj.ProductID;
            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //}


        }
        //public Rental GetByUserID(Rental obj)
        //{

        //    //using (SqlConnection conn = new SqlConnection(DbConnection.Conectionstring))
        //    //{
        //    //    List<Rental> rentals = new List<Rental>();
        //    //    conn.Open();
        //    //    string query = "SELECT Rentals.RentalID, Rentals.Depositum, Rentals.ByDate, Rentals.ToDate, Rentals.Description, Rentals.RentedOutBy, " +
        //    //        "ASPNetUsers.FirstName, ASPNetUsers.LastName, ASPNetUsers.Gender, ASPNetUsers.DayOfBirth FROM Rentals " +
        //    //        "INNER JOIN ASPNetUsers on ASPNetUsers.Id = Rentals.RentedOutBy WHERE Address.RentedOutBy = @RentedOutBy";

        //    //    SqlCommand cmd = new SqlCommand(query, conn);

        //    //    cmd.Parameters.Clear();
        //    //    cmd.Parameters.Add("@RentedOutBy", SqlDbType.NVarChar).Value = ID;
        //    //    var reader = cmd.ExecuteReader();
        //    //    while (reader.Read())
        //    //    {
        //    //        Rental r = new Rental();
        //    //        r.RentalID = reader.GetInt32(0);
        //    //        r.Depositum = reader.GetDouble(1);
        //    //        r.ByDate = reader.GetDateTime(2);
        //    //        r.ToDate = reader.GetDateTime(3);
        //    //        r.Description = reader.GetString(4);
        //    //        r.RentedOutBy.Id = ID;

        //    //        rentals.Add(r);
        //    //    }
        //    //    return rentals;
        //    return table.Find(obj.RentedOutBy);

        //}


    }
}
