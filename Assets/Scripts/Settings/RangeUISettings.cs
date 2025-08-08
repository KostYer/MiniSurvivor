using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "RangeUISettingsSO", menuName = "Settings/RangeUISettings")]
    public class RangeUISettings: ScriptableObject
    {
        [SerializeField] private Color _circleColor;
        [SerializeField] private Color _edgeColor;
        [SerializeField] private float _visualEpsilon = .15f;

        public Color CircleColor => _circleColor;
        public Color EdgeColor => _edgeColor;
        public float VisualEpsilon => _visualEpsilon;
    }
}