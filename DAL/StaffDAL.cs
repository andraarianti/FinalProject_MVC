using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BO;
using DAL.Interfaces;
using Dapper;
using Microsoft.IdentityModel.Protocols;

namespace DAL
{
    public class StaffDAL : IStaffDAL
    {
        private string GetConnectionString()
        {
            //return @"Data Source=.\BSISqlExpress;Initial Catalog=TripExpense;Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True";
            //return ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
            return Helper.GetConnectionString();
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                //var strSql = "DELETE FROM BusinessTravel.Staff WHERE StaffID = @StaffID";
                var strSql = "UPDATE BusinessTravel.Staff SET IsDeleted = 1 WHERE StaffID = @StaffID";
                var param = new { StaffID = id };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error DeleteStaff DAL: " + ex.Message);
                }
            }
        }

        public IEnumerable<Staff> GetAll()
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT * FROM BusinessTravel.Staff WHERE IsDeleted = 0";
                try
                {
                    var result = conn.Query<Staff>(strSql);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetAllStaff DAL: " + ex.Message);
                }
            }  
        }

        public Staff GetById(int id)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT * FROM BusinessTravel.Staff WHERE StaffID = @StaffID AND IsDeleted = 0";
                var param = new { StaffID = id };
                try
                {
                    var result = conn.QueryFirstOrDefault<Staff>(strSql, param);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetStaffById DAL: " + ex.Message);
                }
            }
        }

        public IEnumerable<Staff> GetByName(string username)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT * FROM BusinessTravel.Staff WHERE Username LIKE @Username AND IsDeleted = 0";
                var param = new { Username = "%" + username + "%" };
                try
                {
                    var result = conn.Query<Staff>(strSql, param);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetStaffByName DAL: " + ex.Message);
                }
            }
        }

        public void Insert(Staff obj)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "BusinessTravel.AddStaff";
                var param = new { 
                    Name = obj.Name, 
                    PositionID = obj.PositionID, 
                    Role = obj.Role, 
                    Username = obj.Username, 
                    Password = obj.Password, 
                    Email = obj.Email 
                };
                try
                {
                    int result = conn.Execute(strSql, param, commandType: System.Data.CommandType.StoredProcedure);
                    if (result != 1)
                    {
                        throw new ArgumentException("Insert data failed..");
                    };
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error InsertStaff DAL: " + ex.Message);
                }
            }
        }

        public Staff Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM BusinessTravel.Staff WHERE Username = @Username AND Password = BusinessTravel.HashPassword(@Password) AND IsDeleted = 0";
                var param = new { Username = username, Password = password };
                try
                {
                    var result = conn.QueryFirstOrDefault<Staff>(strSql, param);
                    if (result == null)
                    {
                        throw new ArgumentException("Username atau Password salah");
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Login DAL: " + ex.Message);
                }
            }
        }

        public void Update(Staff obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "BusinessTravel.UpdateStaffData";
                var param = new
                {
                    StaffID = obj.StaffID,
                    NewName = obj.Name,
                    NewPositionID = obj.PositionID,
                    NewRole = obj.Role,
                    NewUsername = obj.Username,
                    NewPassword = obj.Password,
                    NewEmail = obj.Email
                };

                try
                {
                    var result = conn.Execute(strSql, param, commandType: CommandType.StoredProcedure);
                    //if (result != 1)
                    //{
                    //    throw new ArgumentException("Update data failed..");
                    //};
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error UpdateStaff DAL: " + ex.Message);
                }
            }
        }


        public bool Login(string password)
        {
            throw new NotImplementedException();
        }

		public int CardTotalEmployee()
		{
			int totalEmployee = 0;

			using (SqlConnection conn = new SqlConnection(GetConnectionString()))
			{
				var strSql = "BusinessTravel.DashboardTotalEmployee";
				try
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand(strSql, conn);
					cmd.CommandType = CommandType.StoredProcedure;
					totalEmployee = (int)cmd.ExecuteScalar(); // Mengambil hasil menggunakan ExecuteScalar
				}
				catch (Exception ex)
				{
					throw new Exception("Error retrieving total employee from database: " + ex.Message);
				}
			}

			return totalEmployee;
		}
	}
}
