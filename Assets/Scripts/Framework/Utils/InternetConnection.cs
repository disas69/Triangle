using System.Collections;
using UnityEngine;

namespace Framework.Utils
{
    public class InternetConnection : MonoBehaviour
    {
        private static InternetConnection _instance;

        [SerializeField] private string _host = "8.8.8.8";
        [SerializeField] private float _pingTimeout = 5f;

        private bool IsHostAvailable { get; set; }

        private IEnumerator Start()
        {
            _instance = this;

            var wait = new WaitForSeconds(_pingTimeout);

            while (true)
            {
                StartCoroutine(CheckConnection());
                yield return wait;
            }
        }

        private IEnumerator CheckConnection()
        {
            var ping = new Ping(_host);
            var pingStartTime = Time.realtimeSinceStartup;

            while (!ping.isDone && Time.realtimeSinceStartup - pingStartTime < _pingTimeout)
            {
                yield return null;
            }

            IsHostAvailable = ping.isDone;
        }

        public static void ForceCheck()
        {
            _instance.StartCoroutine(_instance.CheckConnection());
        }

        public static bool IsAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable && _instance.IsHostAvailable;
        }
    }
}