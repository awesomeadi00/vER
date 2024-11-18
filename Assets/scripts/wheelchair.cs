using UnityEngine;

public class DelayedAppearance : MonoBehaviour
{
    public GameObject model;
    private float delayTime = 10f;
    private float elapsedTime = 0f;

    void Start()
    {
        if (model != null)
        {
            model.SetActive(false);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= delayTime && model != null)
        {
            model.SetActive(true);
        }
    }
}
