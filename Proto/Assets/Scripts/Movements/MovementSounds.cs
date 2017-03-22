using UnityEngine;

public class MovementSounds : MonoBehaviour
{
    [SerializeField]AudioClip[] _FootStepsClips;

    AudioSource _AudioSource;

    protected void Start ()
	{
        _AudioSource = GetComponent<AudioSource>();
    }

    void PlayFootSteps()
    {
        _AudioSource.clip = _FootStepsClips[Random.Range(0, _FootStepsClips.Length)];
        _AudioSource.pitch = Random.Range(0.80f, 1.2f);
        _AudioSource.Play();
    }
}
