
using UnityEngine;

public class Best_Score_Panel : MonoBehaviour
{
    public AudioSource bestSound;
    public GameObject confetti;
    public void Exit()
    {
        this.gameObject.SetActive(false);
    }

    public void Play_BestScore()
    {
        bestSound.Play();
        confetti.SetActive(true);
    }
}
