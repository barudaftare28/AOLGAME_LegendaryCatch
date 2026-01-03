using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public Transform player;
    public float checkDistance = 3f;

    private SpriteRenderer sr;
    private float sqrCheckDistance; // Untuk optimasi

    void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        sr = GetComponentInChildren<SpriteRenderer>();

        // Menghitung kuadrat jarak sekali saja di awal
        sqrCheckDistance = checkDistance * checkDistance;
    }

    void Update()
    {
        if (player == null || sr == null) return;

        // Menggunakan sqrMagnitude lebih ringan daripada Vector3.Distance
        float sqrDistance = (transform.position - player.position).sqrMagnitude;

        if (sqrDistance <= sqrCheckDistance)
        {
            if (!dialogueBox.activeSelf) dialogueBox.SetActive(true);

            // Logika Menoleh
            bool playerDiKiri = player.position.x < transform.position.x;
            sr.flipX = playerDiKiri;
        }
        else
        {
            if (dialogueBox.activeSelf) dialogueBox.SetActive(false);
        }
    }
}