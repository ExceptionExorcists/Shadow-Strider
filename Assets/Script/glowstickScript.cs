using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class glowstickScript : MonoBehaviour
{
    public float duration = 8.0f;
    public float flickerTime = 6.0f;
    public float flickerDuration = 0.2f;
    public float flickerLowerIntensity;
    public float flickerHigherIntensity;
    private float counter = 0;
    private Light glowLight;
    private bool isFlickering = false;
    private bool isOff = false;
    private float timer = 0.0f;
    private NavMeshObstacle nvo;
    public bool used;
    // Start is called before the first frame update
    void Start()
    {
        glowLight = transform.GetChild(0).gameObject.GetComponent<Light>();
        nvo = gameObject.GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (used)
        {
            if(!isOff) nvo.enabled = true;

            counter += Time.deltaTime;
            if (counter > flickerTime && !isOff)
            {

                timer += Time.deltaTime;

                if (!isFlickering)
                {
                    if(timer > flickerDuration)
                    {
                        isFlickering = true;
                        timer = 0.0f;
                        glowLight.intensity = flickerLowerIntensity;
                        flickerDuration = Random.Range(0.01f, 0.2f);
                    }
                }
                else
                {
                    if(timer > flickerDuration)
                    {
                        glowLight.intensity = flickerHigherIntensity;
                        isFlickering = false;
                        timer = 0.0f;
                        flickerDuration = Random.Range(0.01f, 0.2f);

                    }
                }
            }
            if(counter > duration && !isOff)
            {
                glowLight.enabled = false;
                isOff = true;
                nvo.enabled = false;
            }
        }
    }
}
