using BigBrother.UILogic.Models;
using System;
using System.Globalization;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace BigBrother.UILogic.Events 
{
    public class BackgroundSubscriber
    {
        CoreDispatcher _dispatcher;
        public BackgroundSubscriber(CoreDispatcher dispatcher) {
            _dispatcher = dispatcher;
        }

        public void HandlePhoneCallDeleted(PhoneCall phoneCall) {
            var threadId = Environment.CurrentManagedThreadId;
            var phoneCallId = phoneCall.Id;

            // Assign into local variable because it is meant to be fire and forget and calling would require an await/async
            var dialogAction = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MessageDialog dialog = new MessageDialog(string.Format(CultureInfo.InvariantCulture,
                    "PhoneCall {0} deleted in background subscriber on thread {1}", phoneCallId, threadId));
                var showAsync = dialog.ShowAsync();
            });
        }
    }
}
