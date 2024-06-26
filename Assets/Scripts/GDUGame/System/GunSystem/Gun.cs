using QPFramework;
using System.Numerics;
using UnityEngine;

namespace GDUGame {
   /// <summary>
   /// Represent Current State of Gun
   /// </summary>
   public enum GunState {
      Idle = 0,
      Shooting = 1,
      Reloading = 10,
      Depleted = 11,
      Cooling = 20
   }

   /// <summary>
   /// Implement logic of Gameplay about Gun in Scene 
   /// </summary>
   /// <seealso cref="GDUGame.GDUController" />
   public class Gun: GDUController {
      private Bullet bullet;

      private GunData gunData;

      private int capacity;

      [SerializeField]
      private float fireRate;
      private float coolDownTimer;

      public string Name;

      public BindableProperty<GunState> State;

      private void Awake() {
         bullet = transform.Find("Bullet").GetComponent<Bullet>();
         gunData = this.GetSystem<IGunSystem>().CurrentGunData;

         // Set the gun to be able to fire immediately
         coolDownTimer = 0.0f;

         State = new BindableProperty<GunState>() {
            Value = GunState.Idle,
         };
      }

      /// <summary>
      /// Gun Shoot, implement by Inst Bullet Instance GameObject
      /// </summary>
      public void Shoot() {
         if(gunData.BulletCount.Value > 0 && State.Value == GunState.Idle && coolDownTimer <= 0) {
            //State Changing should be instead of using TimeSystem with Fire Frequency
            State.Value = GunState.Shooting;
            var b = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation);
            b.gameObject.SetActive(true);

            b.transform.localScale = bullet.transform.localScale;
            b.Trigger(b.transform);

            State.Value = GunState.Idle;

            coolDownTimer = fireRate; // Cool Down time in seconds
         }
      }

      /// <summary>
      /// Gun Cooldown, cooling down the gun between shots
      /// </summary>
      public void coolDown(float deltaTime){
         coolDownTimer -= deltaTime;
      }

      /// <summary>
      /// Gun Reload, Checking bullet count in gun and spare rounds count
      /// </summary>
      public void Reload() {
         if(gunData.BulletCount.Value < capacity && State.Value == GunState.Idle) {
            if(gunData.SpareRoundsCount.Value > 0) {
               var needBulletCount = capacity - gunData.BulletCount.Value;

               if(needBulletCount > 0) {
                  //Using TimeSystem to implement reload

                  //Remember to change state in ReloadCommand
               }
            }
         }
      }
   }
}
