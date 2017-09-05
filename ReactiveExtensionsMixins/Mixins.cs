using System;
using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveExtensionsMixins
{
    public static class Mixins
    {
        /// <summary>
        /// Converts an Observable&lt;<c>T</c>&gt; to an Observable&lt;<c>Unit</c>&gt;
        /// </summary>
        /// <param name="source">The observable to be converted</param>
        /// <typeparam name="T">The type of values in the original observable</typeparam>
        /// <returns>An <c>IObservable&lt;Unit&gt;</c></returns>
        public static IObservable<Unit> ToSignal<T>(this IObservable<T> source)
            => source.Select(_ => Unit.Default);



        /// <summary>
        /// Catches any exception in the observable chain and returns a value in it's place
        /// </summary>
        /// <param name="source">The observable that might throw an exception</param>
        /// <param name="returnValue">The value to be returned if an exception is thrown</param>
        /// <typeparam name="T">The type of the values in the observable</typeparam>
        /// <returns>An <c>IObservable&lt;T&gt;</c> which will return the specified value if an exception is thrown</returns>
        public static IObservable<T> CatchAndReturn<T>(this IObservable<T> source, T returnValue)
            => source.Catch(Observable.Return(returnValue));



        /// <summary>
        /// Filters an <c>IObservable&lt;bool&gt;</c> to only contain either <c>true</c> or <c>false</c>
        /// </summary>
        /// <param name="source">The observable to be filtered</param>
        /// <param name="desiredValue">The only value that should be emitted by the new observable</param>
        /// <returns>An <c>IObservable&lt;bool&gt;</c> where only the desired boolean value is emitted</returns>
        public static IObservable<bool> Where(this IObservable<bool> source, bool desiredValue)
            => source.Where(value => value == desiredValue);



        /// <summary>
        /// Filters an <c>IObservable&lt;T&gt;</c> to only contain values that are not <c>null</c>
        /// </summary>
        /// <param name="source">The observable to be filtered</param>
        /// <typeparam name="T">The type of the values in the observable</typeparam>
        /// <returns>An <c>IObservable&lt;T&gt;</c> where all values are guaranteed not to be <c>null</c></returns>
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> source) where T : class
            => source.Where(value => value != null);



        /// <summary>
        /// Creates an observable that will emit the <c>EventArgs</c> of an event, every time the event fires
        /// </summary>
        /// <param name="source">The object on which the event fires</param>
        /// <param name="eventName">The name of the event that fires (use <c>nameof()</c> to prevent typos)</param>
        /// <typeparam name="TEventArgs">The type of the arguments the event passes (use <c>EventArgs</c> if the specific type is irrelevant)</typeparam>
        /// <returns></returns>
        public static IObservable<TEventArgs> GetEvents<TEventArgs>(this object source, string eventName)
            => Observable.FromEventPattern<TEventArgs>(source, eventName).Select(eventPattern => eventPattern.EventArgs);



        /// <summary>
        /// Logs <c>OnNext</c>, <c>OnError</c> and <c>OnCompleted</c> signals to the debug output
        /// </summary>
        /// <param name="source">The observable to log to the debug output</param>
        /// <param name="selector">A selector function to pick the value to be logged for <c>OnNext</c> signals</param>
        /// <param name="prefix">A string to prefix the log messages with, to easier identify them</param>
        /// <typeparam name="T">The type of the values in the observable</typeparam>
        /// <returns>An <c>IObservable&lt;T&gt;</c> that logs all signals to the debug output</returns>
        public static IObservable<T> Debug<T>(this IObservable<T> source, Func<T, object> selector, string prefix = null)
        {
            prefix = prefix == null ? "" : prefix + " ";
            return source.Do(
                value => System.Diagnostics.Debug.WriteLine($"{prefix} Next: {selector(value)}"),
                ex => System.Diagnostics.Debug.WriteLine($"{prefix} Error: {ex}"),
                () => System.Diagnostics.Debug.WriteLine($"{prefix} Completed"));
        }



        /// <summary>
        /// Logs <c>OnNext</c>, <c>OnError</c> and <c>OnCompleted</c> signals to the debug output
        /// </summary>
        /// <param name="source">The observable to log to the debug output</param>
        /// <param name="prefix">A string to prefix the log messages with, to easier identify them</param>
        /// <typeparam name="T">The type of the values in the observable</typeparam>
        /// <returns>An <c>IObservable&lt;T&gt;</c> that logs all signals to the debug output</returns>
        public static IObservable<T> Debug<T>(this IObservable<T> source, string prefix = null)
            => source.Debug(value => value, prefix);
    }
}