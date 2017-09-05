using System;
using System.Reactive;
using System.Reactive.Subjects;
using ReactiveExtensionsMixins;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void ToSignalShouldReturnObservableOfUnit()
        {
            var desiredResult = typeof(Unit);
            object result = null;
            
            var intSubject = new Subject<int>();
            var unitObservable = intSubject.ToSignal();
            unitObservable.Subscribe(value => result = value);
            intSubject.OnNext(1);

            Assert.Equal(result.GetType(), desiredResult);
        }

        [Fact]
        public void CatchAndReturnShouldReturnSpecifiedValueWhenObservableThrows()
        {
            var desiredResult = -1;
            var result = 0;
            
            var intSubject = new Subject<int>();
            var safeObservable = intSubject.CatchAndReturn(desiredResult);
            safeObservable.Subscribe(value => result = value);
            intSubject.OnError(new Exception());
            
            Assert.Equal(result, desiredResult);
        }

        [Fact]
        public void WhereTrueShouldOnlyPassTrueValues()
        {
            var desiredResult = 1;
            var result = 0;
            
            var boolSubject = new Subject<bool>();
            var trueObservable = boolSubject.Where(true);
            trueObservable.Subscribe(_ => result++);
            boolSubject.OnNext(false);
            boolSubject.OnNext(true);
            
            Assert.Equal(result, desiredResult);
        }

        [Fact]
        public void WhereFalseShouldOnlyPassFalseValues()
        {
            var desiredResult = 1;
            var result = 0;
            
            var boolSubject = new Subject<bool>();
            var trueObservable = boolSubject.Where(false);
            trueObservable.Subscribe(_ => result++);
            boolSubject.OnNext(true);
            boolSubject.OnNext(false);
            
            Assert.Equal(result, desiredResult);
        }

        [Fact]
        public void WhereNotNullShouldNotPassNullValues()
        {
            var desiredResult = 1;
            var result = 0;
            
            var stringSubject = new Subject<string>();
            var notNullObservable = stringSubject.WhereNotNull();
            notNullObservable.Subscribe(value => result++);
            stringSubject.OnNext("");
            stringSubject.OnNext(null);
            
            Assert.Equal(result, desiredResult);
        }

        [Fact]
        public void GetEventsShouldEmitWhenEventIsFired()
        {
            var desiredResult = "Test";
            var result = "";
            
            var eventObject = new EventObject();
            var firedObservable = eventObject.GetEvents<StringEventArgs>(nameof(EventObject.Fired));
            firedObservable.Subscribe(value => result = value.Value);
            eventObject.FireEvent(desiredResult);
            
            Assert.Equal(result, desiredResult);
        }
    }
}