using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomException
{
    public class ConcurrentUpdateException :Exception
    {
        public override string Message => Resources.Resources.isRemoved;
    }
}
