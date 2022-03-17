
using UnityEngine;
public class Movement : MonoBehaviour
{

    [Header("Network Information")]
    // Neural network for car
    private bool initialized = false;
    public bool alive = true;
    private NeuralNetwork net;

    /*
    //Old Network
     public NN network;
     //public GA ga;
     public int layers = 1;
     public int neurons = 10;
     */


    [Header("Agent Movement")]
    public float speed, jumpForce;
    public float hitDistance;
    public LayerMask layerMaskGround;
    public LayerMask layerMaskWall;
    public float distance, tempDistance;
    public Transform tip;
    Rigidbody rb;
    public bool isJumped, isGrounded;
    float x, z;
    float move;
    bool jump;


    [Header("Fitness")]
    public float overallFitness;
    public float distanceMultiplier = 1.5f;
    public float collisionMultiplier = -5f;
    public float sensorDistanceMultiplier = 1.5f;




    [Header("State")]
    public float totalDistanceTravelled;
    public float timeSinceStarted;
    Vector3 lastPosition, startPosition;
    //private bool startPoint = false;

    void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody>();
        tempDistance = 0;
        //startPosition = transform.position;
        //network = GetComponent<NN>();
        //ga = GetComponent<GA>();

    }

    public void Reset()
    {
        timeSinceStarted = 0f;
        totalDistanceTravelled = 0f;
        overallFitness = 0f;
        lastPosition = startPosition;
        transform.position = startPosition;
        // network.Initialize(layers, neurons);
    }


    void Update()
    {

        /*
        x = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumped = true;
            z = .6f;
        }
        else
        {
            isJumped = false;
            z = 0f;
        }
        */

    }
    void FixedUpdate()
    {
        timeSinceStarted += Time.deltaTime;
        Debug.Log(initialized + " " + alive);
        if (initialized && alive)
        {

            Debug.Log("Loop is working");

            Detections();
            //timeSinceStarted += Time.deltaTime;
            float[] inputs = { distance };
            float[] output = net.FeedForward(inputs);

            //(x, z) = network.RunNN(distance);
            z = output[2];
            if (z >= .5f) isJumped = true;
            else isJumped = false;


            if (output[0] < 0) output[0] = 0;
            if (isJumped && output[0] > 0) net.AddFitness(30);

            MoveJump(output[0], output[1], isJumped);
            totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);

            // net.AddFitness(Vector3.Distance(transform.position, lastPosition));
            CalculateFitness();
            lastPosition = transform.position;

        }
        //CalculateFitness();
    }

    void MoveJump(float x, float z, bool jumped)
    {

        isGrounded = Physics.Raycast(tip.transform.position, Vector3.down, hitDistance, layerMaskGround);
        Debug.DrawRay(tip.transform.position, Vector3.down, Color.red, hitDistance);
        //Move forward;
        transform.Translate(x, 0, z);
        //transform.LookAt(transform.position);

        Vector3 dir = new Vector3(0, 20f, 0);
        //Jump upward; 
        if (isGrounded && jumped)
        {
            //net.AddFitness(10);
            rb.AddForce(dir * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }

        /*  if (!isGrounded)
            {
            net.AddFitness(Vector3.Distance(lastPosition, transform.position) * 10);
            }
        */
    }

    void Detections()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitInfo, Mathf.Infinity, layerMaskWall);

        if (hitInfo.collider != null)
        {
            float distanceOut = (hitInfo.distance) / Mathf.Ceil(distance);
            distance = distanceOut; //Feed this value to the network as input; 


            Debug.Log(distance + " " + distanceOut);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * distance, Color.red, distance);
            tempDistance = distance;
        }
        else distance = tempDistance;
    }

    void Death()
    {
        timeSinceStarted = 0;
        totalDistanceTravelled = 0;
        alive = false;
        // startPoint = true;
        net.AddFitness(-5);
        // Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Death();
            //net.AddFitness(5);
            //Death();
        }
        else if (other.gameObject.CompareTag("Side Wall"))
        {
            net.AddFitness(-20);
            Death();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Check Points"))
        {
            net.AddFitness(5);
        }
    }

    public void Init(NeuralNetwork net)
    {
        this.net = net;
        initialized = true;
    }



    void CalculateFitness()
    {
        totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);
        overallFitness = totalDistanceTravelled * distanceMultiplier;// + distance * sensorDistanceMultiplier;
        net.AddFitness(overallFitness * 10);
        /*
                if (timeSinceStarted > 5000 && overallFitness < 40)
                {
                    Death();
                }
                if (overallFitness > 1000)
                {
                    Death();
                }
                */
    }
    /*
        public void ResetWithNN(NN net)
        {
            network = net;
            Reset();
        }

        void Death()
        {
            GameObject.FindObjectOfType<GA>().Death(overallFitness, network);
        }
    */




}
