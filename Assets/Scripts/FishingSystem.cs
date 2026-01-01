using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class FishingSystem : MonoBehaviour
{
    [Header("Dependencies")]
    public player playerScript;
    public GameObject AlertIcon;
    public GameObject FishIcon; // Tongkat pancing
    public TextMeshProUGUI uiFishText;

    [SerializeField] InventoryManager _inventoryManager;

    [Header("Visual Effects")]
    public GameObject bubblePrefab;

    [Header("Hold Meter Settings")]
    public GameObject fishingSliderUI;
    public Slider pancingSlider;
    public float baseDifficulty = 1f;
    private float currentDifficulty;

    [Header("Fish Data")]
    public List<FishData> availableFish;

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
        if (canFish && Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            StartCoroutine(FishingRoutine());
        }
    }

    IEnumerator FishingRoutine()
    {
        isFishing = true;
        if (playerScript != null) playerScript.canMove = false;

        // 1. Pilih Ikan Terlebih Dahulu (Agar tahu kesulitannya)
        FishData targetFish = GetRandomFish();
        // Gunakan 'Reaction Time' dari FishData sebagai pengali kesulitan (Ikan Rare lebih berat)
        currentDifficulty = baseDifficulty / (targetFish != null ? targetFish.reactionTime : 1f);

        Vector3 rodOriginalPos = FishIcon.transform.localPosition;

        // 2. Fase Tunggu & Gelembung
        if (bubblePrefab != null)
        {
            Vector3 spawnPos = transform.position + transform.right * 1.5f;
            spawnPos.z = 0;
            currentBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
            ParticleSystem ps = currentBubble.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        yield return new WaitForSeconds(Random.Range(2f, 5f));
        if (currentBubble != null) Destroy(currentBubble);

        // 3. Ikan Menggigit (!)
        AlertIcon.SetActive(true);
        yield return new WaitForSeconds(targetFish != null ? targetFish.reactionTime : 0.8f);
        AlertIcon.SetActive(false);

        // 4. MINI-GAME: HOLD METER
        fishingSliderUI.SetActive(true);
        pancingSlider.value = 0.5f;
        bool caught = false;
        float gameTimer = 5f;

        while (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;

            // Efek Getar Tongkat (Sensasi ditarik ikan)
            FishIcon.transform.localPosition = rodOriginalPos + (Vector3)Random.insideUnitCircle * 0.05f;

            // Bar turun otomatis, lebih cepat jika ikan Rare
            pancingSlider.value -= 0.4f * Time.deltaTime * currentDifficulty;

            if (Input.GetKey(KeyCode.Space))
            {
                pancingSlider.value += 0.8f * Time.deltaTime;
            }
            yield return null;
        }

        fishingSliderUI.SetActive(false);
        FishIcon.transform.localPosition = rodOriginalPos;

        // Cek Keberhasilan (Sesuai visual area hijau di tengah)
        if (pancingSlider.value >= 0.35f && pancingSlider.value <= 0.65f) caught = true;

        // 5. Hasil Tangkapan (Loncat Fisik)
        if (caught && targetFish != null)
        {
            totalFishCaught++;
            UpdateFishUI();

            if (targetFish.fishPrefab != null)
            {
                // Munculkan fisik ikan
                GameObject ikanFisik = Instantiate(targetFish.fishPrefab, transform.position + Vector3.up, Quaternion.identity);

                // Beri efek loncat (Wajib ada Rigidbody2D di Prefab Ikan)
                Rigidbody2D rb = ikanFisik.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(Random.Range(-2f, 2f), 5f);
                }
                Destroy(ikanFisik, 1.5f);
            }
            _inventoryManager.fishAdded(targetFish);
            Debug.Log("Berhasil Dapat: " + targetFish.fishName);
        }
        else
        {
            Debug.Log("Ikan Lepas!");
        }

        if (playerScript != null) playerScript.canMove = true;
        yield return new WaitForSeconds(0.5f);
        isFishing = false;
    }

    FishData GetRandomFish()
    {
        if (availableFish.Count == 0) return null;
        return availableFish[Random.Range(0, availableFish.Count)];
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