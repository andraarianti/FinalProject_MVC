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
            using(SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "UPDATE BusinessTravel.Trip SET IsDeleted = 1 WHERE TripID = @TripID";
                var param = new { TripID = id };
                try
                {
                    conn.Execute(strSql, param);
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

        public IEnumerable<Trip> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = "SELECT t.*, s.StatusName, st.Name FROM BusinessTravel.Trip AS t JOIN BusinessTravel.Status As s ON t.StatusID = s.StatusID JOIN BusinessTravel.Staff As st ON t.SubmittedBy = st.StaffID";
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
                var transaction = conn.BeginTransaction();

                try
                {
                   
                    // Insert into Trip Table using SP
                    using (SqlCommand cmdTrip = new SqlCommand("BusinessTravel.AddTripReport", conn, transaction))
                    {
                        cmdTrip.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the SP
                        cmdTrip.Parameters.AddWithValue("@SubmittedBy", trip.SubmittedBy);
                        cmdTrip.Parameters.AddWithValue("@StartDate", trip.StartDate);
                        cmdTrip.Parameters.AddWithValue("@EndDate", trip.EndDate);
                        cmdTrip.Parameters.AddWithValue("@Location", trip.Location);
                        cmdTrip.Parameters.AddWithValue("@StatusId", trip.StatusID);
                        cmdTrip.Parameters.AddWithValue("@ExpenseItems", ConvertToDataTable(expenses, "ExpenseItems"));

                        // Execute the SP and retrieve the TripID
                        using (SqlDataReader reader = cmdTrip.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                trip.TripID = reader.GetInt32(0);
                            }
                            else
                            {
                                throw new Exception("Failed to retrieve TripID after inserting into Trip table.");
                            }
                        }
                    }

                    // Insert into Expense Table using SP
                    foreach (var expense in expenses)
                    {
                        using (SqlCommand cmdExpense = new SqlCommand("BusinessTravel.AddExpense", conn, transaction))
                        {
                            cmdExpense.CommandType = CommandType.StoredProcedure;

                            // Add parameters for the SP
                            cmdExpense.Parameters.AddWithValue("@ExpenseType", expense.ExpenseType);
                            cmdExpense.Parameters.AddWithValue("@ItemCost", expense.ItemCost);
                            cmdExpense.Parameters.AddWithValue("@Description", expense.Description);
                            cmdExpense.Parameters.AddWithValue("@ReceiptImage", expense.ReceiptImage);

                            // Execute the SP
                            cmdExpense.ExecuteNonQuery();
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback();
                    throw new ArgumentException($"{sqlEx.InnerException.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ArgumentException("Kesalahan: " + ex.Message + "-" + ex.Source);
                }
            }
        }


        private DataTable ConvertToDataTable<T>(IEnumerable<T> data, string tableName)
        {
            var table = new DataTable(tableName);
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);

                    if (value != null && value.GetType().IsClass && value.GetType() != typeof(string))
                    {
                        // If the property is a complex object, you may need to handle it separately or convert it to a string, for example.
                        row[property.Name] = value.ToString();
                    }
                    else
                    {
                        row[property.Name] = value ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
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
    }
}
