using UnityEngine;

namespace Framework.UI.Notifications.Model
{
    public class IconNotification : INotification
    {
        public NotificationType Type
        {
            get { return NotificationType.Icon; }
        }

        public Sprite Icon { get; private set; }

        public IconNotification(Sprite icon)
        {
            Icon = icon;
        }
    }
}