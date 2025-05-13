
using System;
using System.Collections.Generic;
using NaughtyAttributes;

public class ProcedureComponent : IGameComponent
{
    
    [ReadOnly]
    public Procedure now;
    void Start()
    {
        ChangeProcedure(new LaunchProcedure());
    }

    public void ChangeProcedure(Procedure targetProcedure)
    {
        if (now != null)
        {
            now.OnExit();
        }
        
        targetProcedure.OnEnter();
        now = targetProcedure;
        
    }

    public void ChangeScene(string changeSceneName, Procedure newProcedure)
    {
        GameEntry.GetGameComponent<DataComponent>().SetData("TargetChangeSceneName",changeSceneName);
        GameEntry.GetGameComponent<DataComponent>().SetData("TargetChangeSceneProcedure",newProcedure);
        ChangeProcedure(new ChangeSceneProcedure());
        
    }

    private void Update()
    {
        now?.OnUpdate();
    }
}
