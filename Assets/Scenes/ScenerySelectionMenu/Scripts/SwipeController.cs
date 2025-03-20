using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    int currentPage;
    Vector3 targetPos;
    [SerializeField] int maxPage;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    float dragThreshould;

    private void Awake()
    {
        currentPage = 1;

        // Kiểm tra xem levelPagesRect có bị null không để tránh lỗi
        if (levelPagesRect == null)
        {
            Debug.LogError("Trường 'levelPagesRect' chưa được gán trong Inspector.");
            return;
        }

        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Math.Abs(eventData.position.x - eventData.pressPosition.x) >= dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x)
                Previous();
            else
                Next();
        }
        else
        {
            MovePage();
        }
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        // Sử dụng LeanTween đúng cách với gameObject
        LeanTween.moveLocal(levelPagesRect.gameObject, targetPos, tweenTime).setEase(tweenType);
    }

}
