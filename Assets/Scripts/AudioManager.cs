using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlayFlip() => audioSource.PlayOneShot(flipSound);
    public void PlayMatch() => audioSource.PlayOneShot(matchSound);
    public void PlayMismatch() => audioSource.PlayOneShot(mismatchSound);
}
