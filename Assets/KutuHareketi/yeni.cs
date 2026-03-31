using System.Collections.Generic;
using UnityEngine;

public class KarakterKontrol : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float ileriHiz = 5f;
    public float yatayHiz = 10f;
    private float solSinir;
    private float sagSinir;

    private float ilkFarePozisyonuX;
    private float ilkKarakterPozisyonuX;

    [Header("Sınır Objesi")]
    public GameObject yerObjesi; 

    [Header("İstifleme Ayarları")]
    public List<GameObject> istiflenmisKupler = new List<GameObject>();
    private Color karakterRengi;

    [Header("Engel Ayarları")]
    public List<GameObject> Engeller = new List<GameObject>();

    void Start()
    {
        karakterRengi = GetComponent<Renderer>().material.color;
        istiflenmisKupler.Add(this.gameObject);

        if (yerObjesi != null)
        {
            Renderer yerRenderer = yerObjesi.GetComponent<Renderer>();
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
        transform.Translate(Vector3.forward * ileriHiz * Time.deltaTime);
        
        float yatayGirdi = Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            ilkFarePozisyonuX = Input.mousePosition.x;
            ilkKarakterPozisyonuX = transform.position.x;
        }
        
        if (Input.GetMouseButton(0))
        {
            float fareFarki = Input.mousePosition.x - ilkFarePozisyonuX;
            float hareketMiktari = (fareFarki / Screen.width) * (yatayHiz * 2.5f); 
            
            if (Mathf.Abs(fareFarki) > 0.1f)
            {
                Vector3 farePozisyonu = transform.position;
                farePozisyonu.x = ilkKarakterPozisyonuX + hareketMiktari;
                transform.position = farePozisyonu;
            }
        }

        if (!Input.GetMouseButton(0))
        {
            transform.position += new Vector3(yatayGirdi * yatayHiz * Time.deltaTime, 0, 0);
        }

        if (yerObjesi != null)
        {
            Vector3 sinirliPozisyon = transform.position;
            sinirliPozisyon.x = Mathf.Clamp(sinirliPozisyon.x, solSinir, sagSinir);
            transform.position = sinirliPozisyon;
        }
    }

    private void OnTriggerEnter(Collider diger)
    {
        if (diger.CompareTag("Kup"))
        {
            Color digerRenk = diger.GetComponent<Renderer>().material.color;

            if (RenklerAyniMi(karakterRengi, digerRenk))
            {
                KupEkle(diger.gameObject);
            }
            else
            {
                FarkliRenkIleCarpis(diger.gameObject);
            }
        }

        if (Engeller.Contains(diger.gameObject))
        {
            EngelIleCarpis(diger.gameObject);
        }
    }

    void FarkliRenkIleCarpis(GameObject carpilannObje)
    {
        carpilannObje.GetComponent<Collider>().enabled = false;
        Destroy(carpilannObje);

        if (istiflenmisKupler.Count > 1)
        {
            GameObject sonKup = istiflenmisKupler[istiflenmisKupler.Count - 1];
            istiflenmisKupler.Remove(sonKup);
            Destroy(sonKup);
            Debug.Log("Yanlış renk! Bir küp kaybettin.");
        }
        else
        {
            OyunBitti();
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
        Debug.Log("Küp eklendi!");
    }

    void EngelIleCarpis(GameObject engel)
    {
        engel.GetComponent<Collider>().enabled = false;

        if (istiflenmisKupler.Count > 1)
        {
            GameObject sonKup = istiflenmisKupler[istiflenmisKupler.Count - 1];
            istiflenmisKupler.Remove(sonKup);
            Destroy(sonKup);
            Debug.Log("Engele çarptın! Bir küp kaybettin.");
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
        Debug.LogError("OYUN BİTTİ!");
        ileriHiz = 0;
        yatayHiz = 0;
    }
}