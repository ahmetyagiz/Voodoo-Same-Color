using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShakeEffect : MonoBehaviour
{
    public GameObject[] capsules;  // Kaps�llerin referans�
    public float maxShakeDistance = 5f;  // En uzak kaps�le kadar olan mesafe
    public float shakeDuration = 1f;  // Dalgalanma s�resi
    public float shakeStrength = 1f;  // Dalgalanma g�c�
    public float delayFactor = 0.1f; // Gecikme fakt�r�
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

        // Kaps�lleri al ve her bir kaps�le etkisini uygula
        for (int i = 0; i < capsules.Length; i++)
        {
            GameObject capsule = capsules[i];

            // T�klama noktas�na olan mesafeyi hesapla
            float distance = Vector3.Distance(clickPosition, capsule.transform.position);

            // Dalgalanma �iddeti mesafeye g�re azal�r
            float shakeAmount = Mathf.Lerp(shakeStrength, 0, distance / maxShakeDistance);

            // Gecikmeyi mesafeye g�re hesapla
            float delay = Mathf.Lerp(0, distance * delayFactor, 1);

            // Kaps�lleri ayn� anda sallamak i�in ayn� zaman diliminde dalgalanma uygula
            if (shakeAmount > 0)
            {
                // �lk pozisyon
                Vector3 originalPosition = capsule.transform.position;

                capsule.transform.DOMoveY(1.5f, shakeDuration / 2).SetDelay(delay)
                    .OnComplete(() =>
                    {
                        capsule.transform.DOMove(originalPosition, shakeDuration / 3);
                    });

                // Rotasyonu �ok etmek i�in DOShakeRotation kullan�yoruz
                capsule.transform.DOShakeRotation(shakeDuration, shakeStrength, 20, 90).SetDelay(delay)
                    .OnKill(() => capsule.transform.position = originalPosition);  // Shake tamamland�ktan sonra pozisyonu s�f�rla
            }
        }

        // T�m kaps�llerin hareketi tamamlanana kadar bekle
        yield return new WaitForSeconds(shakeDuration + 0.5f);

        isShaking = false;
    }
}