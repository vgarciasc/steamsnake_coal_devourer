using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuAnimations : MonoBehaviour {

    [Header("Fixed Animations")]
    public List<SpriteRenderer> toParallax;
    public List<Transform> toRotate;
    public float parallaxSpeed;
    public float rotationSpeed;
    public List<Transform> cameraPositions;
    public Transform cart;
    public List<Transform> cartPositions;

    [Header("UI Elements")]
    public MenuGroups currentGroup;
    public List<CanvasGroup> groups;
    public float alphaChangeDuration;
    public float cameraMovementSpeed;
    public float cartMovementSpeed;

    private void Start()
    {
        StartCoroutine(Parallax());
        StartCoroutine(Rotate());
    }
    public IEnumerator Parallax()
    {
        while (true)
        {
            foreach(var sr in toParallax)
            {
                sr.size += Vector2.right * parallaxSpeed * Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator Rotate()
    {
        while (true)
        {
            foreach(var t in toRotate)
            {
                t.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void OpenScreen(int groupId)
    {
        switch (groupId)
        {
            case (int)MenuGroups.MAIN_MENU:
                MoveCamera(0);
                MoveCart(0);
                OpenGenericScreen(groupId);
                break;
            case (int)MenuGroups.MAIN_LOBBY:
                MoveCamera(0);
                MoveCart(1);
                OpenGenericScreen(groupId);
                break;
            case (int)MenuGroups.CREDITS:
                MoveCamera(0);
                MoveCart(0);
                OpenGenericScreen(groupId);
                break;
            case (int)MenuGroups.PLAYER_LOBBY:
                MoveCamera(1);
                OpenGenericScreen(groupId);
                break;
        }
    }
    private void OpenGenericScreen(int groupId)
    {
        groups[(int)currentGroup].DOFade(0, alphaChangeDuration);
        groups[(int)currentGroup].interactable = false;
        currentGroup = (MenuGroups)groupId;
        groups[(int)currentGroup].DOFade(1, alphaChangeDuration);
        groups[(int)currentGroup].interactable = true;
    }
    private void MoveCamera(int i)
    {
        Camera.main.transform.DOMove(cameraPositions[i].position, cameraMovementSpeed);
    }
    private void MoveCart(int i)
    {
        cart.DOMoveX(cartPositions[i].position.x, cartMovementSpeed);
    }
}
public enum MenuGroups
{
    MAIN_MENU, MAIN_LOBBY, CREDITS, PLAYER_LOBBY
}
