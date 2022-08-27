using System;
using UnityEngine.EventSystems;

namespace Moonlight
{
    public class Button : UnityEngine.UI.Button
    {
        public Action<PointerEventData> OnPointerEnterCB;
        public Action<PointerEventData> OnPointerExitCB;
        public Action<PointerEventData> OnPointerDownCB;
        public Action<PointerEventData> OnPointerUpCB;
        public Action<BaseEventData> OnDeselectCB;
        public Action<BaseEventData> OnSelectCB;
        public Action<BaseEventData> OnSubmitCB;
        protected Action<SelectionState, bool> OnDoStateTransitionCB;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnPointerEnterCB?.Invoke(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            OnPointerExitCB?.Invoke(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnPointerDownCB?.Invoke(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnPointerUpCB?.Invoke(eventData);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            OnSelectCB?.Invoke(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            OnDeselectCB?.Invoke(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            OnSubmitCB?.Invoke(eventData);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            OnDoStateTransitionCB?.Invoke(state, instant);
        }
    }
}
