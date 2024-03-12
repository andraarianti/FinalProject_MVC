using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace DAL.Interfaces
{
    public interface ITripDAL : ICrudDAL<Trip>
    {
        IEnumerable<Trip> GetTripByUserId(int staffID);
        IEnumerable<Trip> GetTripWithExpense(int TripID);
        IEnumerable<Trip> GetTripWithAttendees(int TripID);
        Expense GetExpensesById(int ExpenseID);
        IEnumerable<Expense> GetTripWithExpenseByTripId(int TripID);

        //Create Data
        void CreateExpense(Expense expense);
        void InsertWithExpenseAndAttendees(Trip trip, List<Expense> expenses);
        //Update Data
        void ClaimReimbusment(Trip trip);
        void UpdateExpense(Expense expense);
        void SubmitApproval(int tripId, int statusId);

        //Delete Data
        void DeleteExpense(Expense expense);
    }
}
