using UnityEngine;

public class HighHeelsManager : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 15f;
    public float xBound = 4f;

    [Header("Topuk ve Elmas Ayarları")]
    public Transform womanModel;      // woman objesi
    public Transform pivotLeft;       // Pivot_Left objesi
    public Transform pivotRight;      // Pivot_Right objesi
    public float growthAmount = 0.3f; // Her toplamada uzama miktarı

    private int diamonds = 0;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // 1. İLERİ GİT
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;

        // 2. SAĞA SOLA KONTROL
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * sideSpeed * Time.deltaTime;
            float newX = Mathf.Clamp(transform.position.x + mouseX, -xBound, xBound);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Eğer çarptığımız şey bizim bir parçamızsa (ayağımızdaki ayakkabı gibi) işlem yapma!
        if (other.transform.IsChildOf(transform)) return;

        // TOPUK TOPLAMA
        if (other.CompareTag("Heel"))
        {
            UpdatePlayerHeight(growthAmount);
            Destroy(other.gameObject);
        }

        // ELMAS TOPLAMA
        if (other.CompareTag("Diamond"))
        {
            diamonds++;
            Debug.Log("Toplam Elmas: " + diamonds);
            Destroy(other.gameObject);
        }

        // ENGELE ÇARPMA
        if (other.CompareTag("Obstacle"))
        {
            UpdatePlayerHeight(-growthAmount);
        }
    }

    void UpdatePlayerHeight(float amount)
    {
        // Ayakkabı pivotlarını uzat (Sadece Y ekseninde)
        pivotLeft.localScale += new Vector3(0, amount, 0);
        pivotRight.localScale += new Vector3(0, amount, 0);

        // Ayakkabılar çok küçülürse oyunu bitir
        if (pivotLeft.localScale.y <= 0.1f)
        {
            pivotLeft.localScale = new Vector3(pivotLeft.localScale.x, 0.1f, pivotLeft.localScale.z);
            pivotRight.localScale = new Vector3(pivotRight.localScale.x, 0.1f, pivotRight.localScale.z);
            // İstersen burada Game Over yapabilirsin
        }

        // Kızı ayakkabıların üstünde tutmak için yukarı kaydır
        womanModel.localPosition += new Vector3(0, amount, 0);
    }
}