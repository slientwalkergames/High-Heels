using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 5f;
    public float xBound = 4.5f; // Yolun genişlik sınırı

    [Header("Topuk Ayarları")]
    public GameObject heelPrefab; // Topuk objesi (Prefab)
    public Transform heelParent;  // Topukların dizileceği yer (Karakterin ayak altı)
    public List<GameObject> stackedHeels = new List<GameObject>();
    public float heelHeight = 0.2f; // Her topuğun yüksekliği

    [Header("Karakter Görseli")]
    public Transform characterModel; // Karakterin ana modeli (Yukarı çıkması için)
    public Transform leftLeg, rightLeg; // Bacak açma için bacak objeleri

    private int diamonds = 0;
    private bool isSplitting = false;

    void Update()
    {
        HandleMovement();
        HandleInput();
    }

    // 1. İLERİ VE SAĞA-SOLA HAREKET SİSTEMİ
    void HandleMovement()
    {
        // Otomatik İleri Hareket
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Sağa Sola Hareket (Mouse veya Dokunmatik)
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            Vector3 newPos = transform.position + new Vector3(mouseX * sideSpeed * Time.deltaTime, 0, 0);
            
            // Yolun dışına çıkmaması için sınırlama
            newPos.x = Mathf.Clamp(newPos.x, -xBound, xBound);
            transform.position = newPos;
        }
    }

    // 2. BACAK AÇMA (SPLIT) MEKANİĞİ
    void HandleInput()
    {
        // Ekrana basılı tutunca bacakları aç (Opsiyonel: Raylara gelince otomatik de yapılabilir)
        if (Input.GetMouseButton(0)) 
        {
            // Buraya ray kontrolü de eklenebilir
            // Bacakları yanlara doğru açma animasyonu/pozisyonu
        }
    }

    // 3. ÇARPIŞMALAR (Topuk Almak, Elmas Toplamak, Engele Çarpmak)
    private void OnTriggerEnter(Collider other)
    {
        // TOPUK TOPLAMA
        if (other.CompareTag("Heel"))
        {
            AddHeel();
            Destroy(other.gameObject);
        }

        // ELMAS TOPLAMA
        if (other.CompareTag("Diamond"))
        {
            diamonds++;
            Debug.Log("Elmas Toplandı! Toplam: " + diamonds);
            Destroy(other.gameObject);
        }

        // ENGELE ÇARPMA (Topuk Azaltma)
        if (other.CompareTag("Obstacle"))
        {
            RemoveHeel();
        }

        // RAYA GELİNCE BACAK AÇMA
        if (other.CompareTag("Rail"))
        {
            StartSplit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rail"))
        {
            EndSplit();
        }
    }

    // TOPUK EKLEME FONKSİYONU
    void AddHeel()
    {
        // Karakteri yukarı kaydır
        characterModel.localPosition += new Vector3(0, heelHeight, 0);

        // Yeni topuk oluştur ve listeye ekle
        Vector3 spawnPos = new Vector3(heelParent.position.x, heelParent.position.y + (stackedHeels.Count * heelHeight), heelParent.position.z);
        GameObject newHeel = Instantiate(heelPrefab, heelParent);
        newHeel.transform.localPosition = new Vector3(0, stackedHeels.Count * heelHeight, 0);
        
        stackedHeels.Add(newHeel);
    }

    // TOPUK AZALTMA FONKSİYONU
    void RemoveHeel()
    {
        if (stackedHeels.Count > 0)
        {
            // En alttaki topuğu sil (Oyundaki mantığa göre)
            GameObject lastHeel = stackedHeels[stackedHeels.Count - 1];
            stackedHeels.Remove(lastHeel);
            Destroy(lastHeel);

            // Karakteri aşağı indir
            characterModel.localPosition -= new Vector3(0, heelHeight, 0);
        }
        else
        {
            // Topuk bittiyse oyunu kaybetme mantığı buraya gelir
            Debug.Log("Game Over!");
        }
    }

    // BACAK AÇMA EFEKTİ
    void StartSplit()
    {
        isSplitting = true;
        // Bacakları dışa doğru döndür veya kaydır
        leftLeg.localRotation = Quaternion.Euler(0, 0, 45); 
        rightLeg.localRotation = Quaternion.Euler(0, 0, -45);
    }

    void EndSplit()
    {
        isSplitting = false;
        // Bacakları normale döndür
        leftLeg.localRotation = Quaternion.identity;
        rightLeg.localRotation = Quaternion.identity;
    }
}