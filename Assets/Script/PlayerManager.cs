using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;
using UnityEditor.Compilation;

public class PlayerManager : MonoBehaviour
{
    Text Email;
    Text Password;
    Text Nick;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUp()
    {
        Email = GameObject.Find("EMail").GetComponent<Text>();
        Password = GameObject.Find("Password").GetComponent<Text>();
        Nick = GameObject.Find("NickName").GetComponent<Text>();

        StartCoroutine(SignUpRoutine(Email.text,Password.text,Nick.text));
    }

    //LOGIN CON MAIL E PASSWORD
    IEnumerator SignUpRoutine(string email, string password, string nickname)
    {

        bool done = false;
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");

                done = true;
            }
            else
            {
                Debug.Log("user created successfully");
                LootLockerSDKManager.WhiteLabelLogin(Email.text, Password.text, false, response =>
                {
                    if (!response.success)
                    {
                        Debug.Log("error while login");
                        done = true;
                    }
                    else
                    {
                        Debug.Log("player logged in");

                        LootLockerSDKManager.StartWhiteLabelSession((response) =>
                        {
                            if (!response.success)
                            {
                                Debug.Log("error starting session");
                                done = true;
                            }
                            else
                            {
                                LootLockerSDKManager.SetPlayerName(nickname, (response) => {

                                    if (!response.success)
                                    {
                                        Debug.Log("error assigning nickname");
                                        done = true;
                                    }
                                    
                                else
                                    {
                                        Debug.Log("session started succesfully");
                                        LootLockerSessionRequest sessionRequest = new LootLockerSessionRequest();
                                        LootLocker.LootLockerAPIManager.EndSession(sessionRequest, (response) =>
                                        {
                                            if (!response.success)
                                            {
                                                Debug.Log("error ending session");
                                                done = true;
                                            }
                                            else {
                                                Debug.Log("account created succesfully");
                                            }
                                        });
                                    }
                                });
                            }

                        });
                    }
                });
                //          LootLockerSDKManager.SetPlayerName(nickname, (response) =>
                //          {
                //             if (!response.success)
                //                 return;
                //          });
            }
        });
        yield return new WaitWhile(() => done = false);
    }
    public void LogIn()
    {
       Text Email = GameObject.Find("LogInEMail").GetComponent<Text>();
       Text Password = GameObject.Find("LogInPassword").GetComponent<Text>();
        LootLockerSDKManager.WhiteLabelLogin(Email.text, Password.text, false, response =>
        {
            if (!response.success)
            {
                Debug.Log("error while login");
                return;
            }
            else
            {
                Debug.Log("player logged in");
            }
        LootLockerSDKManager.StartWhiteLabelSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting session");
                return;
            }
            else
            {
                Debug.Log("session started succesfully");
            }
        });
        });        
    }


        /* METODO PER LOGIN GUESTs
         * IEnumerator LoginRoutine()
        {
            bool done = false;
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (response.success)
                {
                    Debug.Log("player was logged in");
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    done = true;
                }
                else
                {
                    Debug.Log("could not start session");
                    done = true;
                }
            });
            yield return new WaitWhile(() => done = false);
        }
       */
    }
