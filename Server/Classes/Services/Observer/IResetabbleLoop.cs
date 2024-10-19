using System.Security.Cryptography.X509Certificates;

namespace Server.Classes.Services.Observer
{
    public interface IResetabbleLoop
    {
        public void ResetAfterLevelChange();
        public void SubscriberToLevelChange()
        {
            ResetEvent.AddToEvent(this);
        }

    }
}
