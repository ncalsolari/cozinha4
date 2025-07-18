using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()] como soh precisamos de uma lista dessa, tiramos ele do menu p impedir a criacao de outra e dar xabu
public class RecipeListSO : ScriptableObject {
    public List<RecipeSO> recipeSOList;
}
