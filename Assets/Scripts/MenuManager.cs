using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject guidePanel;
    public player playerScript;
    public GameObject settingsPanel;

    // Tambahan variabel untuk mengecek apakah sudah mulai main
    private bool sudahMulaiMain = false;

    // Fungsi untuk memutar volume
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    // Fungsi buka/tutup panel (Alur Menu Utama)
    public void BukaSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void TutupSettings()
    {
        settingsPanel.SetActive(false);
        // Jika belum mulai main, balik ke menu utama. Jika sudah main, balik ke game.
        if (!sudahMulaiMain)
        {
            mainMenuPanel.SetActive(true);
        }
        else
        {
            LanjutGame();
        }
    }

    void Start()
    {
        mainMenuPanel.SetActive(true);
        guidePanel.SetActive(false);
        settingsPanel.SetActive(false); // Pastikan mati di awal

        if (playerScript != null) playerScript.canMove = false;
    }

    // --- FITUR BARU: Deteksi tombol ESC saat main ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Hanya bisa ESC jika sudah lewat menu utama dan panduan
            if (sudahMulaiMain && !guidePanel.activeSelf)
            {
                if (settingsPanel.activeSelf) TutupSettings();
                else BukaSettingsSaatMain();
            }
        }
    }

    public void BukaSettingsSaatMain()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f; // Menghentikan waktu game (ikan berhenti loncat)
        if (playerScript != null) playerScript.canMove = false;
    }

    public void LanjutGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // Jalankan waktu lagi
        if (playerScript != null) playerScript.canMove = true;
    }
    // -----------------------------------------------

    public void TekanPlay()
    {
        mainMenuPanel.SetActive(false);
        guidePanel.SetActive(true);
    }

    public void TekanMulai()
    {
        guidePanel.SetActive(false);
        sudahMulaiMain = true; // Tandai bahwa pemain sudah mulai bermain
        if (playerScript != null) playerScript.canMove = true;
    }

    public void TutupPanduan()
    {
        guidePanel.SetActive(false);
        sudahMulaiMain = true;
        if (playerScript != null) playerScript.canMove = true;
    }

    // ===============================================
    // UPDATE TAMBAHAN (Sesuai Permintaan)
    // ===============================================

    // Fungsi untuk tombol "Kembali ke Menu Utama" (Dipasang di dalam Settings Panel)
    public void KembaliKeMenuUtama()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        sudahMulaiMain = false; // Reset status permainan
        Time.timeScale = 1f;    // Jalankan waktu lagi agar menu tidak freeze

        if (playerScript != null) playerScript.canMove = false;

        // Catatan: Jika ingin mereset posisi karakter atau skor, 
        // kamu bisa menambahkan logika reset di sini atau menggunakan SceneManager.LoadScene.
    }

    // Fungsi untuk tombol "Quit" (Dipasang di Main Menu Panel)
    public void KeluarGame()
    {
        Debug.Log("Game Keluar..."); // Muncul di console untuk tes
        Application.Quit();         // Keluar dari aplikasi
    }
}