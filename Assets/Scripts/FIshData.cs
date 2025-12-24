using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fishing/FishData")]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishIcon; // Masukkan gambar ikanmu di sini
    
    [Header("Difficulty Settings")]
    [Tooltip("Semakin kecil nilainya, semakin susah ditangkap (Rare)")]
    [Range(0f, 100f)] public float spawnChance = 50f; 

    [Tooltip("Waktu dalam detik untuk menekan spasi sebelum lepas")]
    public float reactionTime = 1.5f; // Ikan langka kasih waktu 0.5s - 0.8s biar susah
}