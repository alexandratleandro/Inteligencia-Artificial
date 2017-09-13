using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 

public class BreadthFirstSearch : SearchAlgorithm {
	
	private Queue<SearchNode> openQueue = new Queue<SearchNode> ();
	private Stack<SearchNode> openStack = new Stack<SearchNode>();
	private List<SearchNode> openList= new List<SearchNode>();
	private int limite=0;

	private HashSet<object> closedSet = new HashSet<object> ();
	private SearchNode inicial; 

	public int limP;
	public int heuristica;

	void Start ()          
	{
		problem = GameObject.Find ("Map").GetComponent<Map> ().GetProblem();
		SearchNode start = new SearchNode (problem.GetStartState (), 0);
		openQueue.Enqueue (start);
		inicial=start;
		openStack.Push(start);
		openList.Add(start);
	}
	 
	protected override void Step()
	{ // pesquisa em largura 
		if (openQueue.Count > 0)
		{	// remove o primeiro elemento da fila - estado atual
			SearchNode cur_node = openQueue.Dequeue();
			closedSet.Add (cur_node.state);

			if (problem.IsGoal (cur_node.state)) {
				// se nó atual for solucao-> sai
				solution = cur_node;
				finished = true;
				running = false;
			} else { 
				// vai buscar sucessores do nó atual
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);
				if(problem.GetVisited()%1000==0){
					Debug.Log(problem.GetVisited().ToString());
				}
				foreach (Successor suc in sucessors) {
					if (!closedSet.Contains (suc.state)) { 
						SearchNode new_node = new SearchNode (suc.state, suc.cost + cur_node.g, suc.action, cur_node);
						openQueue.Enqueue (new_node);
					}
				}
			}
		}
		else
		{
			finished = true;
			running = false;
		}
	}

	protected override void StepProfundidade ()
	{  
		if (openStack.Count > 0)
		{	// remove o primeiro elemento da fila - estado atual
			SearchNode cur_node = openStack.Pop();
			closedSet.Add (cur_node.state);
			if (problem.IsGoal (cur_node.state)) {
				// se nó atual for solucao-> sai
				solution = cur_node; 
				finished = true;
				running = false;
			} else { 	// vai buscar sucessores do nó atual
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);
				if(problem.GetVisited()%1000==0){
					Debug.Log(problem.GetVisited().ToString());
				}
				foreach (Successor suc in sucessors) {
					if (!closedSet.Contains (suc.state)) { 
						SearchNode new_node = new SearchNode (suc.state, suc.cost + cur_node.g, suc.action, cur_node);
						if(new_node.depth<limP){
							openStack.Push(new_node);
						}
					}
				}
			}
		}
		else
		{
			finished = true;
			running = false;
		}
	}

	protected override void StepAprofundamentoProgressivo(){ 
		if (limite==0 && openStack.Count > 0) // inicio do algoritmo
		{// remove o primeiro elemento da fila - estado atual 
			SearchNode cur_node = openStack.Pop();
			closedSet.Add (cur_node.state); 
		}
		if(openStack.Count ==0 && solution==null){ // se stack estiver vazia quer dizer que já visitou todos os nós até ao nivel maximo.
			finished = false;
			running = true;
			openStack.Push(inicial); // reinicializamos o algortimo com a raiz da arvore
			closedSet.Clear();
			limite++; // incrementamos nive maximo
		}
		if(solution==null && openStack.Count > 0){
			SearchNode cur_node = openStack.Pop();
			closedSet.Add (cur_node.state);
			if (problem.IsGoal (cur_node.state)) { 	// se nó atual for solucao-> sai
				solution = cur_node; 
				finished = true;
				running = false;
			} else { 
				// vai buscar sucessores do nó atual
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);
				if(problem.GetVisited()%1000==0){
					Debug.Log(problem.GetVisited().ToString());
				}
				foreach (Successor suc in sucessors) {
					if (!closedSet.Contains (suc.state)) { 
						SearchNode new_node = new SearchNode (suc.state, suc.cost + cur_node.g, suc.cost + cur_node.g, suc.action, cur_node);
						if(new_node.depth<limite){
							openStack.Push(new_node);
						}  
					}
				}
			}
		}
	} 

	protected override void StepSofrega(){ 
		
		if (openList.Count > 0)
		{  
			SearchNode new_node;
			SearchNode cur_node = openList[0];
			openList.RemoveAt(0);
			closedSet.Add (cur_node.state);
			if (problem.IsGoal (cur_node.state)) {
				// se nó atual for solucao-> sai
				solution = cur_node;
				finished = true;
				running = false;  
			} else { 
				// vai buscar sucessores do nó atual
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);
				if(problem.GetVisited()%1000==0){
					Debug.Log(problem.GetVisited().ToString());
				}
				foreach (Successor suc in sucessors) {
					if (!closedSet.Contains (suc.state)) { 
						//SearchNode new_node = new SearchNode (suc.state, problem.goalsDist(suc.state), suc.action, cur_node);
						//SearchNode new_node = new SearchNode (suc.state, problem.goalsDistMax(suc.state), suc.action, cur_node);
						if (heuristica == 1) {
							new_node = new SearchNode (suc.state, problem.crateMaxDist (suc.state), suc.action, cur_node);
						}
						else if (heuristica==2){
						    new_node = new SearchNode (suc.state, problem.crateTotalDist(suc.state), suc.action, cur_node);
								
						}
						else if (heuristica==3){
							new_node = new SearchNode (suc.state, problem.M_goalsDistMax(suc.state), suc.action, cur_node);

						}
						else if (heuristica==4){
							new_node = new SearchNode (suc.state, problem.E_goalsDistMax(suc.state), suc.action, cur_node);
						}
						else if (heuristica==5){
							new_node = new SearchNode (suc.state, problem.goalsTotalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==6){
							new_node = new SearchNode (suc.state, problem.crateMaxGoalsMed (suc.state), suc.action, cur_node);
						}
						else if (heuristica==7){
							new_node = new SearchNode (suc.state, problem.M_crateGoalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==8){
							new_node = new SearchNode (suc.state, problem.E_crateGoalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==9){
							new_node = new SearchNode (suc.state, problem.remainingMaxcrates(suc.state), suc.action, cur_node);
						}
						else{
							new_node = new SearchNode (suc.state, problem.difCaixa(suc.state), suc.action, cur_node);
						}
						ordena(new_node);
					}
				}
			}
		}
		else
		{
			finished = true;
			running = false;  
		}
	}

	protected override void StepA(){ 
		
		if (openList.Count > 0)
		{ 
			//openList.Sort(SortByCost); // ordena lista pelo custo dos nos
			SearchNode new_node;
			SearchNode cur_node = openList[0];
			openList.RemoveAt(0);
			closedSet.Add (cur_node.state);
			if (problem.IsGoal (cur_node.state)) {
				// se nó atual for solucao-> sai
				solution = cur_node;
				finished = true;
				running = false;  
			} else { 
				// vai buscar sucessores do nó atual
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);
				if(problem.GetVisited()%1000==0){
					Debug.Log(problem.GetVisited().ToString());
				}
				foreach (Successor suc in sucessors) { 
					if (!closedSet.Contains (suc.state)) {  
						if (heuristica == 1) {
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.crateMaxDist (suc.state), suc.action, cur_node);
						}
						else if (heuristica==2){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.crateTotalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==3){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.M_goalsDistMax(suc.state), suc.action, cur_node);
						}
						else if (heuristica==4){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.E_goalsDistMax(suc.state), suc.action, cur_node);
						}
						else if (heuristica==5){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.goalsTotalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==6){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.crateMaxGoalsMed (suc.state), suc.action, cur_node);
						}
						else if (heuristica==7){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.M_crateGoalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==8){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.E_crateGoalDist(suc.state), suc.action, cur_node);
						}
						else if (heuristica==9){
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.remainingMaxcrates(suc.state), suc.action, cur_node);
						}
						else{
							new_node = new SearchNode (suc.state,suc.cost + cur_node.g, problem.difCaixa(suc.state), suc.action, cur_node);
						}
						ordena(new_node);
						//openList.Add(new_node);
					}
				}
			}
		}
		else
		{
			finished = true;
			running = false;  
		}
	} 
 	/**/
 	private void ordena(SearchNode novo){
 		int index;  
     	int i=-1;

 		for(index=0; index<openList.Count; index++){  
			if(novo.f < openList[index].f){
				i=index;  
				break;
			}   

 		}         
 		if(i==-1){
 			openList.Add(novo);
 		}     
 		else{  
 			openList.Insert(i,novo);
 		}
 	} 
 
}
