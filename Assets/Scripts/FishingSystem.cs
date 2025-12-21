using UnityEngine;
using System.Collections;
using System.Numerics;

using Vector3 = UnityEngine.Vector3;
public class FishingSystem : MonoBehaviour
{
    [Header("Dependencies")]
    public player playerScript; // Drag objek Player ke sini di Inspector
    public GameObject alertIcon; // Drag objek "!" ke sini

    public GameObject _fishIcon; //object ikon
    
    [Header("Variable")]
    public Vector3 offset = new Vector3(0,1.5f,0); //posisi ikon terhadap player 1f diatas pemain 

    [Header("Settings")]
    public string waterTag = "Water";
    public string playerTag = "Player";

    [Header("Status")]
    public bool canFish = false;
    public bool isFishing = false;



    void Update()
    {
        if (canFish && Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            StartCoroutine(FishingProcess());
        }

        if (canFish && _fishIcon != null && playerScript != null && _fishIcon.activeSelf)
        {
            _fishIcon.transform.position = playerScript.transform.position + offset;
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
        //ini sama aja kayak diatas cuman gw bikin pisah takut error
        #region Fish Icon effect
        if (other.CompareTag(waterTag))
        {
            if (_fishIcon != null && playerScript != null)
            {
                _fishIcon.SetActive(true);
                _fishIcon.transform.position = other.transform.position + offset;
                Debug.Log("Fish icon nyala");
            }
        }

        #endregion


    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(waterTag))
        {
            canFish = false;
            _fishIcon.SetActive(false);
        } 
    }


}

