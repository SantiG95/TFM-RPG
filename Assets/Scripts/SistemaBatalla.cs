using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EstadoBatalla { INICIO, TURNOJUGADOR, TURNOENEMIGO, VICTORIA, DERROTA }

public class SistemaBatalla : MonoBehaviour
{
    public GameObject prefabJugador;
    public GameObject prefabEnemigo;

    public Transform posicionJugador;
    public Transform posicionEnemigo;

    Unidad unidadJugador;
    Unidad unidadEnemigo;

    public Text textoDialogo;

    public HUDBatalla HUDJugador;
    public HUDBatalla HUDEnemigo;

    public Button HUDAtaque;
    public Button HUDCurar;

    public EstadoBatalla estado;
    // Start is called before the first frame update
    void Start()
    {
        cambiarVisibilidadBotones(false);
        estado = EstadoBatalla.INICIO;
        StartCoroutine(PrepararBatalla());
    }

    IEnumerator PrepararBatalla()
    {
        GameObject jugadorGameobject = Instantiate(prefabJugador, posicionJugador);
        unidadJugador = jugadorGameobject.GetComponent<Unidad>();

        GameObject enemigoGameobject = Instantiate(prefabEnemigo, posicionEnemigo);
        unidadEnemigo = enemigoGameobject.GetComponent<Unidad>();

        textoDialogo.text = "Un " + unidadEnemigo.nombreUnidad + " salvaje aparecio!!";

        HUDJugador.prepararHUD(unidadJugador);
        HUDEnemigo.prepararHUD(unidadEnemigo);

        yield return new WaitForSeconds(2f);

        estado = EstadoBatalla.TURNOJUGADOR;
        TurnoJugador();
    }

    IEnumerator JugadorAtaca()
    {
        bool estaMuerto = unidadEnemigo.recibirDaño(unidadJugador.ataque);
        HUDEnemigo.cambiarVida(unidadEnemigo.vidaActual);
        textoDialogo.text = "Ataque exitoso!";

        yield return new WaitForSeconds(1f);

        if (estaMuerto)
        {
            estado = EstadoBatalla.VICTORIA;
            TerminarBatalla();
        }
        else
        {
            estado = EstadoBatalla.TURNOENEMIGO;
            StartCoroutine(TurnoEnemigo());
        }
    }

    IEnumerator JugadorCura()
    {
        unidadJugador.recuperarVida(5);
        HUDJugador.cambiarVida(unidadJugador.vidaActual);
        textoDialogo.text = unidadJugador.nombreUnidad + " recupera vida";

        yield return new WaitForSeconds(1f);

        estado = EstadoBatalla.TURNOENEMIGO;
        StartCoroutine(TurnoEnemigo());
    }

    IEnumerator TurnoEnemigo()
    {
        textoDialogo.text = unidadEnemigo.nombreUnidad + " ataca!!";
        yield return new WaitForSeconds(1f);
        bool estaMuerto = unidadJugador.recibirDaño(unidadEnemigo.ataque);
        HUDJugador.cambiarVida(unidadJugador.vidaActual);
        yield return new WaitForSeconds(1f);

        if (estaMuerto)
        {
            estado = EstadoBatalla.DERROTA;
            TerminarBatalla();
        }
        else
        {
            estado = EstadoBatalla.TURNOJUGADOR;
            TurnoJugador();
        }
    }

    void TurnoJugador()
    {
        cambiarVisibilidadBotones(true);
        textoDialogo.text = "¿Qué hará " + unidadJugador.nombreUnidad + "?";
    }

    void TerminarBatalla()
    {
        if(estado == EstadoBatalla.VICTORIA)
        {
            textoDialogo.text = "HAS GANADO LA BATALLA";
            unidadEnemigo.gameObject.SetActive(false);
            HUDEnemigo.gameObject.SetActive(false);
        }
        else if (estado == EstadoBatalla.DERROTA)
        {
            textoDialogo.text = "HAS PERDIDO LA BATALLA";
            unidadJugador.gameObject.SetActive(false);
            HUDJugador.gameObject.SetActive(false);
        }
    }

    public void OnBotonAtaque()
    {
        if (estado != EstadoBatalla.TURNOJUGADOR) return;
        cambiarVisibilidadBotones(false);

        StartCoroutine(JugadorAtaca());
    }

    public void OnBotonCurar()
    {
        if (estado != EstadoBatalla.TURNOJUGADOR) return;
        cambiarVisibilidadBotones(false);

        StartCoroutine(JugadorCura());
    }

    void cambiarVisibilidadBotones(bool valor)
    {
        HUDAtaque.gameObject.SetActive(valor);
        HUDCurar.gameObject.SetActive(valor);
    }
}
