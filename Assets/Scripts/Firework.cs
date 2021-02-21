using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Firework : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Vector2 initialPosition;

    private void BurstFirework()
    {
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
        particle.Stop();
        transform.localPosition = initialPosition;
    }
    
    public void DoMoveY(float yTarget, float time)
    {
        transform.DOLocalMoveY(yTarget, time).OnComplete(BurstFirework);
    }
}
