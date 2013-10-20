using PrismTemplates.UILogic.Models;
using System;
using System.Globalization;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace PrismTemplates.UILogic.Events 
{
    public class BackgroundSubscriber
    {
        CoreDispatcher _dispatcher;
        public BackgroundSubscriber(CoreDispatcher dispatcher) {
            _dispatcher = dispatcher;
        }

        public void HandlePhoneCallDeleted(Entity entity) {
            var threadId = Environment.CurrentManagedThreadId;
            var entityId = entity.Id;

            // Assign into local variable because it is meant to be fire and forget and calling would require an await/async
            var dialogAction = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MessageDialog dialog = new MessageDialog(string.Format(CultureInfo.InvariantCulture,
                    "PhoneCall {0} deleted in background subscriber on thread {1}", entityId, threadId));
                var showAsync = dialog.ShowAsync();
            });
        }
    }
}