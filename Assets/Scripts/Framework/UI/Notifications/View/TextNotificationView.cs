using Framework.UI.Notifications.Model;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace Framework.UI.Notifications.View
{
    public class TextNotificationView : NotificationView<TextNotification>
    {
        [UsedImplicitly] public Text TextComponent;

        public override void Initialize(INotification model, float showTime)
        {
            base.Initialize(model, showTime);
            TextComponent.text = Model.Text;
        }
    }
}