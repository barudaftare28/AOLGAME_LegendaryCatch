using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // Wajib untuk Slider

public class FishingSystem : MonoBehaviour
{
    [Header("Dependencies")]
    public player playerScript;
    public GameObject AlertIcon;
    public GameObject FishIcon;
    public TextMeshProUGUI uiFishText;

    [Header("Visual Effects")]
    public GameObject bubblePrefab;

    [Header("Fish Data")]
    public List<FishData> availableFish;

    [Header("Hold Meter Settings")]
    public GameObject fishingSliderUI; // Drag objek "Fishing Slider" ke sini
    public Slider pancingSlider;       // Drag komponen Slider-nya ke sini
    public RectTransform sweetSpot;    // Drag objek "SweetSpot" (hijau) ke sini
    public float difficultyScale = 1f; // Pengali kesulitan

    [Header("Settings")]
    public string waterTag = "Water";
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    public int totalFishCaught = 0;
    private bool canFish = false;
    private bool isFishing = false;
    private GameObject currentBubble;

    void Start()
    {
        if (AlertIcon) AlertIcon.SetActive(false);
        if (FishIcon) FishIcon.SetActive(false);
        if (fishingSliderUI) fishingSliderUI.SetActive(false);
        UpdateFishUI();
    }

    void Update()
    {
        if (FishIcon != null && playerScript != null && FishIcon.activeSelf)
        {
            FishIcon.transform.position = playerScript.transform.position + offset;
        }

        if (canFish && Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            StartCoroutine(FishingRoutine());
        }
    }

    IEnumerator FishingRoutine()
    {
        isFishing = true;
        if (playerScript != null) playerScript.canMove = false;
        if (FishIcon != null) FishIcon.SetActive(false);

        // 1. Fase Tunggu & Gelembung
        float waitTime = Random.Range(2f, 5f);
        if (bubblePrefab != null)
        {
            // Munculkan di posisi player + geser ke depan
            Vector3 spawnPos = playerScript.transform.position + playerScript.transform.right * 1.5f;
            spawnPos.z = 0; // Paksa Z ke 0 agar sejajar player

            currentBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);

            // PERINTAH UNTUK MENYALAKAN (karena Play On Awake mati)
            ParticleSystem ps = currentBubble.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }
        yield return new WaitForSeconds(waitTime);
        if (currentBubble != null) Destroy(currentBubble);

        // 2. Ikan Menggigit (!)
        AlertIcon.SetActive(true);
        yield return new WaitForSeconds(0.8f); // Waktu reaksi singkat
        AlertIcon.SetActive(false);

        // 3. TARGET 24-26 DES: LOGIKA HOLD METER
        fishingSliderUI.SetActive(true);
        pancingSlider.value = 0.5f; // Mulai dari tengah
        bool caught = false;
        float gameTimer = 5f; // Durasi mini-game

        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;

            // Bar turun otomatis (diseret ikan)
            pancingSlider.value -= 0.3f * Time.deltaTime * difficultyScale;

            // Bar naik jika Space ditekan
            if (Input.GetKey(KeyCode.Space))
            {
                pancingSlider.value += 0.8f * Time.deltaTime * difficultyScale;
            }

            // Cek apakah bar ada di area SweetSpot (Hijau)
            // Sederhananya: Bar harus di antara 0.4 dan 0.6 (sesuaikan dengan besar kotak hijaumu)
            if (gameTimer <= 0 && pancingSlider.value >= 0.35f && pancingSlider.value <= 0.65f)
            {
                caught = true;
            }
            yield return null;
        }

        fishingSliderUI.SetActive(false);

        // 4. Hasil
        if (caught)
        {
            totalFishCaught++;

            UpdateFishUI();
            //GameObject ikanTertangkap = Instantiate(GetRandomFish().fishPrefab, transform.position + Vector3.up, Quaternion.identity);
            //Destroy(ikanTertangkap, 2f); // Ikan hilang setelah 2 detik (efek masuk tas)
            Debug.Log("Berhasil Menangkap Ikan!");
        }
        else
        {
            Debug.Log("Ikan Lepas!");
        }

        if (playerScript != null) playerScript.canMove = true;
        isFishing = false;
    }

    void UpdateFishUI()
    {
        if (uiFishText != null) uiFishText.text = "Ikan: " + totalFishCaught;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(waterTag))
        {
            canFish = true;
            if (FishIcon != null) FishIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(waterTag))
        {
            canFish = false;
            if (FishIcon != null) FishIcon.SetActive(false);
        }
    }
}