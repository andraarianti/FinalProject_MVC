using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace DAL.Interfaces
{
    public interface IPositionDAL : ICrudDAL<Position>
    {
        void SetPosition(Position position);
    }
}
