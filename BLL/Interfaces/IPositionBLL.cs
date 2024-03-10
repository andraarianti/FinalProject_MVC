using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IPositionBLL
    {
        void Delete(int id);
        IEnumerable<PositionDTO> GetAll();
        PositionDTO GetById(int id);
        void Insert(PositionDTO obj);
        void Update(PositionDTO obj);
    }
}
