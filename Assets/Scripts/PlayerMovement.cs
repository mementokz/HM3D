using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    Animator player_animation;

    private bool started = false;
    private bool fight = false;

    private int enemyKilled = 0;

    RaycastHit hit;

    public Text text;

   /* [SerializeField]
    private GameObject bulletPrefab;*/

    public ParticleSystem particleSystem;
    void Start()
    {
        player_animation = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Start player running animation
        player_animation.SetInteger("ANIM_ID", 1);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Finish")
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
        }

    }
    void Update()
    {
        if (Input.GetMouseButton(0) && !started)
        {
            text.enabled = false;
            started = true;
            GotoNextPoint();
        }
            // Choose the next destination point when the agent gets
            // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && started)
        {
            // Debug.Log("Stopped");
            fight = true;
            player_animation.SetInteger("ANIM_ID", 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (Physics.Raycast(ray, out raycastHit, 100f))
                {
                    Debug.Log(raycastHit.transform.gameObject.name);
                    if (raycastHit.transform != null && fight)
                    {
                        CurrentClickedGameObject(raycastHit.transform.gameObject);
                    }
                }
            }
        }

    }
    public void CurrentClickedGameObject(GameObject gameObject)
    {
        if (gameObject.tag == "Enemy")
        {

            Debug.Log("enemy cliked");
            // Player looking to target before playing animation
            //var newPos = new Vector3(hit.point.x, 2.2f, hit.point.z);
            //transform.LookAt(newPos);

            // Player's shooting animation
            player_animation.SetInteger("ANIM_ID", 2);

            Shoot();

            // Enemy amount on the map
            enemyKilled++;

            // Enemy died animation & make enemies unclicable
            gameObject.GetComponent<Animator>().SetInteger("Died", 1);
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject.GetComponent<BoxCollider>());



            // Create coroutine for normalize player's direction
            StartCoroutine(ExampleCoroutine());

            if (enemyKilled % 3 == 0 )
            {
                GotoNextPoint();
            }
        }
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1);
        //transform.rotation = new Quaternion();
    }


    public void Shoot()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, out hit))
        {
                //Instantiate(bulletPrefab, hit.point, Quaternion.LookRotation(hit.normal));

                Vector3 direction = hit.point - transform.position;
                //transform.rotation = Quaternion.LookRotation(direction);

                // Setup our Particle System
                particleSystem.transform.position = transform.position;
                particleSystem.transform.rotation = Quaternion.LookRotation(direction);

                // Play Particle System
                particleSystem.Play();
        }
    }
}


