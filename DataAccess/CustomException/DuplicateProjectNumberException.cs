using DataAccess.Repository;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomException
{
    public class DuplicateProjectNumberException : Exception
    {
        public override string Message => Resources.Resources.Duplicate;
    }
}
