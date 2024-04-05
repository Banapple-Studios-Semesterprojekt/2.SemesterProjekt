#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

public class Death_and_respawn : MonoBehaviour
{ 
    //privat variabels 
    private Vector3 RespawnPosition;
    private CharacterController characterController;
    private Inventory inven;
    private PlayerController controller;

    // things to deactivate on death
    public GameObject[] GameObjects_To_Deactivate;
    public List<MonoBehaviour> Componets_To_Deactivate=new List<MonoBehaviour>();

    //thing to control Insecter UI
    [HideInInspector] public bool showWindowFoldOut;
    [HideInInspector] public bool Drop_Delete_checker = false;
   
    //respawn options
    [HideInInspector] public bool Auto_Respawn;
    [HideInInspector] public float Respawn_Time;
    [HideInInspector] public bool RespawnButton;
    [HideInInspector] public float ButtenApearDelay;

    //Death UI
    [HideInInspector] public GameObject Death_ui;
    [HideInInspector] public GameObject Black_Fade;
    [HideInInspector] public GameObject Respawn_button;
    [HideInInspector] public bool UseBlackFade;
    [HideInInspector] public float BlackFadeSpeed;

    //Invantory options
    [HideInInspector] public  bool Drop_Inventory_onDeath;
    [HideInInspector] public bool Delete_Inventory=false;
    [HideInInspector] public  bool Drop_Inventory=false;
    [HideInInspector] public bool Empty_Hotbar;
#if UNITY_EDITOR
    [CustomEditor(typeof(Death_and_respawn))]
    public class Death_and_respawn_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
            Death_and_respawn death_And_Respawn = (Death_and_respawn)target;
            death_And_Respawn.showWindowFoldOut = EditorGUILayout.BeginFoldoutHeaderGroup(death_And_Respawn.showWindowFoldOut,"Death UI Gameobjcts");
            if (death_And_Respawn.showWindowFoldOut)
            {
                death_And_Respawn.Death_ui = EditorGUILayout.ObjectField("Death UI", death_And_Respawn.Death_ui, typeof(GameObject), true) as GameObject;
                death_And_Respawn.Black_Fade = EditorGUILayout.ObjectField("Black Fade", death_And_Respawn.Black_Fade, typeof(GameObject), true) as GameObject;
                death_And_Respawn.Respawn_button = EditorGUILayout.ObjectField("Respawn button", death_And_Respawn.Respawn_button, typeof(GameObject), true) as GameObject;

            }



            death_And_Respawn.Auto_Respawn = GUILayout.Toggle(death_And_Respawn.Auto_Respawn, "Auto Respawn");

            if (death_And_Respawn.Auto_Respawn)
            {
                death_And_Respawn.Respawn_Time = EditorGUILayout.FloatField("Respawn Time", death_And_Respawn.Respawn_Time);
            }
            
            death_And_Respawn.UseBlackFade = GUILayout.Toggle(death_And_Respawn.UseBlackFade, "Use black fade");
            if (death_And_Respawn.UseBlackFade)
            {
                death_And_Respawn.BlackFadeSpeed = EditorGUILayout.FloatField("Fade Speed", death_And_Respawn.BlackFadeSpeed);
            }
            death_And_Respawn.RespawnButton = GUILayout.Toggle(death_And_Respawn.RespawnButton, "Respawn Butten");
            if (death_And_Respawn.RespawnButton)
            {
                death_And_Respawn.ButtenApearDelay = EditorGUILayout.FloatField("Butten Apear Delay", death_And_Respawn.ButtenApearDelay);
            }

            death_And_Respawn.Drop_Inventory_onDeath = GUILayout.Toggle(death_And_Respawn.Drop_Inventory_onDeath, "Drop Inventory On death");
           
            if (death_And_Respawn.Drop_Inventory_onDeath)
            {
                EditorGUILayout.Space();
                death_And_Respawn.Drop_Inventory = EditorGUILayout.Toggle("Drop Inventory", death_And_Respawn.Drop_Inventory);
                death_And_Respawn.Delete_Inventory = EditorGUILayout.Toggle("Empty Inventory", death_And_Respawn.Delete_Inventory);
                death_And_Respawn.Empty_Hotbar = EditorGUILayout.Toggle("Empty Hotbar",death_And_Respawn.Empty_Hotbar);
            }
            else
            {
                death_And_Respawn.Drop_Delete_checker = true;
                death_And_Respawn.Drop_Inventory = false;
                death_And_Respawn.Delete_Inventory = false;
                death_And_Respawn.Empty_Hotbar = false;

            }

           
            if (GUI.changed)
            {
                if (death_And_Respawn.Drop_Inventory_onDeath) {
                    if (death_And_Respawn.Drop_Inventory && death_And_Respawn.Delete_Inventory)
                    {
                        if (death_And_Respawn.Drop_Delete_checker)
                        {
                            death_And_Respawn.Drop_Inventory = false;
                            death_And_Respawn.Drop_Delete_checker = false;
                        }
                        else
                        {
                            death_And_Respawn.Delete_Inventory = false;
                            death_And_Respawn.Drop_Delete_checker = true;
                        }
                    }
                    else if (death_And_Respawn.Drop_Inventory || death_And_Respawn.Delete_Inventory)
                    {
                        if (death_And_Respawn.Drop_Inventory)
                        {
                            death_And_Respawn.Drop_Delete_checker = true;
                        }
                        else
                        {
                            death_And_Respawn.Drop_Delete_checker = false;
                        }
                    }
                    else
                    {
                        death_And_Respawn.Drop_Inventory = true;
                    }
                }
            }
            EditorUtility.SetDirty(target);
        }
    }
#endif
    private void Start()
    {
        SetSpawnPosition();
        characterController = GetComponent<CharacterController>();
        inven = GetComponent<Inventory>();
        controller = GetComponent<PlayerController>();
        Componets_To_Deactivate.Add(GetComponentInChildren<StaminaUI>());
        Componets_To_Deactivate.Add(GetComponentInChildren<Hotbar>());
    }

    public void SetSpawnPosition()
    {
        RespawnPosition = transform.position;
    }


    public void Die()
    {
        controller.ResetPlayer();
        Death_ui.SetActive(true);
        inven.isDead(true);
        characterController.enabled = false;
        playercomponetActivation(false);
        if (Drop_Inventory_onDeath)
        {
            EmptyInventory();
        }
        if (Auto_Respawn)
        {
            Invoke("Respawn", Respawn_Time);
        }
        if (UseBlackFade)
        {
            Black_Fade.SetActive(true);
            Black_Fade.GetComponent<Animator>().speed = BlackFadeSpeed;
        }
        if (RespawnButton)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Invoke("Activate_Respawn_Button", ButtenApearDelay);
              
        }

    }

    public void Respawn()
    {
        transform.position = RespawnPosition;
        playercomponetActivation(true);
        inven.isDead(false);
        characterController.enabled = true;
        Respawn_button.SetActive(false);
        Black_Fade.SetActive(false);
        Death_ui.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void EmptyInventory()
    {
       inven.Empty_inventory(Drop_Inventory,Empty_Hotbar);     

    }

    private void Activate_Respawn_Button()
    {
        Respawn_button.SetActive(true);
    }

    private void playercomponetActivation(bool b)
    {   
        
        for (int i = 0; i < GameObjects_To_Deactivate.Length; i++)
        {
            GameObjects_To_Deactivate[i].SetActive(b);
        }
        for (int i = 0; i < Componets_To_Deactivate.Count; i++)
        {
            Componets_To_Deactivate[i].enabled=b;
        }

       
    }

}