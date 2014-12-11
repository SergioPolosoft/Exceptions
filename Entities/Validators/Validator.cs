using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validators
{
    public interface IValidator<T>
    {
        void Validate<T>(T entity);
    }
}
