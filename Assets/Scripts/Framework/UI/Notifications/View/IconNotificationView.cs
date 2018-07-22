using Framework.UI.Notifications.Model;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace Framework.UI.Notifications.View
{
    public class IconNotificationView : NotificationView<IconNotification>
    {
        [UsedImplicitly] public Image ImageComponent;

        public override void Initialize(INotification model, float showTime)
        {
            base.Initialize(model, showTime);
            ImageComponent.sprite = Model.Icon;
        }
    }
}