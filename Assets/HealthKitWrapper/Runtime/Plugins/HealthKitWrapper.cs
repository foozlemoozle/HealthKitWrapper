using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

public static class HealthKitWrapper
{
    private static event System.Action<double> OnGetDistanceInternal = ( double distance ) => {};

    private delegate void OnGetDistance( double distance );

    public static void GetDistance( System.Action<double> onGetData )
    {
        OnGetDistanceInternal += onGetData;

#if UNITY_EDITOR
        OnGetDistanceDelegateWrapper( 1 );
#elif UNITY_IOS
        Debug.Log( "callback received" );
        GetDistanceWrapper( OnGetDistanceDelegateWrapper );
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
    private static extern void GetDistanceWrapper( OnGetDistance onGetDistance );
#endif
}
