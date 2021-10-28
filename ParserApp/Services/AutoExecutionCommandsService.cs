using ParserApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParserApp.Services
{
    public class AutoExecutionCommandsService
    {
        public bool IsRunning { get; set; } = false;
        private Timer _timer;
        public void Start(List<ICommand> commands, List<object?> parameters, TimeSpan delay)
        {
            IsRunning = true;
            _timer = new Timer(
                async (s) => await Loop(commands, parameters, delay),
                null, 0, (int)delay.TotalMilliseconds);
        }

        public void Stop()
        {
            _timer.Dispose();
            IsRunning = false;
        }

        private async Task Loop(List<ICommand> commands, List<object?> parameters, TimeSpan delay)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                ICommand command = commands[i];
                object? parameter = parameters[i];

                if (!command.CanExecute(parameter))
                    continue;

                if (command is AsyncBaseCommand asyncCommand)
                {
                    await asyncCommand.ExecuteWithTask(parameter);
                }
                else
                {
                    command.Execute(parameter);
                }
            }
        }
    }
}
