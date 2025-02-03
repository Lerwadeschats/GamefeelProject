using System.Collections;
using UnityEngine;

namespace TCG.Dialogues.Core.Camera
{
    public class CameraShaker : MonoBehaviour
    {
        public static CameraShaker Instance { get; private set; } = null;

        [SerializeField] private float _shakePeriod = 0.05f;

        public bool IsShaking { get; private set; } = false;


        [SerializeField] float _shakePower = 1f;

        [SerializeField] float _shakeDuration = 0.5f;
        

        private Vector3 _shakeOffset = Vector3.zero;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            StartShaking();
        }

        private void StartShaking()
        {
            StartCoroutine(_UpdateShake());
        }

        public void Shake(float power, float duration)
        {
            _shakePower = power;
            _shakeDuration = duration;
            IsShaking = true;
        }

        public void ShakeStop()
        {
            transform.position -= _shakeOffset;
            _shakeOffset = Vector3.zero;
            IsShaking = false;
        }

        IEnumerator _UpdateShake()
        {
            float timer = 0;
            Vector3 OriginalPos = transform.localPosition;
            while(timer< _shakeDuration)
            {
                float x = Random.Range(-10, 10) * _shakePower;
                float y= Random.Range(-10, 10) * _shakePower;

                transform.localPosition = new Vector3(x, y, OriginalPos.z);
                timer += _shakePeriod;
                yield return new WaitForSeconds(_shakePeriod);

            }
            transform.localPosition=OriginalPos;
        }
            
    }
    
}