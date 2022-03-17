
/*
using System;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

using Random = UnityEngine.Random;


public class GA : MonoBehaviour
{
    [Header("References")]
    public Movement movement; //GetComponent at start
    private NN[] population;

    public GameObject prefab;
    GameObject[] Playerprefabs;

    [Header("Controls")]
    public int populationSize = 100;
    public int fitnessMultiplier = 10;
    [Range(0f, 1f)]
    public float mutationRate = 0.06f;
    public int matrixMutationMultiplier = 10;

    [Header("Crossover Controls")]
    public int bestAgentCount = 10;
    public int worstAgentCount = 2;
    public int crossoverCount;
    public int crossoverRandomizationCount = 100;


    //Genepool; Holds * information
    List<int> genePool = new List<int>();
    int naturallySelected;




    [Header("View Info")]
    public int currentGeneration;
    public int currentGenome = 0;



    void Start()
    {
        Populate();
    }

    void Populate()
    {
        population = new NN[populationSize];
        PopulateWithRandomValues(population, 0);
        ResetToCurrentGenome();
    }

    void PopulateWithRandomValues(NN[] newPopulation, int startingIndex)
    {
        while (startingIndex < populationSize)
        {
            newPopulation[startingIndex] = new NN();
            newPopulation[startingIndex].Initialize(movement.layers, movement.neurons);
            startingIndex++;
        }
    }

    void ResetToCurrentGenome()
    {
        movement.ResetWithNN(population[currentGenome]); //Create a methon with name ResetWithNN in movement script 
    }

    public void Death(float fitness, NN network)
    {
        if (currentGenome < population.Length - 1)
        {
            population[currentGenome].fitness = fitness;
            currentGenome++;
            ResetToCurrentGenome();
        }
        else
        {
            Repopulate();
        }
    }

    void Repopulate()
    {
        genePool.Clear();
        currentGeneration++;
        naturallySelected = 0;
        SortPopulation();
        Populate();

        NN[] newPopulation = PickBestPopulation();
        Crossover(newPopulation);
        Mutate(newPopulation);
        PopulateWithRandomValues(newPopulation, naturallySelected);
        population = newPopulation;
        currentGenome = 0;
        ResetToCurrentGenome();
    }

    NN[] PickBestPopulation()
    {
        NN[] newPopulation = new NN[populationSize];

        for (int i = 0; i < bestAgentCount; i++)
        {
            newPopulation[naturallySelected] = population[i].InitializeCopy(movement.layers, movement.neurons);
            newPopulation[naturallySelected].fitness = 0;
            int temporaryFitNess = Mathf.RoundToInt(population[i].fitness * fitnessMultiplier);
            for (int j = 0; j < temporaryFitNess; j++)
            {
                genePool.Add(i);
            }

        }

        for (int i = 0; i < worstAgentCount; i++)
        {
            int indexFromLast = population.Length - 1;
            int temporaryFitNess = Mathf.RoundToInt(population[indexFromLast].fitness * fitnessMultiplier);
            for (int j = 0; j < temporaryFitNess; j++)
            {
                genePool.Add(indexFromLast);
            }
        }
        return newPopulation;
    }

    void Crossover(NN[] newPopulation)
    {
        for (int i = 0; i < crossoverCount; i += 2)
        {
            int childOneIndex = i;
            int childTwoIndex = i + 1;

            if (genePool.Count >= 1)
            {
                for (int j = 0; j < crossoverRandomizationCount; j++)
                {
                    childOneIndex = genePool[Random.Range(0, genePool.Count)];
                    childTwoIndex = genePool[Random.Range(0, genePool.Count)];
                    if (childOneIndex != childTwoIndex) break;
                }
            }

            NN childOne = new NN();
            NN childTwo = new NN();

            childOne.Initialize(movement.layers, movement.neurons);
            childOne.Initialize(movement.layers, movement.neurons);
            childOne.fitness = 0;
            childTwo.fitness = 0;

            //Weights
            for (int w = 0; w < childOne.weights.Count; w++)
            {
                if (Random.Range(0f, 1f) < .05)
                {
                    childOne.weights[w] = population[childOneIndex].weights[w];
                    childTwo.weights[w] = population[childTwoIndex].weights[w];
                }
                else
                {
                    childOne.weights[w] = population[childTwoIndex].weights[w];
                    childTwo.weights[w] = population[childOneIndex].weights[w];
                }


            }
            //Biases

            for (int b = 0; b < childOne.biases.Count; b++)
            {
                if (Random.Range(0f, 1f) < .05)
                {
                    childOne.biases[b] = population[childOneIndex].biases[b];
                    childTwo.biases[b] = population[childTwoIndex].biases[b];
                }
                else
                {
                    childOne.biases[b] = population[childTwoIndex].biases[b];
                    childTwo.biases[b] = population[childOneIndex].biases[b];
                }
            }

            newPopulation[naturallySelected] = childOne;
            naturallySelected++;
            newPopulation[naturallySelected] = childTwo;
            naturallySelected++;


        }
    }

    void Mutate(NN[] newPopulation)
    {
        for (int i = 0; i < naturallySelected; i++)
        {
            for (int j = 0; j < newPopulation[i].weights.Count; j++)
            {
                newPopulation[i].weights[j] = MutateMatrix(newPopulation[i].weights[j]);
            }
        }
    }

    Matrix<float> MutateMatrix(Matrix<float> matrix)
    {
        int randomPoints = Random.Range(1, (matrix.RowCount * matrix.ColumnCount) / matrixMutationMultiplier);
        Matrix<float> tempMatrix = matrix;

        for (int i = 0; i < randomPoints; i++)
        {
            int randomizeColumn = Random.Range(0, tempMatrix.ColumnCount);
            int randomizeRow = Random.Range(0, tempMatrix.RowCount);
            tempMatrix[randomizeRow, randomizeColumn] = Mathf.Clamp((tempMatrix[randomizeRow, randomizeColumn] + Random.Range(-1f, 1f)), -1f, 1f);
        }
        return tempMatrix;
    }

    void SortPopulation()
    {
        for (int i = 0; i < population.Length; i++)
        {
            for (int j = 0; j < population.Length; j++)
            {
                if (population[i].fitness < population[j].fitness)
                {
                    NN temp = population[i];
                    population[i] = population[j];
                    population[j] = temp;
                }
            }

        }
    }

}
*/
