using System;
using System.Windows.Input;
using Xunit;
using Winsomnia.Command;

namespace Winsomnia.Tests
{
    public class DelegateCommandTests
    {
        [Fact]
        public void Execute_CallsCommandAction()
        {
            bool executed = false;
            var command = new DelegateCommand
            {
                CommandAction = () => executed = true,
            };

            command.Execute(null);

            Assert.True(executed);
        }

        [Fact]
        public void CanExecute_ReturnsTrueWhenFuncIsNull()
        {
            var command = new DelegateCommand
            {
                CommandAction = () => { },
            };

            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void CanExecute_ReturnsFuncResult()
        {
            var command = new DelegateCommand
            {
                CommandAction = () => { },
                CanExecuteFunc = () => false,
            };

            Assert.False(command.CanExecute(null));
        }

        [Fact]
        public void ImplementsICommand()
        {
            var command = new DelegateCommand();

            Assert.IsAssignableFrom<ICommand>(command);
        }
    }
}
