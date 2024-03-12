using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BLL.Interfaces;
using BO;
using DAL;
using DAL.Interfaces;

namespace BLL
{
    public class TripBLL : ITripBLL
    {
        public readonly ITripDAL _tripDAL;
        public TripBLL()
        {
            _tripDAL = new TripDAL();
        }
        public IEnumerable<ReadTripDTO> GetAllWithStatus()
        {
            List<ReadTripDTO> listTripDto = new List<ReadTripDTO>();
            var tripList = _tripDAL.GetAll();
            foreach (var trip in tripList)
            {
                listTripDto.Add(new ReadTripDTO
                {
                    TripID = trip.TripID,
                    SubmittedBy = trip.SubmittedBy,
                    Location = trip.Location,
                    StartDate = trip.StartDate.Date,
                    EndDate = trip.EndDate.Date,                   
                    TotalCost = trip.TotalCost,
                    StatusID = trip.StatusID,
                    Status = new StatusDTO
                    {
                        StatusID = trip.Status.StatusID,
                        StatusName = trip.Status.StatusName
                    },
                    Staff = new StaffDTO
                    {
                        StaffID = trip.Staff.StaffID,
                        Name = trip.Staff.Name,
                    }
                    
                });
            }                   
            return listTripDto;
        }

        public ReadTripDTO GetByIdTrip(int id)
        {
           ReadTripDTO trip = new ReadTripDTO();
            var tripEntity = _tripDAL.GetById(id);
            trip.TripID = tripEntity.TripID;
            trip.SubmittedBy = tripEntity.SubmittedBy;
            trip.Location = tripEntity.Location;
            trip.StartDate = tripEntity.StartDate.Date;
            trip.EndDate = tripEntity.EndDate.Date;
            trip.TotalCost = tripEntity.TotalCost;
            trip.StatusID = tripEntity.StatusID;
            trip.Status = new StatusDTO
            {
                StatusID = tripEntity.Status.StatusID,
                StatusName = tripEntity.Status.StatusName
            };
            trip.Staff = new StaffDTO
            {
                StaffID = tripEntity.Staff.StaffID,
                Name = tripEntity.Staff.Name,
            };
            return trip;
        }

        public List<ExpenseItemsDTO> GetTripWithExpenseByTripId(int id)
        {
            List<ExpenseItemsDTO> listExpenseDto = new List<ExpenseItemsDTO>();
            var expenseList = _tripDAL.GetTripWithExpenseByTripId(id);

            foreach (var expense in expenseList)
            {
                listExpenseDto.Add(new ExpenseItemsDTO
                {
                    ExpenseID = expense.ExpenseID,
                    TripID = expense.TripID,
                    ExpenseType = expense.ExpenseType,
                    ItemCost = expense.ItemCost,
                    Description = expense.Description,
                    ReceiptImage = expense.ReceiptImage,
                    IsApproved = expense.IsApproved,
                });
            }

            return listExpenseDto;
        }

        public IEnumerable<ReadTripDTO> GetTripWithAttendees()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReadTripDTO> GetTripWithAttendeesAndExpense()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReadTripDTO> GetTripWithExpense()
        {
            throw new NotImplementedException();
        }

        public void CreateTrip(CreateTripReportDTO tripReport, List<ExpenseItemsDTO> expense)
        {
            var tripEntity = new Trip()
            {
                SubmittedBy = tripReport.SubmittedBy,
                Location = tripReport.Location,
                StartDate = tripReport.StartDate,
                EndDate = tripReport.EndDate,
                TotalCost = tripReport.TotalCost,
                StatusID = tripReport.StatusID,
            };

            var expenseItems = new List<Expense>();

            foreach (var item in expense)
            {
                expenseItems.Add(new Expense
                {
                    ExpenseType = item.ExpenseType,
                    ItemCost = item.ItemCost,
                    Description = item.Description,
                    ReceiptImage = item.ReceiptImage,
                });
            }

            _tripDAL.InsertWithExpenseAndAttendees(tripEntity, expenseItems);
        }

        public void CreateExpense(ExpenseItemsDTO expense)
        {
            try
            {
                
                var expenseEntity = new Expense()
                {
                    TripID = expense.TripID,
                    ExpenseType = expense.ExpenseType,
                    ItemCost = expense.ItemCost,
                    Description = expense.Description,
                    ReceiptImage = expense.ReceiptImage,
                };
                _tripDAL.CreateExpense(expenseEntity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void ClaimReimbursmnt(CreateTripReportDTO tripReport)
        {
            try
            {
                var trip = new Trip()
                {
                    TripID = tripReport.TripID,
                    StatusID = 2,
                };
                _tripDAL.ClaimReimbusment(trip);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteExpense(ExpenseItemsDTO expenseItems)
        {
            try
            {
                var expense = new Expense()
                {
                    ExpenseID = expenseItems.ExpenseID,
                    TripID = expenseItems.TripID,
                };
                _tripDAL.DeleteExpense(expense);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ExpenseItemsDTO GetExpenseByExpenseID(int id)
        {
            try
            {
                ExpenseItemsDTO expense = new ExpenseItemsDTO();
                var expenseEntity = _tripDAL.GetExpensesById(id);
                expense.ExpenseID = expenseEntity.ExpenseID;
                expense.TripID = expenseEntity.TripID;
                expense.ExpenseType = expenseEntity.ExpenseType;
                expense.ItemCost = expenseEntity.ItemCost;
                expense.Description = expenseEntity.Description;
                expense.ReceiptImage = expenseEntity.ReceiptImage;
                return expense;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateExpense(ExpenseItemsDTO expenseItems)
        {
            try
            {
                var expense = new Expense()
                {
                    ExpenseID = expenseItems.ExpenseID,
                    TripID = expenseItems.TripID,
                    ExpenseType = expenseItems.ExpenseType,
                    ItemCost = expenseItems.ItemCost,
                    Description = expenseItems.Description,
                    ReceiptImage = expenseItems.ReceiptImage,
                };
                _tripDAL.UpdateExpense(expense);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteTrip(int tripId)
        {
            try
            {
                _tripDAL.Delete(tripId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SubmitApproval(int id, int statusId)
        {
            try
            {
                _tripDAL.SubmitApproval(id, statusId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ITripBLL> GetAllByStaffId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReadTripDTO> GetTripByUserId(int id)
        {
            List<ReadTripDTO> readTripDTOs = new List<ReadTripDTO>();
            var expenseList = _tripDAL.GetTripByUserId(id);

            foreach (var expense in expenseList)
            {
                readTripDTOs.Add(new ReadTripDTO
                {
                    TripID = expense.TripID,
                    SubmittedBy = expense.SubmittedBy,
                    Location = expense.Location,
                    StartDate = expense.StartDate.Date,
                    EndDate = expense.EndDate.Date,
                    TotalCost = expense.TotalCost,
                    StatusID = expense.StatusID,
                    Status = new StatusDTO
                    {
                        StatusID = expense.Status.StatusID,
                        StatusName = expense.Status.StatusName
                    },
                    Staff = new StaffDTO
                    {
                        StaffID = expense.Staff.StaffID,
                        Name = expense.Staff.Name,
                    }

                });
            }

            return readTripDTOs;
        }
    }
}
