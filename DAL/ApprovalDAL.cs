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
    public class ApprovalDAL : IApprovaDAL
    {
        private string GetConnectionString()
        {
            //return @"Data Source=.\BSISqlExpress;Initial Catalog=TripExpense;Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True";
            //return ConfigurationManager.ConnectionStrings["MyDbConnectionString"].ConnectionString;
            return Helper.GetConnectionString();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Approval> GetAll()
        {
            throw new NotImplementedException();
        }

        public Approval GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Approval> GetByTripId(int id)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.GetApprovalByTrip";
                var param = new { 
                    TripID = id
                };
                try
                {
                    return conn.Query<Approval>(strSql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetByTripId DAL: " + ex.Message);
                }
            }
        }

        public void Insert(Approval obj)
        {
            throw new NotImplementedException();
        }

        public void Update(Approval obj)
        {
            throw new NotImplementedException();
        }

        public void SetApproval(Approval approval)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.ApproveExpenseItems";
                try
                {
                    var param = new
                    {
                        ExpenseID = approval.ExpenseID,
                        ApproverID = approval.ApproverID,
                    };
                    conn.Execute(strSql, param, commandType:System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error SetApproval DAL: " + ex.Message);
                }
            }
        }

        public void SetRejection(Approval approval)
        {
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.RejectExpenseItems";
               
                try
                {
                    var param = new
                    {
                        ExpenseID = approval.ExpenseID,
                        ApproverID = approval.ApproverID,
                    };
                    conn.Execute(strSql, param, commandType: System.Data.CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error SetRejection DAL: " + ex.Message);
                }
            }   
        }
    }
}
