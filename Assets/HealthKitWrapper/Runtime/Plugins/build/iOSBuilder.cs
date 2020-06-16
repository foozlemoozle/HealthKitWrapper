/// Created by: Kirk George
/// Copyright: Kirk George
/// Website: https://github.com/foozlemoozle?tab=repositories
/// See upload date for date created.


#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace com.keg.healthkitwrapper
{
    public static class iOSBuilder
    {
        private static string ENTITLEMENTS_PATH = Application.dataPath + "/Plugins/build/entitlements/";

        [PostProcessBuild]
        public static void AddEntitlements( BuildTarget buildTarget, string pathToBuiltProject )
        {
            if( buildTarget == BuildTarget.iOS )
            {
                PBXProject pbxProj = new PBXProject();

                string pbxProjPath = PBXProject.GetPBXProjectPath( pathToBuiltProject ); //pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

                pbxProj.ReadFromFile( pbxProjPath );
                string target = pbxProj.TargetGuidByName( "Unity-iPhone" );

                string[] entitlements = Directory.GetFiles( ENTITLEMENTS_PATH );

                int count = entitlements.Length;
                for( int i = 0; i < count; ++i )
                {
                    if( entitlements[ i ].EndsWith( ".meta" ) )
                    {
                        //don't do the .meta file
                        continue;
                    }

                    Debug.LogFormat( "Adding entitlement {0} to xcode project.", entitlements[ i ] );
                    pbxProj.AddCapability( target, PBXCapabilityType.HealthKit, entitlements[ i ], true );
                }
            }
        }

        [PostProcessBuild]
        public static void ChangeXcodePlist( BuildTarget buildTarget, string pathToBuiltProject )
        {

            if( buildTarget == BuildTarget.iOS )
            {

                // Get plist
                string plistPath = pathToBuiltProject + "/Info.plist";
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString( File.ReadAllText( plistPath ) );

                // Get root
                PlistElementDict rootDict = plist.root;

                AddHealthKitAuthDescription( rootDict );

                // Write to file
                File.WriteAllText( plistPath, plist.WriteToString() );
            }
        }

        private static void AddHealthKitAuthDescription( PlistElementDict rootDict )
        {
            string key = "NSHealthShareUsageDescription";//"Privacy - Health Share Usage Description";
                                                         //change this to whatever you want
            string value = "This app uses exercise information to function.  Please authorize all of the above.";
            rootDict.SetString( key, value );
        }
    }
#endif
}
