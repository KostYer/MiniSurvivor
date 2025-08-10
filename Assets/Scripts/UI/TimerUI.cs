using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text _timer;

        public void OnTimerTick(float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);

            _timer.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}