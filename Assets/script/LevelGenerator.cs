using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Bina Ayarları")]
    public GameObject[] buildingPrefabs;
    public Transform player; // Karakterini buraya sürükle
    public int numberOfBuildings = 50;
    public float spawnDistance = 20f; // Binalar arası mesafe
    public float platformWidth = 6f;  // Yolun kenarından uzaklık

    [Header("Anlık Düzenleme")]
    public float xRotation = 0f;
    public bool generateNow = false;

    private void OnValidate()
    {
        if (generateNow)
        {
            GenerateCity();
            generateNow = false;
        }
    }

    public void GenerateCity()
    {
        if (player == null) return;

        // Eski grubu sil
        Transform oldGroup = transform.Find("CityGroup");
        if (oldGroup != null) DestroyImmediate(oldGroup.gameObject);

        GameObject cityGroup = new GameObject("CityGroup");
        cityGroup.transform.SetParent(this.transform);

        // Karakterin şu anki Z pozisyonundan başla
        float startZ = player.position.z + 10f; 

        for (int i = 0; i < numberOfBuildings; i++)
        {
            GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            // Rastgele X (sağ veya sol)
            float side = (Random.value > 0.5f) ? platformWidth : -platformWidth;
            // Rastgele Z (karakterin ilerisinde)
            float zPos = startZ + (i * spawnDistance) + Random.Range(-5f, 5f);

            Vector3 spawnPos = new Vector3(side, 0, zPos);
            Quaternion rotation = Quaternion.Euler(xRotation, Random.Range(0f, 360f), 0);

            GameObject b = Instantiate(prefab, spawnPos, rotation, cityGroup.transform);
            b.transform.localScale = Vector3.one * Random.Range(0.8f, 1.5f);
        }
    }
}