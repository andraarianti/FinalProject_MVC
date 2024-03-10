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
    public class PositionBLL : IPositionBLL
    {
        public readonly IPositionDAL _positionDAL;

        public PositionBLL()
        {
            _positionDAL = new PositionDAL();
        }

        public void Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("ID is required");
            }

            try
            {
                _positionDAL.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Delete BLL: " + ex.Message);
            }
        }

        public IEnumerable<PositionDTO> GetAll()
        {
            List<PositionDTO> listPositionDTO = new List<PositionDTO>();
            var positionList = _positionDAL.GetAll();
            foreach (var position in positionList)
            {
                listPositionDTO.Add(new PositionDTO
                {
                    PositionID = position.PositionID,
                    PositionName = position.PositionName
                });
            }
            return listPositionDTO;
        }

        public PositionDTO GetById(int id)
        {
            PositionDTO position = new PositionDTO();
            var positionDal = _positionDAL.GetById(id);

            if (positionDal == null)
            {
                throw new ArgumentException($"Data article with id:{id} not found");
            }

            position.PositionID = positionDal.PositionID;
            position.PositionName = positionDal.PositionName;

            return position;
        }

        public void Insert(PositionDTO obj)
        {
            if (string.IsNullOrEmpty(obj.PositionName))
            {
                throw new Exception("Position Name is required");
            }

            try 
            {
                var newPosition = new Position
                {
                    PositionName = obj.PositionName
                };
                _positionDAL.Insert(newPosition);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public void Update(PositionDTO obj)
        {
            if (obj.PositionID <= 0)
            {
                throw new ArgumentException("ID is required");
            }
            if (string.IsNullOrEmpty(obj.PositionName))
            {
                throw new ArgumentException("Position Name is required");
            }

            try
            {
                var position = new Position
                {
                    PositionID = obj.PositionID,
                    PositionName = obj.PositionName
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
