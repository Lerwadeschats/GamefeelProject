using System.Collections;
using UnityEngine;

namespace TCG.Dialogues.Core.Camera
{
    public class CameraShaker : MonoBehaviour
    {
        public static CameraShaker Instance { get; private set; } = null;

        [SerializeField] private float _shakePeriod;


        [SerializeField] float _baseShakePower = 1f;

        [SerializeField] float _baseShakeDuration = 0.5f;
        

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
            StartCoroutine(_UpdateShake(_baseShakePower, _baseShakeDuration));
        }
        public void StartShaking(float shakePower,float shakeDuration)
        {
            StartCoroutine(_UpdateShake(shakePower, shakeDuration));
        }
        IEnumerator _UpdateShake(float shakePower,float shakeDuration )
        {
            float timer = 0;
            Vector3 OriginalPos = transform.localPosition;
            while(timer< shakeDuration)
            {
                float x = Random.Range(-10, 10) * shakePower;
                float y= Random.Range(-10, 10) * shakePower;

                transform.localPosition = new Vector3(x, y, OriginalPos.z);
                timer += _shakePeriod;
                yield return new WaitForSeconds(_shakePeriod);

            }
            transform.localPosition=OriginalPos;
        }
            
    }
    
}