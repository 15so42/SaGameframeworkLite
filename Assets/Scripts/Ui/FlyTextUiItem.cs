using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlyTextUiItem : MonoBehaviour
{
    public Text text;

    private FlyTextUiForm flyTextUiForm;
    // 记录初始的世界坐标
    private Vector3 startWorldPos;
    // 当前动画过程中计算出的世界坐标
    private Vector3 currentWorldPos;
    // 在 Init 时获取 Camera.main
    private Camera mainCamera;
    

    public void Init(FlyTextUiForm flyTextUiForm, Vector3 worldPos, string msg, float duration = 1f)
    {
        this.flyTextUiForm = flyTextUiForm;
        text.text = msg;
        text.fontSize = flyTextUiForm.textSize;

        // 在初始化时获取 Camera.main
        mainCamera = Camera.main;

        // 记录初始世界坐标，并赋值给当前坐标
        startWorldPos = worldPos;
        currentWorldPos = worldPos;
        // 设置初始的 UI 位置
        transform.position = mainCamera.WorldToScreenPoint(currentWorldPos);

        // 定义跳跃的目标位置，这里只在水平面上偏移
        Vector3 randomOffset = Random.insideUnitSphere * flyTextUiForm.worldJumpRadius;
        // 仅保留水平位移（x 和 z），保持 y 为起始值
        Vector3 targetWorldPos = startWorldPos + new Vector3(randomOffset.x, 0, randomOffset.z);
        // 定义跳跃的最高点高度
        float jumpHeight = flyTextUiForm.worldJumpHeight;

        // 使用 DOTween.To 来生成一个从 0 到 1 的进度变量 tween
        DOTween.To(() => 0f, progress =>
            {
                // 先计算水平上的线性插值位置
                Vector3 linearPos = Vector3.Lerp(startWorldPos, targetWorldPos, progress);
                // 使用一个抛物线公式模拟跳跃效果：进度 0 和 1 时偏移为 0，进度 0.5 时达到最大高度
                float yOffset =  jumpHeight * progress /** (1 - progress)*/;
                // 计算当前的世界位置（叠加跳跃弧线的 y 偏移）
                currentWorldPos = new Vector3(linearPos.x, linearPos.y + yOffset, linearPos.z);
                // 根据当前相机投影转换为屏幕坐标，更新 UI 元素的位置
                transform.position = mainCamera.WorldToScreenPoint(currentWorldPos);
            }, 1f, this.flyTextUiForm.jumpDuration)
            .SetEase(this.flyTextUiForm.ease)
            .OnComplete(() =>
            {
                // 动画完成后通知 FlyTextUiForm 回收或其他处理
                flyTextUiForm.Release(this);
            });
    }
}