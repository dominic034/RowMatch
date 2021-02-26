using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Firework : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private Vector2 initialPosition;

    private void BurstFirework()
    {
        trail.Stop();
        trail.gameObject.SetActive(false);
        particle.Play();
        StartCoroutine(WaitForComplete(particle.main.startLifetime.constant));
    }

    private IEnumerator WaitForComplete(float time)
    {
        yield return new WaitForSeconds(time);
        ResetParticle();
    }

    private void ResetParticle()
    {
        trail.gameObject.SetActive(true);
        particle.Stop();
        transform.localPosition = initialPosition;
    }
    
    public void DoMoveY(float yTarget, float time)
    {
        trail.Play();
        transform.DOLocalMoveY(yTarget, time).OnComplete(BurstFirework);
    }
}
