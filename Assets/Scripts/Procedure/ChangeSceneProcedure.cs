using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneProcedure : Procedure
{
    private string targetSceneName;
    private Procedure targetProcedure;
    private AsyncOperation asyncLoad;

    public override void OnEnter()
    {
        // 读取目标场景名称和流程
        targetSceneName = GameEntry.Data.GetData<string>("TargetChangeSceneName", "");
        targetProcedure = GameEntry.Data.GetData<Procedure>("TargetChangeSceneProcedure", new MainMenuProcedure());

        // 通过协程异步加载 Loading 场景，然后加载目标场景
        GameEntry.Procedure.StartCoroutine(LoadLoadingAndTargetScene());
    }

    public override void OnUpdate()
    {
        // 根据需要添加其他逻辑
    }

    IEnumerator LoadLoadingAndTargetScene()
    {
        // 异步加载 Loading 场景（确保场景在切换前完全加载）
        AsyncOperation loadingOp = SceneManager.LoadSceneAsync("Loading");
        yield return loadingOp;  // 等待 Loading 场景加载完成

        // 关闭之前的所有UI（如果需要）
        GameEntry.Ui.CloseAllUiForm();
        //Debug.Log("Loading 场景加载完成");

        // 开始加载目标场景（在 Loading 场景中显示加载进度）
        yield return GameEntry.Procedure.StartCoroutine(LoadTargetSceneAsync());
    }

    IEnumerator LoadTargetSceneAsync()
    {
        const float MIN_LOADING_TIME = 0.5f; // 最小显示 Loading 场景的时间
        float loadingStartTime = Time.realtimeSinceStartup;
        float displayedProgress = 0f; // 用于平滑显示的进度值

        // 异步加载目标场景，禁止自动激活
        asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false;

        // 发送初始进度0%
        TypeEventSystem.Send(new LoadingProgressEvent(0f));
        //Debug.Log("Progress: 0%");

        // 当异步加载进度低于0.9（Unity的加载阈值）或未达到最小加载时间时持续更新进度
        while (asyncLoad.progress < 0.9f || (Time.realtimeSinceStartup - loadingStartTime) < MIN_LOADING_TIME)
        {
            float elapsed = Time.realtimeSinceStartup - loadingStartTime;
            // 根据最小加载时间计算出的最小进度
            float minProgress = Mathf.Clamp01(elapsed / MIN_LOADING_TIME);
            // asyncLoad.progress 最大只有 0.9，此处映射到0~1区间
            float asyncProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            // 取两者中较大的值作为目标进度
            float targetProgress = Mathf.Max(asyncProgress, minProgress);

            // 平滑更新显示进度
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.deltaTime);
            TypeEventSystem.Send(new LoadingProgressEvent(displayedProgress));
            //Debug.Log($"Progress: {displayedProgress * 100}%");

            yield return null;
        }

        // 确保显示100%
        displayedProgress = 1f;
        TypeEventSystem.Send(new LoadingProgressEvent(displayedProgress));
        //Debug.Log("Progress: 100%");

        // 等待额外0.3秒，确保加载动画等播放完成
        yield return new WaitForSeconds(0.3f);

        // 允许激活目标场景
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

        // 切换到目标流程
        GameEntry.Procedure.ChangeProcedure(targetProcedure);
    }

    public override void OnExit()
    {
       
    }
}
