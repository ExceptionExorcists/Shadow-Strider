using System;
using Script;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ListenerScript : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    public float huntingSpeed;
    public float defaultSpeed;
    public States _state;
    public float maxRoamingDistance;
    public float huntingRange;
    public float investigateRange;
    public enum States { Waiting, Roaming, Investigating, Hunting }
    
    public enum NoiseStrength { Low, Medium, High }

    private Transform _roamingTowards;
    private Animator animator;

    public AudioClip growlClip;
    public AudioClip screachClip;
    public float clipDuration;
    private float timer;
    private AudioSource AS;
    private bool isPlayingAudio = false;
    private float audioTimer;
    public float auidoCooldown;
    private bool isPlayingHunting;

    public float huntDistanceMargin;

    public bool alarmHunt = false;

    private GameObject damageColliderGO;
    private ListenerScript()
    {
        GameManager.ListenerScript = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        AS = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        _state = States.Waiting;
        agent = GetComponent<NavMeshAgent>();
        damageColliderGO = transform.Find("Damage").gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        //click to direct enemy, for testing
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }*/

        timer += Time.deltaTime;
        if(timer > clipDuration)
        {
            if (!isPlayingAudio)
            {
                AS.pitch = Random.Range(0.8f, 1.2f);
            }
            AS.Play();
            timer = 0;
        }

        switch (_state) {
            case States.Waiting:
                agent.speed = defaultSpeed;
                Vector3 randomDirection = Random.insideUnitSphere * maxRoamingDistance + transform.position;
                NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxRoamingDistance, 1);
                Vector3 finalPosition = hit.position;
                agent.SetDestination(finalPosition);
                _state = States.Roaming;
                damageColliderGO.GetComponent<SphereCollider>().enabled = false;
                break;
                
            case States.Roaming:
                damageColliderGO.GetComponent<SphereCollider>().enabled = false;

                if (agent.velocity.magnitude < 0.15f) _state = States.Waiting;
                break;
            case States.Investigating:
                damageColliderGO.GetComponent<SphereCollider>().enabled = true;
                if (agent.velocity.magnitude < 0.15f) _state = States.Waiting;
                break;
            case States.Hunting:
                damageColliderGO.GetComponent<SphereCollider>().enabled = true;

                agent.SetDestination(target.transform.position);
                PlayAudioClip(screachClip);
                if (Vector3.Distance(agent.transform.position, target.transform.position) < huntDistanceMargin || !isPointReachable(target.transform.position) && !alarmHunt)
                {
                    _state = States.Waiting;
                    animator.SetBool("Hunting", false);
                    PostProcessing.Instance.effectMaterial.SetFloat("_PulseEffect", 0.0f);
                    isPlayingHunting = false;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        audioTimer += Time.deltaTime;
        if(audioTimer > auidoCooldown)
        {
            isPlayingAudio = false;
        }
    }

    public void InvestigateArea(Vector3 position, GameObject huntTarget, NoiseStrength strength) {
        if (_state == States.Hunting) return;

        float radius = Vector3.Distance(transform.position, position);

        switch (strength) {
            case NoiseStrength.Low:
                break;
            case NoiseStrength.Medium:
                if(radius < huntingRange)
                {
                    _state = States.Hunting;
                    EnterHuntingState(position, huntTarget, false);
                }
                else
                {
                    if(radius < investigateRange)
                    {
                        _state = States.Investigating;
                        agent.SetDestination(position);
                        PlayAudioClip(growlClip);
                    }
                }
                break;
            case NoiseStrength.High:
                EnterHuntingState(position, huntTarget, false);                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(strength), strength, null);
        }

    }

    public void EnterHuntingState(Vector3 position, GameObject huntTarget, bool alarm)
    {
        alarmHunt = alarm;
        animator.SetBool("Hunting", true);
           _state = States.Hunting;
        agent.SetDestination(position);
        target = huntTarget;
        agent.speed = huntingSpeed;
        PostProcessing.Instance.effectMaterial.SetFloat("_PulseEffect", 0.5f);
    }

    public void PlayAudioClip(AudioClip ac)
    {
        if(_state == States.Hunting && !isPlayingHunting && isPointReachable(target.transform.position))
        {
            AS.PlayOneShot(ac);
            isPlayingAudio = true;
            isPlayingHunting = true;
            audioTimer = 0.0f;

        }
        else if (!isPlayingAudio)
        {
            AS.PlayOneShot(ac);
            isPlayingAudio = true;
            audioTimer = 0.0f;
        }
    }

    public bool isPointReachable(Vector3 position)
    {
        var path = new NavMeshPath();
        agent.CalculatePath(position, path);
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                return true;
            case NavMeshPathStatus.PathPartial:
                return false;
            case NavMeshPathStatus.PathInvalid:
                return false;
            default:
                return true;
        }
    }
}
