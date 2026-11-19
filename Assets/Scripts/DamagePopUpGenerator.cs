using System.Globalization;
using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    public GameObject damagePopUpPrefab;
    public LifeSystem lifeSystem;


    void Awake()
    {
        current = this;
        if(lifeSystem)
        {
            lifeSystem.OnDamage += (currentHealth, newHealth, attackID) =>
            {
                float damage = currentHealth - newHealth;
                if (damage >= 10)
                {
                    int numberPopup = Mathf.FloorToInt(damage / (damage / 10));
                    int damageValue = Mathf.FloorToInt(damage / numberPopup);
                    for (int i = 0; i < numberPopup; i++)
                    {
                        Vector3 Randomness = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-.5f, .5f));
                        CreateDamagePopUp(transform.position + Randomness, damageValue.ToString(), new Vector4(Randomness.x, 0, 0, 1));
                    }
                }
                else { 
                    Vector3 Randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
                    CreateDamagePopUp(transform.position + Randomness, damage.ToString(), new Vector4(Randomness.x, 0, 0, 1));
                }
            };
        }
    }


    
    
    public void CreateDamagePopUp( Vector3 position, string text, Color color)
    {
        Debug.Log(text);
        var popup = Instantiate(damagePopUpPrefab, position, Quaternion.identity);
        var temp  = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        Destroy(popup, 1f);


    }

}
