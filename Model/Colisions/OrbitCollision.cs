using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCollision : MonoBehaviour, IPlayerCollision, IPlayerAttraction
{
    private Planet _planet;

    private Coroutine _cor;
    public bool CanAttract { get; set; }
    public Transform AttractionTarget { get; set; }

    private void Awake()
    {
        if (!transform.parent.TryGetComponent(out _planet)) Debug.LogError("OrbitCollision не нашёл компонент Planet");
    }

    private void Start()
    {
        Events.onBonusActivated += StartAttracting;
        Events.onBonusDeactivated += StopAttracting;
    }


    #region IPlayerCollision
    public void ActOnTriggerEnter(GameObject player)
    {
        if (Main.ShipStatesLogic != null)
        {
            Events.onOrbitEntered?.Invoke(_planet);
            CanAttract = false;
            player.GetComponent<PlayerMovementController>().ChangePlanetInfo(_planet);
            player.GetComponent<PlayerMovementController>().Rotator();

            if (Main.ShipStatesLogic.ShipState.IsAroundPlanet() == false)
            {
                Main.ShipStatesLogic.ChangeStates();
                
                JSONTest.UserLocalDataObject.LocalUserStatistics.total_planets += 1;
                //PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
                //Objects.instance.sessionScoreText.text = PlayerPrefs.GetInt("score").ToString();
                
                Debug.LogWarning("Entered planet");
            }
        }
        else
        {
            Debug.LogWarning("Main.ShipStatesLogic == null");
        }
        
    }

    public void ActOnTriggerExit(GameObject player)
    {
        ;
    }
    #endregion

    #region IPlayerAttraction

    public void EnterPlayerAttractionRange(GameObject player)
    {
        CanAttract = true;
        AttractionTarget = player.transform;

        if (BonusManager.instance.BonusIsActive(Bonuses.type.magnet))
        {
            StartAttracting(Bonuses.type.magnet);
        }
    }

    public void ExitPlayerAttractionRange(GameObject player)
    {
        CanAttract = false;
        AttractionTarget = null;

        StopAttracting(Bonuses.type.magnet);

    }

    public void StartAttracting(Bonuses.type bonusType)
    {
        if (CanAttract && bonusType == Bonuses.type.magnet)
        {
            _cor = StartCoroutine(Attract(AttractionTarget));
        }
    }

    public void StopAttracting(Bonuses.type bonusType)
    {
        if (_cor != null) StopCoroutine(_cor);
        _cor = null;
    }

    public IEnumerator Attract(Transform player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        float intensity = _planet.planet.attractionForce;
        while (CanAttract)
        {
            Utils.PullByGravity(rb, transform, intensity);
            //float xPos = player.transform.position.x + (transform.position.x - player.transform.position.x > 0 ? 10f : -10f) * Time.deltaTime;
            //float yPos = player.transform.position.y + (transform.position.y - player.transform.position.y > 0 ? 10f : -10f) * Time.deltaTime;
            //player.transform.position = new Vector2(xPos, yPos);
            yield return new WaitForFixedUpdate();
        }

    }
    #endregion

    public void GetDeleted()
    {
        Debug.Log($"Despawning planet {_planet.planet.Name}");
        Events.onPlanetDeleted?.Invoke();
        PlanetManager.instance.DespawnPlanet(_planet);
    }
}
