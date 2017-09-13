using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SelectionMethod {

	public SelectionMethod() {

	}

	//override on each specific selection class
	public abstract List<Individual> selectIndividuals (int Tselecao, List<Individual> oldpop, int num,  float kTorneio); //num is usually 2 q
	
}
   