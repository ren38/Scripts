using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillbar : MonoBehaviour
{
    [SerializeField]
    private GameObject CooldownVeil;
    [SerializeField]
    private GameObject[] buttons;
    [SerializeField]
    private ObjectActor Player;
    // index, beginning time, duration, veil object
    private List<(int, float, float, GameObject)> veils;
    private List<(int, float, float, GameObject)> toBeDestroyed;

    protected IntObserver newCooldown;
    protected GameObjectObserver death;

    void Start()
    {
        if (this.Player != null)
        {
            newCooldown = gameObject.AddComponent<IntObserver>();
            newCooldown.setupObserver(createCooldown);
            Player.skillFinishSubscribe(newCooldown);

            death = gameObject.AddComponent<GameObjectObserver>();
            death.setupObserver(died);
            Player.deathSubscribe(death);
        }
        veils = new List<(int, float, float, GameObject)>();
    }
    
    private void createCooldown(int index)
    {
        float duration = Player.getCooldownTime(index) - Time.time;
        if (duration > 0.0f)
        {
            GameObject newVeil = Instantiate(CooldownVeil, buttons[index].transform);
            newVeil.transform.SetSiblingIndex(1);
            veils.Add((index, Time.time, duration, newVeil));
        }
    }

    void Update()
    {
        toBeDestroyed = new List<(int, float, float, GameObject)>();
        foreach ((int, float, float, GameObject) veil in veils)
        {
            float progress = (Time.time - veil.Item2)/ veil.Item3;
            if(progress < 1.0f)
            {
                veil.Item4.transform.localScale = new Vector3(1, 1 - progress, 0);
            }
            else
            {
                Destroy(veil.Item4);
                toBeDestroyed.Add(veil);
            }
        }
        foreach(var ele in toBeDestroyed)
        {
            veils.Remove(ele);
        }

    }

    private void died(GameObject o)
    {
        foreach ((int, float, float, GameObject) veil in veils)
        {
            Destroy(veil.Item4);
        }
        veils.Clear();
    }
}
