// HotkeyButton.cs (版本 2 - 无需 Animator)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 需要引入此命名空间来模拟点击
using System.Collections;

namespace Ricimi
{
    /// <summary>
    /// 为任何UI按钮添加快捷键功能。
    /// 它会监听指定的快捷键，并在按键时模拟一次完整的点击，
    /// 这会触发按钮的视觉反馈和 onClick 事件。
    /// </summary>
    [RequireComponent(typeof(Button))] // 确保此脚本挂载的游戏对象上一定有Button组件
    public class HotkeyButton : MonoBehaviour
    {
        [Header("快捷键设置")]
        [Tooltip("要监听的键盘按键")]
        public KeyCode hotkey = KeyCode.None;

        // 我们不再需要自定义的 UnityEvent，因为我们将直接使用 Button 自带的 OnClick 事件
        // [Header("自定义事件")]
        // public UnityEvent onHotkeyPressed;

        private Button m_button;
        // CleanButton 实现了按下和抬起的视觉效果，我们最好能获取它
        private IPointerDownHandler m_pointerDownHandler;
        private IPointerUpHandler m_pointerUpHandler;

        void Awake()
        {
            // 获取必要的组件引用
            m_button = GetComponent<Button>();

            // 获取实现了按下/抬起接口的组件，这里就是 CleanButton
            m_pointerDownHandler = GetComponent<IPointerDownHandler>();
            m_pointerUpHandler = GetComponent<IPointerUpHandler>();

            if (m_button == null)
            {
                Debug.LogError("HotkeyButton: 无法在对象上找到 Button 组件。", this);
            }
        }

        void Update()
        {
            // 检查快捷键是否被按下
            if (Input.GetKeyDown(hotkey))
            {
                // 检查按钮是否处于可交互状态
                if (m_button != null && m_button.interactable)
                {
                    // 模拟点击
                    PressButton();
                }
            }
        }

        /// <summary>
        /// 模拟按钮被按下的行为
        /// </summary>
        public void PressButton()
        {
            // 启动一个协程来模拟按下和快速抬起的过程，以展示视觉反馈
            StartCoroutine(SimulateButtonClick());
        }

        private IEnumerator SimulateButtonClick()
        {
            // 1. 模拟按下 (触发视觉反馈)
            if (m_pointerDownHandler != null)
            {
                m_pointerDownHandler.OnPointerDown(new PointerEventData(EventSystem.current));
            }

            // 等待一小段时间，让用户能看到按下的效果
            // 0.1秒是一个比较合适的、能被感知到的最短时间
            yield return new WaitForSeconds(0.1f);

            // 2. 模拟抬起 (恢复视觉状态)
            if (m_pointerUpHandler != null)
            {
                m_pointerUpHandler.OnPointerUp(new PointerEventData(EventSystem.current));
            }

            // 3. 触发按钮的核心功能 (调用 OnClick 事件)
            // 这一步是执行您在 Inspector 中为按钮配置的所有事件
            m_button.onClick.Invoke();
        }
    }
}