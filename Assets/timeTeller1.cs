using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Collections;
using TMPro;
using System;

public class timeTeller1 : MonoBehaviour
{
    public GameObject timeTextObject;
    struct TimeData
    {
        //public string client_ip;
        //...
        public string datetime;
        //..
    }
    // add your personal API key after APPID= and before &units=
    string url = "http://worldtimeapi.org/api/timezone/America/Chicago";

    void Start()
    {

        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }
    void GetDataFromWeb()
    {

        StartCoroutine(GetRequest(url));
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {


                TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);

                string date = Regex.Match(timeData.datetime, @"^\d{4}-\d{2}-\d{2}").Value;

                string time = Regex.Match(timeData.datetime, @"\d{2}:\d{2}:\d{2}").Value;

                int hours = Int32.Parse(time.Substring(0, 2));

                string minutes = time.Substring(2);

                string finalTime = "";

                if (hours == 0) 
                {
                    hours += 12;
                    finalTime += hours.ToString();
                    finalTime += minutes;
                    finalTime += "AM";
                } else if(hours >= 1 && hours <= 11)
                {
                    finalTime += hours.ToString();
                    finalTime += minutes;
                    finalTime += "AM";
                } else if (hours == 12)
                {
                    finalTime += hours.ToString();
                    finalTime += minutes;
                    finalTime += "PM";
                } else
                {
                    hours -= 12;
                    finalTime += hours.ToString();
                    finalTime += minutes;
                    finalTime += "PM";
                }


                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: ");

                // this code will NOT fail gracefully, so make sure you have
                // your API key before running or you will get an error

                // grab the current temperature and simplify it if needed
                //int startTemp = webRequest.downloadHandler.text.IndexOf("temp", 0);
                //int endTemp = webRequest.downloadHandler.text.IndexOf(",", startTemp);
                //double tempF = float.Parse(webRequest.downloadHandler.text.Substring(startTemp + 6, (endTemp - startTemp - 6)));
                //int easyTempF = Mathf.RoundToInt((float)tempF);
                //int finalTempC = (((easyTempF - 32) * 5) / 9);
                //Debug.Log ("integer temperature is " + easyTempF.ToString());
                //int startConditions = webRequest.downloadHandler.text.IndexOf("main", 0);
                //int endConditions = webRequest.downloadHandler.text.IndexOf(",", startConditions);
                //string conditions = webRequest.downloadHandler.text.Substring(startConditions + 7, (endConditions - startConditions - 8));
                //Debug.Log(conditions);

                timeTextObject.GetComponent<TextMeshPro>().text = date.ToString() + "\n" + finalTime.ToString();
            }
        }
    }
}

