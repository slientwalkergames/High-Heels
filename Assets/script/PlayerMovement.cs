using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f; // İleri gitme hızı
    public float sideSpeed = 5f;    // Sağa-sola gitme hızı
    public float limitX = 2.5f;     // Yolun dışına çıkmaması için sınır

    private Vector3 touchStartPos;
    private Vector3 playerStartPos;

    void Update()
    {
        // 1. İleri hareket (Sabit)
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

        // 2. Sağa-Sola Hareket (Mouse/Dokunmatik girişi)
        if (Input.GetMouseButtonDown(0)) // Tıklamaya başladığın an
        {
            touchStartPos = Input.mousePosition;
            playerStartPos = transform.position;
        }
        else if (Input.GetMouseButton(0)) // Tıklamaya devam ettiğin sürece
        {
            Vector3 currentTouchPos = Input.mousePosition;
            float diffX = (currentTouchPos.x - touchStartPos.x) / Screen.width; // Ekran genişliğine oranla
            
            Vector3 newPos = transform.position;
            newPos.x = playerStartPos.x + (diffX * sideSpeed);
            
            // Sınırları belirle (Yolun dışına çıkmasın)
            newPos.x = Mathf.Clamp(newPos.x, -limitX, limitX);
            
            transform.position = newPos;
        }
    }
}