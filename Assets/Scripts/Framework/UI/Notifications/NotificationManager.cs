using System.Collections.Generic;
using Framework.UI.Notifications.Configuration;
using Framework.UI.Notifications.Model;
using Framework.UI.Notifications.View;
using JetBrains.Annotations;
using UnityEngine;

namespace Framework.UI.Notifications
{
    public class NotificationManager : MonoBehaviour
    {
        private static NotificationManager _instance;
        private static Dictionary<NotificationType, NotificationView> _activeNotification;

        [UsedImplicitly] public Transform RootTransform;
        [UsedImplicitly] public NotificationsConfiguration Configuration;

        private void Start()
        {
            _instance = this;
            _activeNotification = new Dictionary<NotificationType, NotificationView>();
        }

        public static void Show(INotification notification, float showTime)
        {
            if (_instance != null)
            {
                Hide(notification.Type);

                var notificationView = _instance.Configuration.GetNotificationView(notification.Type);
                if (notificationView != null)
                {
                    var notificationInstance = Instantiate(notificationView, _instance.RootTransform);
                    notificationInstance.Initialize(notification, showTime);
                    notificationInstance.Show();

                    _activeNotification.Add(notification.Type, notificationInstance);
                }
                else
                {
                    Debug.LogWarning(string.Format("Notification View for type {0} is not assigned", notification.Type));
                }
            }
            else
            {
                Debug.LogWarning("Failed to show notification. NotificationManager component must be placed and configured in the main scene in order to work properly.");
            }
        }

        public static void Hide(NotificationType type)
        {
            NotificationView notification;
            if (_activeNotification.TryGetValue(type, out notification))
            {
                if (notification != null)
                {
                    notification.Hide();
                }
            }

            _activeNotification.Remove(type);
        }

        public static void HideAll()
        {
            foreach (var notificationView in _activeNotification)
            {
                if (notificationView.Value != null)
                {
                    notificationView.Value.Hide();
                }
            }

            _activeNotification.Clear();
        }
    }
}