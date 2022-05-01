using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class TimeService
    {
        private const float SlowDownScale = 0.1f;
        public bool IsTimeSlowDown { get; private set; }

        public void Stop() => 
            Time.timeScale = 0f;

        public void SlowDown()
        {
            Time.timeScale = SlowDownScale;
            IsTimeSlowDown = true;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            IsTimeSlowDown = false;
        }
    }
}
