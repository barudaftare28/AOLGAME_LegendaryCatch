using UnityEngine;

public class NPCMapWalker : MonoBehaviour
{
    public float speed = 2f;
    public float leftBoundary = -10f; // Batas kiri map
    public float rightBoundary = 10f; // Batas kanan map

    private int direction = 1;

    void Update()
    {
        // Gerak maju sesuai arah
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Cek apakah sudah mentok batas kanan
        if (transform.position.x >= rightBoundary)
        {
            direction = -1;
            FlipCharacter();
        }
        // Cek apakah sudah mentok batas kiri
        else if (transform.position.x <= leftBoundary)
        {
            direction = 1;
            FlipCharacter();
        }
    }

    void FlipCharacter()
    {
        // Opsional: Biar NPC-nya muter balik arah muka
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
         transform.localScale = newScale; // Aktifkan jika sprite 2D
    }
}