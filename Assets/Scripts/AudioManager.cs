using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip background;
    public AudioClip splash;

    public AudioClip fishCaught;

    public AudioClip fishOff;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void playSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
