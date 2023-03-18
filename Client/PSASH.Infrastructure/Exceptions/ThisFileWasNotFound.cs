using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSASH.Infrastructure.Exceptions
{
    public class ThisFileWasNotFound : InfrastructureException
    {
        public ThisFileWasNotFound()
            : base($"Данный файл не найден")
        {
        }
    }
}
