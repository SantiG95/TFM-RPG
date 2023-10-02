using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad : MonoBehaviour
{
    public string nombreUnidad;
    public int nivelUnidad;

    public int ataque;

    public int vidaMax;
    public int vidaActual;

    public bool recibirDaño(int daño)
    {
        vidaActual -= daño;

        if(vidaActual <= 0)
        {
            vidaActual = 0;
            return true;
        }
        return false;
    }

    public void recuperarVida(int recuperacion)
    {
        vidaActual += recuperacion;
        if (vidaActual > vidaMax) vidaActual = vidaMax;
    }
}
