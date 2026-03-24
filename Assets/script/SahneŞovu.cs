using System.Collections.Generic;
using UnityEngine;

public class WomanKontrol : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float ileriHiz = 5f;
    public float yatayHiz = 10f;
    private float solSinir;
    private float sagSinir;

    private float ilkFarePozisyonuX;
    private float ilkWomanPozisyonuX; // Karakter -> Woman

    [Header("Sınır Objesi (gameobject içindeki Cube)")]
    public GameObject yerKup; // Sizin belirttiğiniz Scale X:10 olan zemin

    [Header("İstifleme Ayarları")]
    public List<GameObject> istiflenmisHeeller = new List<GameObject>(); // Kupler -> Heeller
    private Color womanRengi; // KarakterRengi -> womanRengi

    [Header("Engel Listeleri")]
    public List<GameObject> RoundedCube = new List<GameObject>();
    public List<GameObject> RoundedCube1 = new List<GameObject>();
    public List<GameObject> WallObjeleri = new List<GameObject>(); // gameobject içindeki wall'lar için

    void Start()
    {
        // Woman'ın (Ana karakter) rengini al
        womanRengi = GetComponent<Renderer>().material.color;
        
        // Woman nesnesini listeye ekle
        istiflenmisHeeller.Add(this.gameObject);

        // Zemin sınırlarını hesapla
        if (yerKup != null)
        {
            Renderer yerRenderer = yerKup.GetComponent<Renderer>();
            // Karakterin dışarı taşmaması için 0.5f pay bırakıldı
            solSinir = yerRenderer.bounds.min.x + 0.5f;
            sagSinir = yerRenderer.bounds.max.x - 0.5f;
        }
    }

    void Update()
    {
        HareketEt();
    }

    void HareketEt()
    {
        // 1. İleri Hareket
        transform.Translate(Vector3.forward * ileriHiz * Time.deltaTime);

        float yatayGirdi = Input.GetAxis("Horizontal");

        // 2. Fare Sürükleme Mantığı
        if (Input.GetMouseButtonDown(0))
        {
            ilkFarePozisyonuX = Input.mousePosition.x;
            ilkWomanPozisyonuX = transform.position.x;
        }
        
        if (Input.GetMouseButton(0))
        {
            float fareFarki = Input.mousePosition.x - ilkFarePozisyonuX;
            float hareketMiktari = (fareFarki / Screen.width) * (yatayHiz * 2.5f); 
            
            if (Mathf.Abs(fareFarki) > 0.1f)
            {
                Vector3 farePozisyonu = transform.position;
                farePozisyonu.x = ilkWomanPozisyonuX + hareketMiktari;
                transform.position = farePozisyonu;
            }
        }

        // 3. Klavye Mantığı
        if (!Input.GetMouseButton(0))
        {
            transform.position += new Vector3(yatayGirdi * yatayHiz * Time.deltaTime, 0, 0);
        }

        // 4. Sınırlandırma (Scale X 10 olan Cube sınırları)
        if (yerKup != null)
        {
            Vector3 sinirliPozisyon = transform.position;
            sinirliPozisyon.x = Mathf.Clamp(sinirliPozisyon.x, solSinir, sagSinir);
            transform.position = sinirliPozisyon;
        }
    }

    private void OnTriggerEnter(Collider diger)
    {
        // --- HEEL (TOPUK) TOPLAMA VE RENK KONTROLÜ ---
        if (diger.name.ToLower().Contains("heel"))
        {
            Color digerRenk = diger.GetComponent<Renderer>().material.color;

            if (RenklerAyniMi(womanRengi, digerRenk))
            {
                HeelEkle(diger.gameObject);
            }
            else
            {
                FarkliRenkIleCarpis(diger.gameObject);
            }
        }

        // --- ENGEL KONTROLÜ (3 Farklı Liste İçin) ---
        if (RoundedCube.Contains(diger.gameObject) || 
            RoundedCube1.Contains(diger.gameObject) || 
            WallObjeleri.Contains(diger.gameObject))
        {
            EngelIleCarpis(diger.gameObject);
        }
    }

    void FarkliRenkIleCarpis(GameObject carpilanObje)
    {
        carpilanObje.GetComponent<Collider>().enabled = false;
        Destroy(carpilanObje);

        if (istiflenmisHeeller.Count > 1)
        {
            GameObject sonHeel = istiflenmisHeeller[istiflenmisHeeller.Count - 1];
            istiflenmisHeeller.Remove(sonHeel);
            Destroy(sonHeel);
            Debug.Log("Yanlış renkli heel! Bir tane eksildi.");
        }
        else
        {
            OyunBitti();
        }
    }

    void HeelEkle(GameObject yeniHeel)
    {
        yeniHeel.GetComponent<Collider>().enabled = false;
        yeniHeel.transform.SetParent(this.transform);

        float yeniYukseklik = istiflenmisHeeller.Count; 
        yeniHeel.transform.localPosition = new Vector3(0, yeniYukseklik, 0);
        yeniHeel.transform.localRotation = Quaternion.identity;

        istiflenmisHeeller.Add(yeniHeel);
        Debug.Log("Heel eklendi! Toplam: " + (istiflenmisHeeller.Count - 1));
    }

    void EngelIleCarpis(GameObject engel)
    {
        engel.GetComponent<Collider>().enabled = false;

        if (istiflenmisHeeller.Count > 1)
        {
            GameObject sonHeel = istiflenmisHeeller[istiflenmisHeeller.Count - 1];
            istiflenmisHeeller.Remove(sonHeel);
            Destroy(sonHeel);
            Debug.Log("Engele çarptın! Topuk kaybedildi.");
        }
        else
        {
            OyunBitti();
        }
    }

    bool RenklerAyniMi(Color c1, Color c2)
    {
        return Mathf.Abs(c1.r - c2.r) < 0.1f && 
               Mathf.Abs(c1.g - c2.g) < 0.1f && 
               Mathf.Abs(c1.b - c2.b) < 0.1f;
    }

    void OyunBitti()
    {
        Debug.LogError("OYUN BİTTİ! Woman durdu.");
        ileriHiz = 0;
        yatayHiz = 0;
    }
}