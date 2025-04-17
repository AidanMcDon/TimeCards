using UnityEngine;

public static class TimeManager
{
    public static float timeScale = 1f;
    private static float lastTimeScale = 1f;
    public static float DeltaTime => Time.deltaTime * timeScale;
    public static float FixedDeltaTime => Time.fixedDeltaTime * timeScale;
    public static float UnscaledDeltaTime => Time.unscaledDeltaTime * timeScale;

    public static void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
    }
    public static void Pause(){
        lastTimeScale = timeScale;
        timeScale = 0f;
    }

    public static void Unpause(){
        timeScale = lastTimeScale;
    }

    public class WaitForGameSeconds : CustomYieldInstruction
    {
        private float elapsed = 0;
        private float duration;
        
        public WaitForGameSeconds(float seconds)
        {
            duration = seconds;
        }
        
        public override bool keepWaiting
        {
            get
            {
                elapsed += DeltaTime;
                return elapsed < duration;
            }
        }
    }
}
