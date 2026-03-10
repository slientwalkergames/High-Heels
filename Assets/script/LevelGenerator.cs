using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Bina Ayarları")]
    public GameObject[] sourceBuildings; // Sahnedeki binaları buraya sürükle
    public int numberOfBuildings = 40;
    public float minZ = 20f;
    public float maxZ = 200f;
    public float minX = 5f;
    public float maxX = 15f;

    [Header("Düzenleme Ayarları")]
    public float xRotation = 0f;
    public float minScale = 0.8f;
    public float maxScale = 1.5f;

    void Start()
    {
        // Oyun başladığı an şehri kur
        GenerateCity();
    }

    public void GenerateCity()
    {
        // 1. Yeni bir grup oluştur
        GameObject cityGroup = new GameObject("CityGroup");
        cityGroup.transform.SetParent(this.transform);

        // 2. Binaları diz
        for (int i = 0; i < numberOfBuildings; i++)
        {
            if (sourceBuildings.Length == 0) return;

            GameObject source = sourceBuildings[Random.Range(0, sourceBuildings.Length)];

            // Rastgele pozisyon
            float zPos = Random.Range(minZ, maxZ);
            float xPos = Random.Range(minX, maxX);
            if (Random.value > 0.5f) xPos = -xPos;

            Vector3 spawnPos = new Vector3(xPos, 0, zPos);
            Quaternion rotation = Quaternion.Euler(xRotation, Random.Range(0f, 360f), 0);

            // Binayı kopyala
            GameObject b = Instantiate(source, spawnPos, rotation, cityGroup.transform);
            
            // Ölçek ve Yükseklik
            b.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
            b.transform.position += new Vector3(0, 0.5f, 0);
        }
    }
}