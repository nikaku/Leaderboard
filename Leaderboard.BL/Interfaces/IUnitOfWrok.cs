using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Interfaces
{
    public interface IUnitOfWrok
    {
        void SaveChanges();
        void Dispose();
    }
}
