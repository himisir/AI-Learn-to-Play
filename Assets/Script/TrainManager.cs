using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using TMPro;

public class TrainManager : MonoBehaviour
{ // Start is called before the first frame update
    public GameObject individual;
    private bool isTraning = false;
    public int populationSize;  // Population size
    private int generationNumber = 0;
    public float timerTime = 10;
    [Range(1f, 10f)]
    //public 
    float timeScale = 1;
    private int[] layers = new int[] { 1, 30, 30, 3 };
    // Network description
    public List<NeuralNetwork> nets;
    public List<Movement> movementList = null;
    private int count = 0;
    public bool flag = false;

    public TextMeshProUGUI txt;

    void Start()
    {
        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        while (true)
        {
            if (flag)
            {
                Debug.Log("Timer");
                isTraning = false;
                flag = false;
                //yield return new WaitForSeconds(timerTime);
            }
            yield return new WaitForSeconds(timerTime);

        }
    }

    // Timer to kill individuals
    /*
    void Timer()
    {
        isTraning = false;
    }
    */
    // Loops infinite times
    void FixedUpdate()
    {
        //Time.timeScale = timeScale;
        // If training or atleast one is alive
        if (isTraning == false || count == 0)
        {
            if (generationNumber == 0)
            {
                // Initialize
                InitNeuralNetworks();
            }
            else
            {
                // Sort nets based on the fitness
                nets.Sort();
                // Save best performing network
                string path = Application.dataPath + "/Models/Player" + generationNumber.ToString() + ".txt";
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "");
                }

                for (int i = 0; i < nets[populationSize - 1].weights.Length; i++)
                {
                    for (int j = 0; j < nets[populationSize - 1].weights[i].Length; j++)
                    {
                        for (int k = 0; k < nets[populationSize - 1].weights[i][j].Length; k++)
                        {
                            if (k == nets[populationSize - 1].weights[i][j].Length - 1)
                                File.AppendAllText(path, nets[populationSize - 1].weights[i][j][k].ToString());
                            else
                                File.AppendAllText(path, nets[populationSize - 1].weights[i][j][k].ToString() + ",");
                        }
                        File.AppendAllText(path, "\n");
                    }
                    File.AppendAllText(path, ";\n");
                }


                // Mutate half of the population with lower score
                for (int i = 0; i < populationSize / 2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                    nets[i].Mutate();
                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                }

            }

            // Increment genration count
            generationNumber++;
            txt.text = "Generation number: " + generationNumber.ToString();
            isTraning = true;
            flag = true;
            // Reset timer
            //Invoke("Timer", timerTime);
            // Initiate individuals
            CreateCarBodies();
            //flag = false;
        }
        // Get count of alive individuals
        int tempcount = 0;
        foreach (Movement temp in movementList)
        {
            if (temp.alive == true)
            {
                tempcount++;
            }
        }
        count = tempcount;
        // If follow car set camera coordinates behind car

    }

    // Create individuals
    private void CreateCarBodies()
    {
        // Destroy all instances of individuals if list is not empty
        if (movementList != null)
        {
            for (int i = 0; i < movementList.Count; i++)
            {
                GameObject.Destroy(movementList[i].gameObject);
            }

        }
        // Create list of neural networks
        movementList = new List<Movement>();
        for (int i = 0; i < populationSize; i++)
        {
            Movement boomer = (((GameObject)Instantiate(individual, individual.transform.position, individual.transform.rotation)).transform.GetComponent("Movement") as Movement);


            boomer.Init(nets[i]);

            movementList.Add(boomer);
        }
        count = populationSize;
    }


    // Initialize neural networks
    void InitNeuralNetworks()
    {
        // Population must be even, incase it is not
        if (populationSize % 2 != 0)
        {
            populationSize = 10;
        }
        // Create list of neural networks
        nets = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }

}


