using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class firecrackerScript : MonoBehaviour
{
    private bool used = false;
    public float duration = 4;
    private float counter = 0;
    public ParticleSystem particles;

    private bool sparking = false;
    public float sparkDuration;
    public float waitDuration;
    private float timer = 0.0f;
    private Light light;
    private NavMeshObstacle nvMeshObs;
    private AudioSource AS;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        nvMeshObs = GetComponent<NavMeshObstacle>();
        light = transform.GetChild(0).gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (used)
        {

            if (counter < duration)
            {
                nvMeshObs.enabled = true;

                timer += Time.deltaTime;
                if (!sparking)
                {
                    if(timer > waitDuration)
                    {
                        sparking = true;
                        timer = 0.0f;
                        light.enabled = true;
                        AS.pitch = Random.Range(0.5f, 1.5f);
                        AS.Play();
                    }
                }
                else
                {
                    if(timer > sparkDuration)
                    {
                        sparking = false;
                        timer = 0.0f;
                        light.enabled = false;
                    }
                }

                counter += Time.deltaTime;
            }
            else
            {
                light.enabled = false;
                nvMeshObs.enabled = false;
            }
        }
    }

    public void UseFireCracker()
    {
        ParticleSystem sparks = Instantiate(particles, transform);
        sparks.Play();
        used = true;
    }
}
