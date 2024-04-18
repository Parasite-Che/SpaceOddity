using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipStates
{
    public abstract void Handle(ShipStatesLogic logic, Main _main);

    public abstract void Moving(Main _main);

    public abstract bool IsAroundPlanet();

}

class MovingBetweenPlanets : ShipStates
{
    public override void Handle(ShipStatesLogic logic, Main _main)
    {
        logic.ShipState = new MovingAroundPlanet();
        _main.StopSpirallingIntoPlanet();
    }

    public override void Moving(Main _main)
    {
        _main.MovingBetweenPlanet();
    }

    public override bool IsAroundPlanet()
    {
        return false;
    }

}

class MovingAroundPlanet : ShipStates
{
    public override void Handle(ShipStatesLogic logic, Main _main)
    {
        logic.ShipState = new MovingBetweenPlanets();
        _main.ShipUncouplingJump();
        _main.StopSpirallingIntoPlanet();
    }

    public override void Moving(Main _main)
    {
        _main.MovingAroundPlanet();
    }

    public override bool IsAroundPlanet()
    {
        return true;
    }
}

public class ShipStatesLogic
{
    public ShipStates ShipState { get; set; }
    private Main _main;

    public ShipStatesLogic(ShipStates state, Main Main)
    {
        this.ShipState = state;
        this._main = Main;
    }

    public void ChangeStates()
    {
        this.ShipState.Handle(this, _main);
    }

    public void Moving()
    {
        this.ShipState.Moving(_main);
    }
}

