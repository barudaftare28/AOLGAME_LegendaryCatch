using UnityEngine;
using System.Collections;

public class FishingSystem : MonoBehaviour
{
    [Header("Dependencies")]
    public player playerScript; // Drag objek Player ke sini di Inspector
    public GameObject alertIcon; // Drag objek "!" ke sini

    [Header("Settings")]
    public string waterTag = "Water";

    [Header("Status")]
    public bool canFish = false;
    public bool isFishing = false;

    void Update()
    {
        if (canFish && Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            StartCoroutine(FishingProcess());
        }
    }

    IEnumerator FishingProcess()
    {
        isFishing = true;
        if (playerScript != null) playerScript.canMove = false; // Player berhenti
        Debug.Log("Mancing dimulai...");

        yield return new WaitForSeconds(Random.Range(2f, 5f));

        alertIcon.SetActive(true); // Muncul tanda "!"

        float timer = 1.5f; // Waktu untuk bereaksi
        bool caught = false;
        while (timer > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                caught = true;
                break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        Debug.Log(caught ? "Dapat Ikan!" : "Lepas...");

        alertIcon.SetActive(false);
        if (playerScript != null) playerScript.canMove = true; // Player bisa jalan lagi
        isFishing = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ini akan print APAPUN yang ditabrak ke konsol
        Debug.Log("Kena sesuatu: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag(waterTag))
        {
            Debug.Log("Masuk Air!");
            canFish = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(waterTag)) canFish = false;
    }


}

