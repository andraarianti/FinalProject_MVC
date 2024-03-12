using System;
using System.Collections.Generic;
using BLL.DTO;
using BLL.Interfaces;
using BO;
using DAL;
using DAL.Interfaces;

namespace BLL
{
    public class StaffBLL : IStaffBLL
    {
        public readonly IStaffDAL _staffDAL;
        public StaffBLL()
        {
           _staffDAL = new StaffDAL();
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new Exception("ID is required");
            }

            try
            {
                _staffDAL.Delete(id);   
            }
            catch (Exception ex)
            {
                throw new Exception("Error Delete BLL: " + ex.Message);
            }
        }

        public IEnumerable<StaffDTO> GetAll()
        {
            List<StaffDTO> listStaffDto = new List<StaffDTO>();
            var staffList = _staffDAL.GetAll();
            foreach (var staff in staffList)
            {
                listStaffDto.Add(new StaffDTO
                {
                    StaffID = staff.StaffID,
                    Name = staff.Name,
                    PositionID = staff.PositionID,
                    Role = staff.Role,
                    LastModified = staff.LastModified,
                    IsDeleted = staff.IsDeleted,
                    Username = staff.Username,
                    Password = staff.Password,
                    Email = staff.Email
                });
            }
            return listStaffDto;
        }

        public StaffDTO GetById(int id)
        {
            StaffDTO staffDTO = new StaffDTO();
            var staff = _staffDAL.GetById(id);
            if(staff != null)
            {
                staffDTO.StaffID = staff.StaffID;
                staffDTO.Name = staff.Name;
                staffDTO.PositionID = staff.PositionID;
                staffDTO.Role = staff.Role;
                staffDTO.LastModified = staff.LastModified;
                staffDTO.IsDeleted = staff.IsDeleted;
                staffDTO.Username = staff.Username;
                staffDTO.Password = staff.Password;
                staffDTO.Email = staff.Email;
            }
            else
            {
                throw new Exception("Staff not found");
            }
            return staffDTO;
        }

        public IEnumerable<StaffDTO> GetByName(string username)
        {
            try
            {
                List<StaffDTO> listStaffDto = new List<StaffDTO>();
                var staffList = _staffDAL.GetByName(username);
                foreach (var staff in staffList)
                {
                    listStaffDto.Add(new StaffDTO
                    {
                        StaffID = staff.StaffID,
                        Name = staff.Name,
                        PositionID = staff.PositionID,
                        Role = staff.Role,
                        LastModified = staff.LastModified,
                        IsDeleted = staff.IsDeleted,
                        Username = staff.Username,
                        Password = staff.Password,
                        Email = staff.Email
                    });
                }
                return listStaffDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error GetByName BLL: " + ex.Message);
            }
        }

        public void Insert(StaffDTO obj)
        {
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrEmpty(obj.Username) || string.IsNullOrEmpty(obj.Password) || string.IsNullOrEmpty(obj.Email))
            {
                throw new Exception("Name, Username, Password and Email are required");
            }

            try
            {
                var staff = new Staff
                {
                    Name = obj.Name,
                    PositionID = obj.PositionID,
                    Role = obj.Role,
                    Username = obj.Username,
                    Password = obj.Password,
                    Email = obj.Email
                };
                _staffDAL.Insert(staff);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Insert BLL: " + ex.Message);
            }
        }

        public StaffDTO Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Username and Password are required");
            }

            try
            {
                var result = _staffDAL.Login(username, password);
                if (result == null)
                {
                    throw new Exception("BLL Invalid username or password");
                }
                StaffDTO staffDTO = new StaffDTO
                {
                    Username = result.Username,
                    Password = result.Password,
                    Role = result.Role,
                    Name = result.Name,
                    Email = result.Email
                };

                return staffDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Login BLL: " + ex.Message);
            }
        }

        public void Update(StaffDTO obj)
        {
            try
            {
                var staff = new Staff
                {
                    StaffID = obj.StaffID,
                    Name = obj.Name,
                    PositionID = obj.PositionID,
                    Role = obj.Role,
                    Username = obj.Username,
                    Email = obj.Email
                };

                if (!string.IsNullOrEmpty(obj.Password))
                {
                    staff.Password = obj.Password;
                }
                _staffDAL.Update(staff);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Update BLL: " + ex.Message);
            }
        }
    }
}
