using PrismTemplates.UILogic.Models;
using Microsoft.Practices.Prism.PubSubEvents;

namespace PrismTemplates.UILogic.Events
{
    public class EntityDeletedEvent : PubSubEvent<Entity>
    {
    }
}