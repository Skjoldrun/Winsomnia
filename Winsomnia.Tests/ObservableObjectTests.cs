using System.ComponentModel;
using Xunit;
using Winsomnia.Utility;

namespace Winsomnia.Tests
{
    public class ObservableObjectTests
    {
        private sealed class Subject : ObservableObject
        {
            private string? _name;

            public string? Name
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public string? NameViaCaller
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        [Fact]
        public void OnPropertyChanged_RaisesEventWithPropertyName()
        {
            var subject = new Subject();
            string? raisedName = null;
            subject.PropertyChanged += (_, e) => raisedName = e.PropertyName;

            subject.Name = "test";

            Assert.Equal("Name", raisedName);
        }

        [Fact]
        public void OnPropertyChanged_UsesCallerMemberName()
        {
            var subject = new Subject();
            string? raisedName = null;
            subject.PropertyChanged += (_, e) => raisedName = e.PropertyName;

            subject.NameViaCaller = "test";

            Assert.Equal("NameViaCaller", raisedName);
        }

        [Fact]
        public void OnPropertyChanged_DoesNotThrowWhenNoSubscribers()
        {
            var subject = new Subject();

            subject.Name = "test";

            Assert.Equal("test", subject.Name);
        }
    }
}
