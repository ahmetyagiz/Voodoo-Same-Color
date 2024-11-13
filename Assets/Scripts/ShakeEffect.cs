using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShakeEffect : MonoBehaviour
{
    public GameObject[] capsules;  // Kapsüllerin referansý
    public float maxShakeDistance = 5f;  // En uzak kapsüle kadar olan mesafe
    public float shakeDuration = 1f;  // Dalgalanma süresi
    public float shakeStrength = 1f;  // Dalgalanma gücü
    public float delayFactor = 0.1f; // Gecikme faktörü
    private bool isShaking = false;

    public void StartShakeEffect(Vector3 shakePosition)
    {
        if (!isShaking)
        {
            StartCoroutine(Shake(shakePosition));
        }
    }

    private IEnumerator Shake(Vector3 clickPosition)
    {
        isShaking = true;

        // Kapsülleri al ve her bir kapsüle etkisini uygula
        for (int i = 0; i < capsules.Length; i++)
        {
            GameObject capsule = capsules[i];

            // Týklama noktasýna olan mesafeyi hesapla
            float distance = Vector3.Distance(clickPosition, capsule.transform.position);

            // Dalgalanma þiddeti mesafeye göre azalýr
            float shakeAmount = Mathf.Lerp(shakeStrength, 0, distance / maxShakeDistance);

            // Gecikmeyi mesafeye göre hesapla
            float delay = Mathf.Lerp(0, distance * delayFactor, 1);

            // Kapsülleri ayný anda sallamak için ayný zaman diliminde dalgalanma uygula
            if (shakeAmount > 0)
            {
                // Ýlk pozisyon
                Vector3 originalPosition = capsule.transform.position;

                capsule.transform.DOMoveY(1.5f, shakeDuration / 2).SetDelay(delay)
                    .OnComplete(() =>
                    {
                        capsule.transform.DOMove(originalPosition, shakeDuration / 3);
                    });

                // Rotasyonu þok etmek için DOShakeRotation kullanýyoruz
                capsule.transform.DOShakeRotation(shakeDuration, shakeStrength, 20, 90).SetDelay(delay)
                    .OnKill(() => capsule.transform.position = originalPosition);  // Shake tamamlandýktan sonra pozisyonu sýfýrla
            }
        }

        // Tüm kapsüllerin hareketi tamamlanana kadar bekle
        yield return new WaitForSeconds(shakeDuration + 0.5f);

        isShaking = false;
    }
}