using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvolutionState : MonoBehaviour {

	///////////////////////////////////////////parameterizaçao aqui//////////////////////////////////////////////////// 	
	public int selecao=1; // 1- random, 2 - torneio,  resto - roleta
	public int elitismo=0; // numero de individuos a manter
	public int recombinacao=1; //1--> prof(ao meio); 2--> n pontos; 3-->ponto random
	public int representacao=1;
	
	public float KTorneio=0.5f; // 0 e 1--> ter em conta que é problema de minimizaçao??--> só para seleçao torneio
	public int NPontos;
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public float startPointX;
	public float startPointY;
	public float endPointX;
	public float endPointY;
	public float g;
	public float startVelocity;
	public int numTrackPoints;
	private ProblemInfo info;


	public int numGenerations; // numero de geraçoes
	public int populationSize; // tamanho de populacao
	public float mutationProbability; // probabilidade mutação
	public float crossoverProbability; // probabilidade recombinação

	private List<Individual> population;
	private SelectionMethod randomSelection;

	private int evaluatedIndividuals;
	private int currentGeneration; 
	public int EvaluationsPerStep;

	private StatisticsLogger stats;
	public string statsFilename;

	private PolygonGenerator drawer;


	bool evolving;
	bool drawing;

	// Use this for initialization
	void Start () {
		info = new ProblemInfo ();
		info.startPointX = startPointX;
		info.startPointY = startPointY;
		info.endPointX = endPointX;
		info.endPointY = endPointY;
		info.g = g;
		info.startVelocity = startVelocity;
		info.numTrackPoints = numTrackPoints; 

		///////////////////////////////////////
		info.recombinacao=recombinacao;
		
		info.N = NPontos;
		//////////////////////////////////////

		randomSelection = new RandomSelection (); //change accordingly

		stats = new StatisticsLogger (statsFilename);

		drawer = new PolygonGenerator ();

		InitPopulation ();
		evaluatedIndividuals = 0;
		currentGeneration = 0;
		evolving = true;
		drawing = false;
	}
	

	void FixedUpdate () {
		if (evolving) {
			EvolStep ();
		} else if(drawing) {

			for(int i =0; i < population.Count; i++){
				population[i].evaluate();
			}

			population.Sort((x, y) => x.fitness.CompareTo(y.fitness));
			drawer.drawCurve(population[0].trackPoints,info);
			drawing=false;
		}
	}

	// passo evolutivo, avaliar cada individuo da geraçao atual antes de reproduçao
	void EvolStep() {
		//do for a given number of generations
		if (currentGeneration < numGenerations) {			
			//if there are individuals to evaluate on the current generation
			int evalsThisStep = EvaluationsPerStep < (populationSize - evaluatedIndividuals) ? EvaluationsPerStep : (populationSize - evaluatedIndividuals);
			for (int ind = evaluatedIndividuals; ind<evaluatedIndividuals+evalsThisStep; ind++) {
				population[ind].evaluate();
			}
			evaluatedIndividuals += evalsThisStep;			
			//if all individuals have been evaluated on the current generation, breed a new population
			if(evaluatedIndividuals==populationSize) {
				stats.PostGenLog(population,currentGeneration);
				// reproduzir populacao
				population = BreedPopulation();
				evaluatedIndividuals=0;
				currentGeneration++;
			}
		// depois de feita a evolucao reproduzir resultado grafico
		} else {
			stats.finalLog();
			evolving=false;
			drawing = true;
			print ("evolution stopped");
		}			
	}

	// inicializa a populacao
	void InitPopulation () {
		population = new List<Individual>();
		while (population.Count<populationSize) {
			// cria individuos de acordo com o exemplo
			ExampleIndividual newind = new ExampleIndividual(info); //change accordingly
			newind.Initialize(representacao);
			population.Add (newind);
		}
	}

	// produz nova geraçao, nova populaçao a partir da geracao atual
	List<Individual> BreedPopulation() {  
		
		List<Individual> newpop = new List<Individual>();

		//*********************************ELITISMO****************************// 

		population.Sort((x, y) => x.fitness.CompareTo(y.fitness));
		for(int j =0; j< elitismo;j++) {
			newpop.Add(population[j]); // adicionar os n melhores individuos da populção
		}
 
        //*********************************************************************************//
		//breed individuals and place them on new population. We'll apply crossover and mutation later 
		while(newpop.Count<populationSize) {
			// seleciona-se 2 individuos aleatoriamente da população
			List<Individual> selectedInds = randomSelection.selectIndividuals(selecao,population,2, KTorneio); //we should propably always select pairs of individuals
			// adicionamos individuos escolhidos random a lista de nova populaçao
			for(int i =0; i< selectedInds.Count;i++) {
				if(newpop.Count<populationSize) {
					newpop.Add(selectedInds[i]); //added individuals are already copys, so we can apply crossover and mutation directly					
				}
				else{ //discard any excess individuals
					selectedInds.RemoveAt(i);	
				}
			}  
			// aplicar recombinaçoes e mutaçoes aos individuos escolhidos de acordo com as probabilidades definidas
			while(selectedInds.Count>1) {					
				selectedInds[0].Crossover(selectedInds[1],crossoverProbability,representacao);
				selectedInds[0].Mutate(mutationProbability,representacao);
				selectedInds[1].Mutate(mutationProbability,representacao);					
				selectedInds.RemoveRange(0,2);
			}
			// se nao tivermos um par selecionado nao se pode fazer recombinaçao portanto usa-se apenas operador de mutaçao
			if(selectedInds.Count==1) {
				selectedInds[0].Mutate(mutationProbability,representacao);
				selectedInds.RemoveAt(0);
			}	
		}
		//devolve-se populacao da nova geraçao, descendencia resultande da seleçao, recombinação e mutaçao da geracao anterior 
		return newpop;
	}

}


