/// Created by: Kirk George
/// Copyright: Kirk George
/// Website: https://github.com/foozlemoozle?tab=repositories
/// See upload date for date created.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.keg.healthkitwrapper;

namespace com.keg.healthkitwrapper.tests
{
    public class TestPlugin : MonoBehaviour
    {
        public TextMeshProUGUI text;

        // Start is called before the first frame update
        void Start()
        {
            HealthKitWrapper.GetDistance( 0, OnGetDistance );
        }

        private void OnGetDistance( double distance )
        {
            if( text != null )
            {
                text.text = "Distance: " + distance.ToString();
            }
        }
    }
}
