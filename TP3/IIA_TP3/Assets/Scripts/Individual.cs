using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Individual {
	//variaçao entre pontos
	// por o x no genotipo
	// com angulos
	// com binario - sem virgula flutuante
	public Dictionary<float, float> trackPoints;
	protected BrachystochroneProblem problem;
	protected ProblemInfo info;

	public float fitness;
	public FitnessInfo eval;

	public Individual(ProblemInfo inf) {
		info = inf;
		fitness = 0f;
		problem = new BrachystochroneProblem (info);
		trackPoints = new Dictionary<float,float > ();
	} 

	//override on each specific individual class
	public abstract void Initialize (int representacao);
	public abstract void Mutate (float probability,int representacao);
	public abstract void Crossover (Individual partner, float probability,int representacao);
	public abstract void CalcTrackPoints ();
	public abstract void CalcFitness();
	public abstract Individual Clone ();

	public void evaluate() {
		CalcTrackPoints ();
		eval = problem.evaluate (trackPoints);
		CalcFitness ();
	}

	public override string ToString ()
	{
		List<float> result = new List<float> ();
		foreach (KeyValuePair<float, float> point in trackPoints) {
			result.Add (point.Key);
			result.Add (point.Value);
		}
		return "[Individual] track points: [" + string.Join (",", result.ConvertAll<string> (f => f.ToString()).ToArray()) + "]";
	}







}
