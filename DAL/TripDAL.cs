using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BO;
using DAL.Interfaces;
using Dapper;

namespace DAL
{
    public class TripDAL : ITripDAL
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
                var strSql = "BusinessTravel.DeleteTripReport";
                var param = new { TripId = id };
                try
                {
                    conn.Execute(strSql, param, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error DeleteTrip DAL: " + ex.Message);
                }
            }
        }

        public IEnumerable<Trip> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT t.*, s.StatusName, st.Name FROM BusinessTravel.Trip AS t JOIN BusinessTravel.Status As s ON t.StatusID = s.StatusID JOIN BusinessTravel.Staff As st ON t.SubmittedBy = st.StaffID WHERE t.IsDeleted = 0";
                try
                {
                    List<Trip> trips = new List<Trip>();
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var trip = new Trip()
                            {
                                TripID = Convert.ToInt32(dr["TripID"]),
                                SubmittedBy = Convert.ToInt32(dr["SubmittedBy"]),
                                StartDate = Convert.ToDateTime(dr["StartDate"]),
                                EndDate = Convert.ToDateTime(dr["EndDate"]),
                                Location = dr["Location"].ToString(),
                                StatusID = Convert.ToInt32(dr["StatusID"]),
                                TotalCost = Convert.ToDecimal(dr["TotalCost"]),
                                Staff = new Staff()
                                {
                                    Name = dr["Name"].ToString()
                                },
                                Status = new Status()
                                {
                                    StatusName = dr["StatusName"].ToString()
                                }
                            };
                            trips.Add(trip);
                        }
                    }
                    return trips;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetAllTrip DAL: " + ex.Message);
                }
            }
        }

        public Trip GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT t.TripID, t.*, s.StatusID, s.StatusName, st.StaffID, st.Name
                                FROM BusinessTravel.Trip AS t
                                JOIN BusinessTravel.Status AS s ON t.StatusID = s.StatusID
                                JOIN BusinessTravel.Staff AS st ON t.SubmittedBy = st.StaffID
                                WHERE t.TripID = @TripID";
                var param = new { TripID = id };
                var result = conn.Query<Trip, Status, Staff, Trip>(strSql, (trip, status, staff) =>
                {
                    trip.Status = status;
                    trip.Staff = staff;
                    return trip;
                }, param, splitOn: "StatusID, StaffID");
                return result.FirstOrDefault();
            }
        }
        IEnumerable<Expense> ITripDAL.GetTripWithExpenseByTripId(int TripID)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT e.*, t.TripID, t.SubmittedBy, t.StartDate, t.EndDate, t.Location, t.StatusID, t.TotalCost, t.LastModified, t.IsDeleted
                        FROM BusinessTravel.Expense AS e
                        JOIN BusinessTravel.Trip AS t ON e.TripID = t.TripID
                        WHERE e.TripID = @TripID AND e.IsDeleted = 0";
                var param = new { TripID = TripID };
                try
                {
                    List<Expense> expenses = new List<Expense>();
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    cmd.Parameters.AddWithValue("@TripID", TripID);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var expense = new Expense()
                            {
                                ExpenseID = Convert.ToInt32(dr["ExpenseID"]),
                                TripID = Convert.ToInt32(dr["TripID"]),
                                Description = dr["Description"].ToString(),
                                ItemCost = Convert.ToDecimal(dr["ItemCost"]),
                                ExpenseType = dr["ExpenseType"].ToString(),
                                ReceiptImage = dr["ReceiptImage"].ToString(),
                                LastModified = Convert.ToDateTime(dr["LastModified"]),
                                IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
                                IsApproved = Convert.ToBoolean(dr["IsApproved"]),
                                Trip = new Trip()
                                {
                                    TripID = Convert.ToInt32(dr["TripID"]),
                                    SubmittedBy = Convert.ToInt32(dr["SubmittedBy"]),
                                    StartDate = Convert.ToDateTime(dr["StartDate"]),
                                    EndDate = Convert.ToDateTime(dr["EndDate"]),
                                    Location = dr["Location"].ToString(),
                                    StatusID = Convert.ToInt32(dr["StatusID"]),
                                    TotalCost = Convert.ToDecimal(dr["TotalCost"]),
                                    LastModified = Convert.ToDateTime(dr["LastModified"])
                                }
                            };
                            expenses.Add(expense);
                        }
                    }
                    return expenses;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetTripWithExpenseByTripId DAL: " + ex.Message);
                }
            }
        }
        public IEnumerable<Trip> GetTripWithAttendees(int TripID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trip> GetTripWithExpense(int TripID)
        {
            throw new NotImplementedException();
        }

        public void Insert(Trip obj)
        {
            throw new NotImplementedException();
        }

        public void Update(Trip obj)
        {
            throw new NotImplementedException();
        }

        public void InsertWithExpenseAndAttendees(Trip trip, List<Expense> expenses)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                try
                {
                    // Insert into Trip Table using SP
                    using (SqlCommand cmdTrip = new SqlCommand("BusinessTravel.AddTripReport", conn))
                    {
                        cmdTrip.CommandType = CommandType.StoredProcedure;

                        var table = new DataTable();
                        table.Columns.Add("ExpenseType", typeof(string));
                        table.Columns.Add("ItemCost", typeof(decimal));
                        table.Columns.Add("Description", typeof(string));
                        table.Columns.Add("ReceiptImage", typeof(string));

                        foreach (var expense in expenses)
                        {
                            table.Rows.Add(expense.ExpenseType, expense.ItemCost, expense.Description, expense.ReceiptImage);
                        }

                        SqlParameter parameter = cmdTrip.Parameters.AddWithValue("@ExpenseItems", table);
                        parameter.SqlDbType = SqlDbType.Structured;

                        // Add other parameters as needed
                        cmdTrip.Parameters.AddWithValue("@SubmittedBy", trip.SubmittedBy);
                        cmdTrip.Parameters.AddWithValue("@Location", trip.Location);
                        cmdTrip.Parameters.AddWithValue("@StartDate", trip.StartDate);
                        cmdTrip.Parameters.AddWithValue("@EndDate", trip.EndDate);
                        cmdTrip.Parameters.AddWithValue("@StatusID", trip.StatusID);

                        cmdTrip.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Handle exceptions if needed
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    // Handle exceptions if needed
                    throw new ArgumentException("Kesalahan: " + ex.Message + "-" + ex.Source);
                }
            }
        }


        public void CreateExpense(Expense expense)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.AddExpense";
                var param = new
                {
                    TripID = expense.TripID,
                    ExpenseType = expense.ExpenseType,
                    ItemCost = expense.ItemCost,
                    Description = expense.Description,
                    ReceiptImage = expense.ReceiptImage,
                };
                try
                {
                    conn.Execute(strSql, param, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Kesalahan: " + ex.Message);
                }
            }
        }

        public void ClaimReimbusment(Trip trip)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.UpdateTripStatus";
                var param = new
                {
                    TripID = trip.TripID,
                    StatusID = trip.StatusID
                };
                try
                {
                    conn.Execute(strSql, param, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Kesalahan: " + ex.Message);
                }
            }
        }

        public void DeleteExpense(Expense expense)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.DeleteExpense";
                var param = new
                {
                    ExpenseID = expense.ExpenseID,
                    TripID = expense.TripID
                };
                try
                {
                    conn.Execute(strSql, param, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Kesalahan: " + ex.Message);
                }
            }
        }

        public Expense GetExpensesById(int ExpenseID)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM BusinessTravel.Expense WHERE ExpenseID = @ExpenseID";
                var param = new { ExpenseID = ExpenseID };
                try
                {
                    return conn.Query<Expense>(strSql, param).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error GetExpensesById DAL: " + ex.Message);
                }
            }
        }

        public void UpdateExpense(Expense expense)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"BusinessTravel.UpdateExpense";
                var param = new
                {
                    ExpenseID = expense.ExpenseID,
                    TripID = expense.TripID,
                    ExpenseType = expense.ExpenseType,
                    ItemCost = expense.ItemCost,
                    Description = expense.Description,
                    ReceiptImage = expense.ReceiptImage
                };
                try
                {
                    conn.Execute(strSql, param, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Kesalahan: " + ex.Message);
                }
            }
        }

        public void SubmitApproval(int tripId, int statusId)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "UPDATE BusinessTravel.Trip SET StatusID = @StatusID WHERE TripID = @TripID";
                var param = new { TripID = tripId, StatusID = statusId };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error SubmitApproval DAL: " + ex.Message);
                }
            }
        }

        public IEnumerable<Trip> GetTripByUserId(int staffID)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT t.TripID, t.*, s.StatusID, s.StatusName, st.StaffID, st.Name
                                FROM BusinessTravel.Trip AS t
                                JOIN BusinessTravel.Status AS s ON t.StatusID = s.StatusID
                                JOIN BusinessTravel.Staff AS st ON t.SubmittedBy = st.StaffID
                                WHERE t.SubmittedBy = @SubmittedBy AND t.IsDeleted = 0";
                var param = new { SubmittedBy = staffID };
                var result = conn.Query<Trip, Status, Staff, Trip>(strSql, (trip, status, staff) =>
                {
                    trip.Status = status;
                    trip.Staff = staff;
                    return trip;
                }, param, splitOn: "StatusID, StaffID");
                return result;
            }
        }
    }
}
