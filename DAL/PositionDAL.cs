using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using BO;
using DAL.Interfaces;
using Dapper;

namespace DAL
{
    public class PositionDAL : IPositionDAL
    {
        private string GetConnectionString()
        {
            //return @"Data Source=.\BSISqlExpress;Initial Catalog=TripExpense;Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True";
            return ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                //var strSql = "DELETE FROM BusinessTravel.Position WHERE PositionID = @PositionID";
                var strSql = "UPDATE BusinessTravel.Position SET IsDeleted = 1 WHERE PositionID = @PositionID";
                var param = new { PositionID = id };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error DeletePosition DAL: " + ex.Message);
                }
            }
        }

        public IEnumerable<Position> GetAll()
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT * FROM BusinessTravel.Position WHERE IsDeleted = 0";
                try
                {
                    var result = conn.Query<Position>(strSql);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetAllPosition DAL: " + ex.Message);
                }
            }
        }

        public Position GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT * FROM BusinessTravel.Position WHERE PositionID = @PositionID AND IsDeleted = 0";
                var param = new { PositionID = id };
                try
                {
                    var result = conn.QueryFirstOrDefault<Position>(strSql, param);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetPositionById DAL: " + ex.Message);
                }
            }
        }

        public void Insert(Position obj)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString())){
                var strSql = "INSERT INTO BusinessTravel.Position(PositionName) VALUES(@PositionName)";
                var param = new { 
                    PositionName = obj.PositionName 
                };
                try
                {
                    int result = conn.Execute(strSql, param);
                    if (result != 1)
                    {
                        throw new Exception("Error InsertPosition DAL: " + result);
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }

        public void SetPosition(Position position)
        {
            throw new NotImplementedException();
        }

        public void Update(Position obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "UPDATE BusinessTravel.Position SET PositionName = @PositionName WHERE PositionID = @PositionID";
                var param = new
                {
                    PositionID = obj.PositionID,
                    PositionName = obj.PositionName
                };
                try
                {
                    int result = conn.Execute(strSql, param);
                    if (result != 1)
                    {
                        throw new Exception("Error UpdatePosition DAL: " + result);
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
}
