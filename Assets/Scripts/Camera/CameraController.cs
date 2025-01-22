using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public bool isPlayMode = true; // 控制是否进入播放模式

    public Image[] images; // 需要切换的三张图片
    private int currentImageIndex = 0; // 当前显示的图片索引

    public float switchInterval = 0.8f; // 图片切换的时间间隔

    private Coroutine switchCoroutine;

    void Update()
    {
        if (isPlayMode && switchCoroutine == null)
        {
            // 如果进入播放模式且没有正在运行的协程，启动切换协程
            switchCoroutine = StartCoroutine(SwitchImages());
        }
        else if (!isPlayMode && switchCoroutine != null)
        {
            // 如果退出播放模式且有正在运行的协程，停止切换协程
            StopCoroutine(switchCoroutine);
            switchCoroutine = null;
        }
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            // 隐藏所有图片
            foreach (var img in images)
            {
                img.gameObject.SetActive(false);
            }

            // 显示当前图片
            images[currentImageIndex].gameObject.SetActive(true);

            // 更新索引指向下一张图片
            currentImageIndex = (currentImageIndex + 1) % images.Length;

            // 等待设定的时间间隔
            yield return new WaitForSeconds(switchInterval);
        }
    }
}