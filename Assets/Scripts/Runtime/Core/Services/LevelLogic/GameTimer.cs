using UnityEngine;

namespace Match2.Core.Services.LevelLogic
{
    public class GameTimer
    {
        private float currentTime;
        private bool isRunning;

        public float CurrentTime => currentTime;

        public void Initialize(float startTime)
        {
            currentTime = startTime;
            isRunning = false;
        }

        public void StartTimer()
        {
            isRunning = true;
        }

        public void StopTimer()
        {
            isRunning = false;
        }

        public void Update(float deltaTime)
        {
            if (!isRunning) return;

            currentTime = Mathf.Max(0, currentTime - deltaTime);
        }

        public bool IsTimeUp() => currentTime <= 0;
    }
}