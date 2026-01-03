using UnityEngine;

public class NPCSquareWalk2D : MonoBehaviour
{
    public float speed = 2f;
    public float sideLength = 3f;

    private Vector3 lastTurnPos;
    private int step = 0;

    void Start()
    {
        lastTurnPos = transform.position;
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;

        // Tentukan arah jalan (Versi 2D)
        switch (step)
        {
            case 0: moveDir = Vector3.right; break; // Kanan
            case 1: moveDir = Vector3.up; break; // Atas (Ganti dari forward)
            case 2: moveDir = Vector3.left; break; // Kiri
            case 3: moveDir = Vector3.down; break; // Bawah (Ganti dari back)
        }

        // Gerakkan NPC
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // Atur arah hadap (Flip Sprite)
        if (moveDir.x > 0) transform.localScale = new Vector3(1, 1, 1); // Hadap kanan
        else if (moveDir.x < 0) transform.localScale = new Vector3(-1, 1, 1); // Hadap kiri

        // Cek jarak untuk belok
        if (Vector3.Distance(lastTurnPos, transform.position) >= sideLength)
        {
            lastTurnPos = transform.position;
            step = (step + 1) % 4;
        }
    }
}