using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private ShakeEffect shakeEffect;

    private void Update()
    {
        // Dokunma ilk kez bu frame'de baþladýysa
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            OnClicked();
        }
    }

    private void OnClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetPointerPosition());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Capsule"))
            {
                Vector3 hitPos = hit.transform.position;
                shakeEffect.StartShakeEffect(hitPos);
            }
        }
    }

    public Vector2 GetPointerPosition()
    {
        return Pointer.current.position.ReadValue();
    }
}