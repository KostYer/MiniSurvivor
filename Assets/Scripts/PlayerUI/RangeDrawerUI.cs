using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUI
{
    public class RangeDrawerUI: MonoBehaviour
    {
        [SerializeField] private RectTransform _circleMain;
        [SerializeField] private RectTransform _edgeCircle;
        
        private RangeUISettings _uiSettings;
        private ShootSettings _shootSettings;
        
        
        public void Initialize(RangeUISettings uiSettings,  ShootSettings shootSettings)
        {
            _uiSettings = uiSettings;
            _shootSettings = shootSettings;

            var diameter = _shootSettings.ShootRange * 2f - uiSettings.VisualEpsilon;
            _circleMain.sizeDelta = new Vector2(diameter, diameter);
            _edgeCircle.sizeDelta = new Vector2(diameter, diameter);

            _circleMain.GetComponent<Image>().color = _uiSettings.CircleColor;
            _edgeCircle.GetComponent<Image>().color = _uiSettings.EdgeColor;
        }
    }
}