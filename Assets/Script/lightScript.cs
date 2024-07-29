using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class lightScript : MonoBehaviour
{

    public float minFlickerTime;
    public float maxFlickerTime;
    public float maxCooldown;
    public float minCooldown;
    private float timer;
    private float counter;
    private float flickerDuration;
    private float cooldown;
    private float newFlickerIntensity;
    public float defaultLightIntensity = 2.5f;
    public float minLightIntensity = 1.0f;
    private Light _light;
    private NavMeshObstacle _nvMo;
    public bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        FlickerPrep();
        _light = GetComponent<Light>();
        _nvMo = GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            _light.enabled = true;
            _nvMo.enabled = true;
            counter += Time.deltaTime;
            if (counter > cooldown)
            {
                timer += Time.deltaTime;
                _light.intensity = newFlickerIntensity;
                if (timer > flickerDuration)
                {
                    _light.intensity = defaultLightIntensity;
                    FlickerPrep();
                }
            }
        }
        else
        {
            _light.enabled = false;
            _nvMo.enabled = false;
        }

    }
    public void FlickerPrep()
    {
        cooldown = Random.Range(minCooldown, maxCooldown);
        flickerDuration = Random.Range(minFlickerTime, maxFlickerTime);
        counter = 0.0f;
        timer = 0.0f;
        newFlickerIntensity = Random.Range(minLightIntensity, defaultLightIntensity);
    }    
}
