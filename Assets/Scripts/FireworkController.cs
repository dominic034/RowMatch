using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireworkController : MonoBehaviour
{
    [SerializeField] private List<Firework> fireworks;
    [SerializeField] private float minRandom;
    [SerializeField] private float maxRandom;
    [SerializeField] private float speed;
    
    private void Start()
    {
        GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelComplete);
    }

    private void OnLevelComplete(CompleteType type)
    {
        if(type != CompleteType.NewScore)
            return;
        
        foreach (var f in fireworks)
            f.DoMoveY(Random.Range(minRandom, maxRandom),speed);
    }

    [ContextMenu("Fireworks")]
    private void TEST_Fireworks()
    {
        OnLevelComplete(CompleteType.None);
    }
}
