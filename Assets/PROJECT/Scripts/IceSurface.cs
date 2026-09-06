using UnityEngine;

//public class IceSurface : MonoBehaviour
//{
//    [Header("Ajustes de Deslizamiento")]
//    [Tooltip("Aceleracion del jugador sobre el hielo (menor = mas patinoso al arrancar)")]
  //  [SerializeField] private float aceleracionHielo = 2f;
//
  //  [Tooltip("Deceleracion sobre el hielo (muy baja = desliza mucho al soltar teclas)")]
    //[SerializeField] private float deceleracionHielo = 0.5f;
//
  //  [Header("Valores por defecto del Player")]
    //[SerializeField] private float aceleracionOriginal = 15f;
//    [SerializeField] private float deceleracionOriginal = 20f;
//
  //  private void OnCollisionEnter(Collision collision)
  //  {
    //    if (collision.gameObject.CompareTag("Player"))
      //  {
        //    Movement movement = collision.gameObject.GetComponent<Movement>();
          //  if (movement != null)
            //{
              //  movement.ModificarTraccion(aceleracionHielo, deceleracionHielo);
            //}
        //}
    //}
//
  //  private void OnCollisionExit(Collision collision)
   // {
     //   if (collision.gameObject.CompareTag("Player"))
       // {
         //   Movement movement = collision.gameObject.GetComponent<Movement>();
           // if (movement != null)
          //  {
           //     movement.RestaurarTraccionOriginal(aceleracionOriginal, deceleracionOriginal);
            //}
       // }
   // }
//}