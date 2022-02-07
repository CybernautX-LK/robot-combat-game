using UnityEngine;

public class EffectController : MonoBehaviour
{
	[SerializeField] float duration;
	[SerializeField] float timer;

    //Unity Events
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration) Destroy(gameObject);
    }
}