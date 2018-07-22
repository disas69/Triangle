namespace Framework.UI.Notifications.Model
{
    public class TextNotification : INotification
    {
        public NotificationType Type
        {
            get { return NotificationType.Text; }
        }

        public string Text { get; private set; }

        public TextNotification(string text)
        {
            Text = text;
        }
    }
}