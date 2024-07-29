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
    private Light _light;
    private NavMeshObstacle _nvMeshObs;
    private AudioSource _as;

    

    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _nvMeshObs = GetComponent<NavMeshObstacle>();
        _light = transform.GetChild(0).gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (used)
        {

            if (counter < duration)
            {
                _nvMeshObs.enabled = true;

                timer += Time.deltaTime;
                if (!sparking)
                {
                    if(timer > waitDuration)
                    {
                        sparking = true;
                        timer = 0.0f;
                        _light.enabled = true;
                        _as.pitch = Random.Range(0.5f, 1.5f);
                        _as.Play();
                        
                        Script.GameManager.ListenerScript.gameObject.GetComponent<ListenerScript>().InvestigateArea(transform.position, gameObject, ListenerScript.NoiseStrength.High);
                    }
                }
                else
                {
                    if(timer > sparkDuration)
                    {
                        sparking = false;
                        timer = 0.0f;
                        _light.enabled = false;
                    }
                }

                counter += Time.deltaTime;
            }
            else
            {
                _light.enabled = false;
                _nvMeshObs.enabled = false;
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
