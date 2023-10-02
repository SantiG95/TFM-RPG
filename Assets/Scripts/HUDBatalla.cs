using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBatalla : MonoBehaviour
{
    public Text textoNombre;
    public Slider vidaSlider;

    public void prepararHUD(Unidad unidad)
    {
        textoNombre.text = unidad.nombreUnidad;
        vidaSlider.maxValue = unidad.vidaMax;
        vidaSlider.value = unidad.vidaActual;
    }

    public void cambiarVida(int vida)
    {
        vidaSlider.value = vida;
    }
}
