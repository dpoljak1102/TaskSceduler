using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSceduler.App.Service.Common
{
    /// <summary>
    /// IHandleParameters interface for ViewModels that need to handle navigation parameters
    /// </summary>
    public interface IHandleParameters
    {
        void HandleParameters(object parameters);
    }
}
