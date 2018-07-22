using System;
using System.Collections.Generic;
using Framework.UI.Notifications.View;
using UnityEngine;

namespace Framework.UI.Notifications.Configuration
{
    [Serializable]
    [CreateAssetMenu(fileName = "NotificationsConfiguration", menuName = "UI/NotificationsConfiguration")]
    public class NotificationsConfiguration : ScriptableObject
    {
        public List<NotificationConfig> Configs = new List<NotificationConfig>();

        public NotificationView GetNotificationView(NotificationType type)
        {
            var config = Configs.Find(c => c.Type == type);
            if (config != null)
            {
                return config.Prefab;
            }

            Debug.LogWarning(string.Format("Failed to find NotificationConfig of type {0}", type));
            return null;
        }
    }

    [Serializable]
    public class NotificationConfig
    {
        public NotificationType Type;
        public NotificationView Prefab;
    }
}