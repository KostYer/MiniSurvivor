using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveNumUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text _waveNumber;

        public void OnWaveChanged(int wave)
        {
            _waveNumber.text = $"wave {wave}";
        }
    }
}