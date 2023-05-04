using System;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;
    private float fitness;

    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i=0; i<layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        InitNeurons();
        InitWeights();
    }

    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i=0; i<copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }

    private void CopyWeights(float[][][] CopyWeights)
    {
        for (int i = 0; i<weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = CopyWeights[i][j][k];
                }
            }
        }
    }

    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for (int i=0; i<layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();
    }

    private void InitWeights()
    {
        List<float[][]> weightList = new List<float[][]>();

        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWightsList = new List<float[]>();

            int neuronsInPreviousLayer = layers[i - 1];

            for (int j=0; j<neurons[i].Length; j++)
            {
                float[] neuronsWeights = new float[neuronsInPreviousLayer];

                for (int k=0; k<neuronsInPreviousLayer; k++)
                {
                    neuronsWeights[k] = UnityEngine.Random.Range(-1f, 1f);
                }
                layerWightsList.Add(neuronsWeights);
            }
            weightList.Add(layerWightsList.ToArray());
        }
        weights = weightList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {
        for (int i=0; i<inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float val = 0f;
                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    val += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(val);
            }
        }
        return neurons[neurons.Length - 1];
    }

    public void Mutate(float puissance)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];
                    
                    float newWeight = UnityEngine.Random.Range(-puissance, puissance);
                    weight += newWeight;
                    
                    weights[i][j][k] = weight;
                }
            }
        }
    }


    public void addFitness(float fit)
    {
        fitness += fit;
    }
    public void SetFitness(float fit)
    {
        fitness = fit;
    }
    public float GetFitness()
    {
        return fitness;
    }

    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }
}
