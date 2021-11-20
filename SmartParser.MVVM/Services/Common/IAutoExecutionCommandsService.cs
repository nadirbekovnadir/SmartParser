using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SmartParser.MVVM.Services.Common
{
    public interface IAutoExecutionCommandsService
    {
        bool IsRunning { get; set; }

        void Start(List<ICommand> commands, List<object?> parameters, TimeSpan delay);
        void Stop();
    }
}