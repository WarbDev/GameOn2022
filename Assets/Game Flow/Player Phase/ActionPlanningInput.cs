using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ActionPlanningInput : MonoBehaviour
{
    public event Action<ActOption> SelectedAction;
    public event Action PlanningCancelled;

}
