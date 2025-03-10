using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{
    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;
    public bool m_Loop = true; // Variable para controlar si la animación se loopea
    public bool m_PlayOnStart = true; // Variable para controlar si la animación se reproduce al iniciar

    private int m_IndexSprite;
    public Coroutine m_CorotineAnim;
    bool IsDone;

    private void Start()
    {
        if (m_Image == null)
            m_Image = GetComponent<Image>();
        
        if (m_PlayOnStart)
        {
            Func_PlayUIAnim();
        }
    }

    private void OnEnable()
    {
        if (m_PlayOnStart && m_SpriteArray.Length > 0)
        {
            Func_PlayUIAnim();
        }
    }

    private void OnDisable()
    {
        Func_StopUIAnim();
    }

    public void Func_PlayUIAnim()
    {
        IsDone = false;
        if (m_CorotineAnim != null)
        {
            StopCoroutine(m_CorotineAnim);
        }
        m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = true;
        if (m_CorotineAnim != null)
        {
            StopCoroutine(m_CorotineAnim);
            m_CorotineAnim = null;
        }
    }

    public void Func_ResetUIAnim()
    {
        Func_StopUIAnim(); // Detener la animación actual
        m_IndexSprite = 0; // Reiniciar el índice del sprite a 0
        m_Image.sprite = m_SpriteArray[m_IndexSprite]; // Actualizar la imagen al primer sprite
        IsDone = false; // Reiniciar el estado de la animación
    }

    public void SetSprites(Sprite[] sprites)
    {
        m_SpriteArray = sprites;
        Func_ResetUIAnim();
    }

    public void SetSpriteAtIndex(int index)
    {
        if (index >= 0 && index < m_SpriteArray.Length)
        {
            m_IndexSprite = index;
            m_Image.sprite = m_SpriteArray[m_IndexSprite];
        }
        else
        {
            Debug.LogWarning("Index out of range");
        }
    }
    
    IEnumerator Func_PlayAnimUI()
    {
        while (!IsDone)
        {
            yield return new WaitForSeconds(m_Speed);
            if (m_IndexSprite >= m_SpriteArray.Length)
            {
                if (m_Loop)
                {
                    m_IndexSprite = 0; // Reiniciar el índice si se loopea
                }
                else
                {
                    IsDone = true; // Detener la animación si no se loopea
                    yield break;
                }
            }
            m_Image.sprite = m_SpriteArray[m_IndexSprite];
            m_IndexSprite += 1;
        }
    }
}