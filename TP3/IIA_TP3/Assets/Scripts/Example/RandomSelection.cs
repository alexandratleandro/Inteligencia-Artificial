using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// selecao aleatoria de elementos da populacao atual
public class RandomSelection : SelectionMethod {

	public RandomSelection(): base() {

	}

	public override List<Individual> selectIndividuals (int Tselecao, List<Individual> oldpop, int num, float KTorneio)
	{
		if(Tselecao==1){
			return randomSelection (oldpop, num);
		}
		else if(Tselecao==2){
			return torneioSelection (oldpop, num, KTorneio,2);
		}
		//selecao=3...
		else {
			return roletaSelection (oldpop, num);
		}
	}

	// seleciona num elementos da populacao atual
	List<Individual> randomSelection(List<Individual>oldpop, int num) { 
		List<Individual> selectedInds = new List<Individual> ();
		int popsize = oldpop.Count;
		for (int i = 0; i<num; i++) {
			//make sure selected individuals are different
			Individual ind = oldpop [Random.Range (0, popsize)];
			while (selectedInds.Contains(ind)) {
				ind = oldpop [Random.Range (0, popsize)];
			}
			selectedInds.Add (ind.Clone()); //we return copys of the selected individuals
		}
		return selectedInds;
	}

	// seleção roleta
	// proabilidadede individuo ser selecionado depende da porçao de roleta que lhe é atribuida consoante a sua qualidade
	List<Individual> roletaSelection(List<Individual>oldpop, int num) {
		List<Individual> selectedInds = new List<Individual> ();
		//List<float> probPop = new List<float> (oldpop.Count); // tamanho da lista de probabilidades
		int popsize = oldpop.Count;
		float [] probPop = new float[popsize];
		
		float sum=0; // somatorio fitness de todos os individuos da população
		float sumProbs=0; // soma probabilidades todos individuos
		float prob=0; 

		// soma dos valores de todos os indivíduos da população
		for (int i = 0; i<popsize; i++) { 
			sum+=1/(oldpop[i].fitness);
		}
		// calcular probabilidade/porçao de cada individuo de acordo com formula
		for (int i = 0; i<popsize; i++) { 
			prob=sumProbs+ 1/(oldpop[i].fitness)/sum;
			probPop[i]=prob;
			sumProbs+=prob;
		}

        Individual ind=oldpop[0]; //so inicializar
		for (int i = 0; i<num; i++) {			
			float numero = UnityEngine.Random.Range (0, 1);
			for (int j = 0; j<popsize; j++) {	 
				// de acordo com probabilidade random devolver individuo que se encontra nessa proçao				
				if(probPop[j]<numero && probPop[j+1]>numero){
					ind=oldpop[j];
					break;
				}	 					
			}			
			selectedInds.Add (ind.Clone()); //we return copys of the selected individuals
		} 
		selectedInds.Add (ind.Clone());
		return selectedInds; 
	}

	//*******************SELECAO POR TORNEIO*********************//
	List<Individual> torneioSelection(List<Individual>oldpop, int num, float k,int n) {
		int aux=0;
		float avaliar=0f;
		int index=0;
		List<Individual> selectedInds2 = new List<Individual> ();


		for(int j=0; j<num;j++){
			aux=0;
			index=0; // guarda indice do melhor ou pior individuo dentro de torneio
			avaliar=0f; // valor de fitness guardado como melhor/pior

	        //escolher aleatoriamente
			List<Individual> selectedInds = randomSelection(oldpop, n) ;  		        

			float r = Random.Range (0.5f, 1f);  //valor de r aleatorio entre 0 e 1  

	        // torneio entre os individuos pre-selecionados (2 ou mais)
			if(r<k){ // escolhe-se o melhor individuo dentro do torneio
				for(int i =0; i< selectedInds.Count;i++) {
								
				    if(aux==0){
					   avaliar=selectedInds[i].fitness;
					   index=i;
					   aux++;
					}
					else{
						// determinamos individuo com menor fitness == melhor individuo
						if(selectedInds[i].fitness<avaliar){ //melhor individuo--> menos tempo--> menos fitness
							avaliar=selectedInds[i].fitness;
							index=i;					
						}
					}
				}
			}
			else{ // escolhe-se o pior individuo dentro do torneio
				for(int i =0; i< selectedInds.Count;i++) {
								
				    if(aux==0){
					   avaliar=selectedInds[i].fitness;
					   index=i;
					    aux++;
					}
					else{
						if(selectedInds[i].fitness>avaliar){ //pior individuo--> mais tempo
							avaliar=selectedInds[i].fitness;
							index=i;					
						}
					}
				}
			}
			selectedInds2.Add(selectedInds[index].Clone());
			//restaurar selectedInds 	        
	        for(int i =0; i< selectedInds.Count;i++) {	        	
	        	selectedInds.RemoveAt(i);	        	
	        } 
	    }	
		return selectedInds2;
		//todos devolvidos à populaçao??
	}
 
}   
