using System;

namespace Tests
{
    public class EventObject
    {
        public event EventHandler<StringEventArgs> Fired;

        public void FireEvent(string value) => Fired?.Invoke(this, new StringEventArgs(value));
    }

    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string value) => Value = value; 

        public string Value { get; }
    }
}