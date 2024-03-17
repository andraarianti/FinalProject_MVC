using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ITripBLL
    {
        //Get Data
        IEnumerable<ReadTripDTO> GetAllWithStatus();
        IEnumerable<ReadTripDTO> GetAllWithoutDrafted();
        ReadTripDTO GetByIdTrip(int id);
        IEnumerable<ReadTripDTO> GetTripByUserId(int id);
        IEnumerable<ReadTripDTO> GetTripWithExpense();
        List<ExpenseItemsDTO> GetTripWithExpenseByTripId(int id);
        IEnumerable<ReadTripDTO> GetTripWithAttendees();
        IEnumerable<ReadTripDTO> GetTripWithAttendeesAndExpense();
        ExpenseItemsDTO GetExpenseByExpenseID(int id);

        //Create Data
        void CreateTrip(CreateTripReportDTO tripReport, List<ExpenseItemsDTO> expense);
        void CreateExpense(ExpenseItemsDTO expense);

        //Update Data
        void ClaimReimbursmnt(CreateTripReportDTO createTripReportDTO);
        void UpdateExpense(ExpenseItemsDTO expenseItems);
        void SubmitApproval(int id, int statusId);

        //Delete Data
        void DeleteTrip(int id);
        void DeleteExpense(ExpenseItemsDTO expenseItems);
    }
}
