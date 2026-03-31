using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek karakter (woman)
    public Vector3 offset;   // Kamera ile karakter arasındaki mesafe
    public float smoothSpeed = 0.125f; // Takip yumuşaklığı

    void Start()
    {
        // Başlangıçta aradaki mesafeyi otomatik hesapla
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Kameranın gitmesi gereken hedef pozisyon
        Vector3 desiredPosition = target.position + offset;
        
        // Mevcut pozisyondan hedef pozisyona yumuşak bir geçiş yap (Lerp)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Kameranın yeni pozisyonunu ayarla
        transform.position = smoothedPosition;
        
    }
}