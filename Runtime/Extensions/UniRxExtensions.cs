using System;
using UniRx;

namespace AByte.UKit.Extensions
{

    public static partial class UniRxExtensions
    {
        public static IDisposable SubscribeToTMProText(this IObservable<string> source, TMPro.TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }

        public static IDisposable SubscribeToTMProText<T>(this IObservable<T> source, TMPro.TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }

        public static IDisposable SubscribeToTMProText<T>(this IObservable<T> source, TMPro.TextMeshProUGUI text, Func<T, string> selector)
        {
            return source.SubscribeWithState2(text, selector, (x, t, s) => t.text = s(x));
        }
    }
}