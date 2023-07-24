using UnityEngine;

namespace Assets.Scripts.EducationSystem
{
    public class EducationPanel : MonoBehaviour
    {
        [SerializeField] private DoScaler[] _windows;

        [SerializeField] private int _current;

        public void Next()
        {
            _windows[_current].SetScale(Vector3.zero);

            if(_current < _windows.Length - 1)
            {
                _current++;
            }

            _windows[_current].SetScale(Vector3.one);
        }

        public void Previous() 
        {
            _windows[_current].SetScale(Vector3.zero);

            if (_current > 0)
            {
                _current--;
            }

            _windows[_current].SetScale(Vector3.one);
        }
    }
}