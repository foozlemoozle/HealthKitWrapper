/// Created by: Kirk George
/// Copyright: Kirk George
/// Website: https://github.com/foozlemoozle?tab=repositories
/// See upload date for date created.

using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace com.keg.healthkitwrapper
{
    public static class HealthKitWrapper
    {
        private static event System.Action<double> OnGetDistanceInternal = ( double distance ) => { };

        private delegate void OnGetDistance( double distance );

        public static void GetDistance( long lastCheckedUnix, System.Action<double> onGetData )
        {
            OnGetDistanceInternal += onGetData;

#if UNITY_EDITOR
            OnGetDistanceDelegateWrapper( 1 );
#elif UNITY_IOS
        Debug.Log( "callback received" );
        GetDistanceWrapper( (double)lastCheckedUnix, OnGetDistanceDelegateWrapper );
#else
        onGetDistance( 1 );
#endif
        }

        [MonoPInvokeCallback( typeof( OnGetDistance ) )]
        private static void OnGetDistanceDelegateWrapper( double distance )
        {
            OnGetDistanceInternal( distance );
            OnGetDistanceInternal = ( double dist ) => { };
        }

#if UNITY_IOS
        [DllImport( "__Internal" )]
        private static extern void GetDistanceWrapper( double lastCheckedUnix, OnGetDistance onGetDistance );
#endif
    }
}
