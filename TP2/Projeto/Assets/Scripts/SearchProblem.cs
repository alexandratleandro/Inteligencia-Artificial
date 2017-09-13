using UnityEngine;
using System.Collections;


public struct Successor
{
	public object state;
	public float cost;
	public Action action;


	public Successor(object state, float cost, Action a)
	{
		this.state = state;
		this.cost = cost;
		this.action = a;
	}
}


public interface ISearchProblem
{
	object GetStartState ();
	bool IsGoal (object state);
	int difCaixa (object state);  
	
	float crateMaxDist (object state);
	float crateTotalDist (object state);
	float M_goalsDistMax (object state);
	float E_goalsDistMax (object state);
	float goalsTotalDist (object state);
	float crateMaxGoalsMed (object state);
	float M_crateGoalDist (object state);
	float E_crateGoalDist (object state);
	float remainingMaxcrates (object state);

	Successor[] GetSuccessors (object state);

	int GetVisited ();
	int GetExpanded ();
}
