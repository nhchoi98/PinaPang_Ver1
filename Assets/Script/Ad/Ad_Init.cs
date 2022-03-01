
public static class Ad_Init 
{
    private const string MaxSdkKey = "x6vHS-2fVBgV7wmqQh5pSZRkaaqurpf7u2Gk8HkE0LXDnLJ3ql_WvpzfTwxDN55qwCeX3Vfq1IbMGbQZBHeQMG";
    public static void Init_Ad()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
           
        };

        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
        //Set_Debugger();
    }
    /*
    /// <summary>
    /// 디버그용 코드. 안쓰면 주석처리 해주자.
    /// </summary>
    private static void Set_Debugger()
    {
        MaxSdk.SetCreativeDebuggerEnabled(true);
        MaxSdk.SetVerboseLogging(true);
    }
    */
    
    
}
