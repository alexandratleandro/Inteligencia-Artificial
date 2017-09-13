using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ExampleIndividual : Individual {

	private float MinX;
	private float MaxX;
	private float MinY;
	private float MaxY;

	public ExampleIndividual(ProblemInfo info) : base(info) {

		MinX = info.startPointX;
		MaxX = info.endPointX;
		MaxY = info.startPointY > info.endPointY ? info.startPointY : info.endPointY;

		MinY = MaxY - 2 * (Mathf.Abs (info.startPointY - info.endPointY));
	}

	public override void Initialize(int representacao) {
		RandomInitialization(representacao);
	}

	public override void Mutate(float probability, int representacao) { 
		
		NewValueMutation (probability,representacao);
	}    

	public override void Crossover(Individual partner, float probability,int representacao) {		
		if(info.recombinacao==1){
			HalfCrossover (partner, probability,representacao);
		}
		else if(info.recombinacao==2){
			npontoCrossover (partner, probability,info.N,representacao);
			//npontoCrossover (partner, probability);
		}
		else{
			pontoCrossover (partner, probability,representacao);
		}
	}

	public override void CalcTrackPoints() {
		//the representation used in the example individual is a list of trackpoints, no need to convert
	}

	public override void CalcFitness() {
		fitness = eval.time; //in this case we only consider time
	}

	public override Individual Clone() {
		ExampleIndividual newobj = (ExampleIndividual)this.MemberwiseClone ();
		newobj.fitness = 0f;
		newobj.trackPoints = new Dictionary<float,float> (this.trackPoints);
		return newobj;
	}

	void RandomInitialization(int representacao) {

		//*********************INICIAR --REP PROF************************+//
		if(representacao==1){

			float step = (info.endPointX - info.startPointX ) / (info.numTrackPoints - 1);
			float y = 0;

			trackPoints.Add (info.startPointX, info.startPointY);//startpoint
			for(int i = 1; i < info.numTrackPoints - 1; i++) {
				y = UnityEngine.Random.Range(MinY, MaxY);
				trackPoints.Add(info.startPointX + i * step, y);
			}
			trackPoints.Add (info.endPointX, info.endPointY); //endpoint

		}

		//**************INICIAR REP X E Y***********************/

		else{
			float step = (info.endPointX - info.startPointX ) / (info.numTrackPoints - 1);
			float y = 0;
			float x=info.startPointX;
			
			

			trackPoints.Add (info.startPointX, info.startPointY);//startpoint
			for(int i = 1; i < info.numTrackPoints - 1; i++) {
				List<float> keys = new List<float>(trackPoints.Keys);
				while(keys.Contains(x)){
					x = UnityEngine.Random.Range(MinX, MaxX); //meter a ser maior??
				}
				y = UnityEngine.Random.Range(MinY, MaxY);
				

				trackPoints.Add(x, y);
			
				
			}
			trackPoints.Add (info.endPointX, info.endPointY); //endpoint

		}

			
	}

	// mutação se range for menor que probabilidade da mutaçao definida
	void NewValueMutation(float probability, int representacao) {

		//*******************MUTACAO REP PROF*************************+//
		if(representacao==1){
			List<float> keys = new List<float>(trackPoints.Keys);
			foreach (float x in keys) {
				//make sure that the startpoint and the endpoint are not mutated 
				if(Math.Abs (x-info.startPointX)<0.01 || Math.Abs (x-info.endPointX)<0.01) {
					continue;
				}
				// altera trackpoints com valor random
				if(UnityEngine.Random.Range (0f, 1f) < probability) {
					trackPoints[x] = UnityEngine.Random.Range(MinY,MaxY);
				}
			}

		}

		//*******************+MUTACAO REP X E Y***********************//

		else{ 
			
			int index=0;

			float x1=info.startPointX;
			float comeco=info.startPointX;
			float y1;

			List<float> keys = new List<float>(trackPoints.Keys); //correr atual
			int aux=keys.Count;

			for(int i=0;i<aux;i++) {

				List<float> keysA = new List<float>(trackPoints.Keys); //atualizada
				
				//make sure that the startpoint and the endpoint are not mutated 
				if(Math.Abs (keys[i]-info.startPointX)<0.01 || Math.Abs (keys[i]-info.endPointX)<0.01) {
					continue;
				}


				// altera trackpoints com valor random
				if(UnityEngine.Random.Range (0f, 1f) < probability) {

					
					while(keysA.Contains(x1)){
						if(i<aux-1){
							x1=UnityEngine.Random.Range(comeco,keys[i+1]);
						}

						else{
							x1=UnityEngine.Random.Range(comeco,MaxX);
						}
						
					}


					y1=UnityEngine.Random.Range(MinY,MaxY);
					

					trackPoints.Remove(keys[i]);
	    			trackPoints.Add(x1,y1);
	    			keysA.Add(x1);
	    			index= keysA.IndexOf(keys[i]);
	    			keysA.RemoveAt(index);  						
				} 
				comeco=keys[i]; 

			} 
		}
		
	}


	// recombinação de acordo com probabilidade definida
	void HalfCrossover(Individual partner, float probability,int representacao) { 

		if (UnityEngine.Random.Range (0f, 1f) > probability) {
				return;
		}

		if(representacao==1){ 
			//this example always splits the chromosome in half
			int crossoverPoint = Mathf.FloorToInt (info.numTrackPoints / 2f);
			List<float> keys = new List<float>(trackPoints.Keys);
			for (int i=0; i<crossoverPoint; i++) {
				float tmp = trackPoints[keys[i]];
				trackPoints[keys[i]] = partner.trackPoints[keys[i]];
				partner.trackPoints[keys[i]]=tmp;
			}

		}
		else{

			//this example always splits the chromosome in half
			int crossoverPoint = Mathf.FloorToInt (info.numTrackPoints / 2f);
			List<float> keys = new List<float>(trackPoints.Keys);
			List<float> keysP = new List<float>(partner.trackPoints.Keys);
			

			for (int i=0; i<crossoverPoint; i++) {
				float tmp = trackPoints[keys[i]];
				float tempK= keys[i];
				if(!keys.Contains(keysP[i]) && !keysP.Contains(keys[i])){
					trackPoints.Remove(keys[i]);
					trackPoints.Add(keysP[i],partner.trackPoints[keysP[i]]);
					partner.trackPoints.Remove(keysP[i]);

					partner.trackPoints.Add(tempK,tmp);

				}
				
			} 

		}
		
	}

	/*****************************RECOMBINAÇAO COM N PONTOS***********************/
	void npontoCrossover(Individual partner, float probability, int n, int representacao) { //n nao esta a ser utilizado pq nao sei
		int troca=1; // flag para verificar se trocamos ou nao; primeiro n se troca

		if (UnityEngine.Random.Range (0f, 1f) > probability) {
				return;
		}

		if(representacao==1){ 
		
			List<float> keys = new List<float>(trackPoints.Keys);	
			List<int> npts = new List<int>(n);   

			// criacao de n pontos de corte distintos
			for(int j=0; j<n; j++){
				int pt = UnityEngine.Random.Range(1,info.numTrackPoints -1);
				while (npts.Contains(pt)) {
					pt = UnityEngine.Random.Range(1,info.numTrackPoints -1);
				} 
				npts.Add(pt);
			}	
			npts.Sort(); // ordenar pontos de corte

			for (int i=0; i<n-1;i++){ // para cada ponto de corte vamos trocar ou nao até ao ponto de corte seguinte
				if(troca==1){ // trocamos genes entre ponto atual e seguinte
					for(int h = npts[i];h<npts[i+1]; h++){					
						float tmp = trackPoints[keys[h]];
						trackPoints[keys[h]] = partner.trackPoints[keys[h]];
						partner.trackPoints[keys[h]]=tmp;
						troca=0; // alteramos para nao trocar 
					}
				} 
				else{
					troca =1;				
				}
			} 	 
		}

		else{

			List<float> keys = new List<float>(trackPoints.Keys);	
			List<float> keysP = new List<float>(partner.trackPoints.Keys);	
			List<int> npts = new List<int>(n);   

			// criacao de n pontos de corte distintos
			for(int j=0; j<n; j++){
				int pt = UnityEngine.Random.Range(0,info.numTrackPoints );
				while (npts.Contains(pt)) {
					pt = UnityEngine.Random.Range(0,info.numTrackPoints );
				} 
				npts.Add(pt);
			}	
			npts.Sort(); // ordenar pontos de corte

			for (int i=0; i<n-1;i++){ // para cada ponto de corte vamos trocar ou nao até ao ponto de corte seguinte
				if(troca==1){ // trocamos genes entre ponto atual e seguinte
					for(int h = npts[i];h<npts[i+1]; h++){					
						float tmp = trackPoints[keys[h]];
						float tempK= keys[h];
						if(!keys.Contains(keysP[h]) && !keysP.Contains(keys[h])){
							trackPoints.Remove(keys[i]);
							trackPoints.Add(keysP[i],partner.trackPoints[keysP[i]]);
							partner.trackPoints.Remove(keysP[i]);

							partner.trackPoints.Add(tempK,tmp);

				        }
						
					}

					troca=0; // alteramos para nao trocar 
				} 
				else{
					troca =1;				
				}
			} 	
		} 
	}
	

	//*************************PONTO RANDOM CROSSOVER**************************//
	void pontoCrossover(Individual partner, float probability,int representacao) { 

		if (UnityEngine.Random.Range (0f, 1f) > probability) {
				return;
		}

		if(representacao==1){

			int n=0;
		
			List<float> keys = new List<float>(trackPoints.Keys); 
			
			//dois pontos de corte aleatorios 
			n=UnityEngine.Random.Range(0,info.numTrackPoints ); 
			
			for (int i=n; i<info.numTrackPoints; i++) {
				float tmp = trackPoints[keys[i]];
				trackPoints[keys[i]] = partner.trackPoints[keys[i]];
				partner.trackPoints[keys[i]]=tmp;
			} 

		}

		else{

			int n=0;		
			List<float> keys = new List<float>(trackPoints.Keys);
			List<float> keysP = new List<float>(partner.trackPoints.Keys);			
			//dois pontos de corte aleatorios 
			n=UnityEngine.Random.Range(0,info.numTrackPoints );		
			
			for (int i=n; i<info.numTrackPoints; i++) {
				float tmp = trackPoints[keys[i]];
				float tempK= keys[i];
				if(!keys.Contains(keysP[i]) && !keysP.Contains(keys[i])){
					trackPoints.Remove(keys[i]);
					trackPoints.Add(keysP[i],partner.trackPoints[keysP[i]]);
					partner.trackPoints.Remove(keysP[i]);

					partner.trackPoints.Add(tempK,tmp);

				}
			} 	

		} 
    } 

}
