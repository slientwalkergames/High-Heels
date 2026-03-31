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
    public Transform heelParent;  // Topukların dizileceği yer
    public List<GameObject> stackedHeels = new List<GameObject>();
    public float heelHeight = 0.2f; // Her topuğun yüksekliği

    [Header("Karakter Görseli")]
    public Transform characterModel; // Karakterin ana modeli
    public Transform leftLeg, rightLeg; // Bacak objeleri

    private int diamonds = 0;
    private bool isSplitting = false;

    void Update()
    {
        HandleMovement();
        HandleInput();
    }

    // 1. HAREKET SİSTEMİ
    void HandleMovement()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            Vector3 newPos = transform.position + new Vector3(mouseX * sideSpeed * Time.deltaTime, 0, 0);
            newPos.x = Mathf.Clamp(newPos.x, -xBound, xBound);
            transform.position = newPos;
        }
    }

    void HandleInput()
    {
        // Gerekirse bacak açma kontrolleri buraya eklenebilir
    }

    // 2. ÇARPIŞMA KONTROLLERİ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heel"))
        {
            AddHeel();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Diamond"))
        {
            diamonds++;
            Debug.Log("Elmas Toplandı! Toplam: " + diamonds);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Obstacle"))
        {
            RemoveHeel();
        }

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

    // 3. FONKSİYONLAR
    void AddHeel()
    {
        characterModel.localPosition += new Vector3(0, heelHeight, 0);

        GameObject newHeel = Instantiate(heelPrefab, heelParent);
        newHeel.transform.localPosition = new Vector3(0, stackedHeels.Count * heelHeight, 0);
        
        stackedHeels.Add(newHeel);
    }

    void RemoveHeel()
    {
        if (stackedHeels.Count > 0)
        {
            GameObject lastHeel = stackedHeels[stackedHeels.Count - 1];
            stackedHeels.Remove(lastHeel);
            Destroy(lastHeel);

            characterModel.localPosition -= new Vector3(0, heelHeight, 0);
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }

    void StartSplit()
    {
        isSplitting = true;
        leftLeg.localRotation = Quaternion.Euler(0, 0, 45); 
        rightLeg.localRotation = Quaternion.Euler(0, 0, -45);
    }

    void EndSplit()
    {
        isSplitting = false;
        leftLeg.localRotation = Quaternion.identity;
        rightLeg.localRotation = Quaternion.identity;
    }
}