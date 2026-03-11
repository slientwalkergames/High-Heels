using UnityEngine;
using System.Collections.Generic;

public class KarakterKontrol : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float ileriHiz = 5f;
    public float yatayHiz = 10f;
    public float yolSiniri = 4.5f;

    [Header("İstifleme Ayarları")]
    public List<GameObject> istiflenmisKupler = new List<GameObject>();
    private Color karakterRengi;

    [Header("Engel Ayarları")]
    // Buraya Unity içinden 7 adet engel küpünü (cube1, cube2 vb.) sürükleyip bırakabilirsin
    public List<GameObject> Engeller = new List<GameObject>();

    void Start()
    {
        karakterRengi = GetComponent<Renderer>().material.color;
        
        // Karakterin kendisini listenin ilk elemanı yapalım
        istiflenmisKupler.Add(this.gameObject);
    }

    void Update()
    {
        HareketEt();
    }

    void HareketEt()
    {
        // 1. Sürekli İleri Hareket
        transform.Translate(Vector3.forward * ileriHiz * Time.deltaTime);

        // 2. Sağa-Sola Hareket
        float yatayGirdi = Input.GetAxis("Horizontal");
        Vector3 yeniPozisyon = transform.position + new Vector3(yatayGirdi * yatayHiz * Time.deltaTime, 0, 0);

        yeniPozisyon.x = Mathf.Clamp(yeniPozisyon.x, -yolSiniri, yolSiniri);
        transform.position = yeniPozisyon;
    }

    private void OnTriggerEnter(Collider diger)
    {
        // --- KÜP TOPLAMA MANTIĞI ---
        if (diger.CompareTag("Kup"))
        {
            Color digerRenk = diger.GetComponent<Renderer>().material.color;

            if (RenklerAyniMi(karakterRengi, digerRenk))
            {
                KupEkle(diger.gameObject);
            }
            else
            {
                OyunBitti();
            }
        }

        // --- ENGEL MANTIĞI ---
        // Eğer çarptığımız obje "Engeller" listesinin içindeyse
        if (Engeller.Contains(diger.gameObject))
        {
            EngelIleCarpis(diger.gameObject);
        }
    }

    void KupEkle(GameObject yeniKup)
    {
        yeniKup.GetComponent<Collider>().enabled = false;
        yeniKup.transform.SetParent(this.transform);

        float yeniYukseklik = istiflenmisKupler.Count; 
        yeniKup.transform.localPosition = new Vector3(0, yeniYukseklik, 0);
        yeniKup.transform.localRotation = Quaternion.identity;

        istiflenmisKupler.Add(yeniKup);
        Debug.Log("Küp eklendi! Toplam: " + istiflenmisKupler.Count);
    }

    void EngelIleCarpis(GameObject engel)
    {
        // Engele bir kez çarpmak için engel collider'ını kapatabiliriz
        engel.GetComponent<Collider>().enabled = false;

        if (istiflenmisKupler.Count > 1)
        {
            // En üstteki (en son eklenen) küpü bul
            GameObject sonKup = istiflenmisKupler[istiflenmisKupler.Count - 1];
            
            // Listeden çıkar
            istiflenmisKupler.Remove(sonKup);
            
            // Sahneden sil (veya arkada bırakabilirsin)
            Destroy(sonKup);
            
            Debug.Log("Engele çarptın! Bir küp kaybettin. Kalan: " + (istiflenmisKupler.Count - 1));
        }
        else
        {
            // Eğer yanında hiç küp yoksa ve sadece kendisi kalmışsa oyun biter
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
        Debug.LogError("OYUN BİTTİ!");
        ileriHiz = 0;
        yatayHiz = 0;
    }
}