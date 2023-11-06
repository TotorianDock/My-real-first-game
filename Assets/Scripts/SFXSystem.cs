using System.Collections;
using UnityEngine;

public class SFXSystem : MonoBehaviour
{
    public AudioClip[] sounds;
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioSource _audioSourceSound;
    public static float MainVolume = 1f;

    public void PlaySound(AudioClip clip, float volume = 0.4f)
    {
        _audioSourceSound.loop = false;
        _audioSourceSound.clip = clip;
        _audioSourceSound.volume = MainVolume * volume;
        _audioSourceSound.Play();
    }
    public void PlayMusic(AudioClip clip, float volume = 0.4f)
    {
        _audioSourceMusic.Stop();
        _audioSourceMusic.loop = true;
        _audioSourceMusic.clip = clip;
        _audioSourceMusic.volume = MainVolume * volume;
        _audioSourceMusic.Play();
    }
    public void ChangeVolume(float volume = 1f)
    {
        volume /= 100;
        MainVolume = volume;

        _audioSourceSound.volume = volume;
        _audioSourceMusic.volume = volume * 0.17f;
    }
    public void PauseAll()
    {
        _audioSourceSound.Pause();
        _audioSourceMusic.Pause();
    }
    public void UnPauseAll()
    {
        _audioSourceSound.UnPause();
        _audioSourceMusic.UnPause();
    }
    public IEnumerator SoundErrors()
    {
        while (true)
        {
            _audioSourceMusic.Pause();
            _audioSourceSound.Pause();
            yield return new WaitForSeconds(0.1f);
            _audioSourceMusic.UnPause();
            _audioSourceSound.UnPause();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
