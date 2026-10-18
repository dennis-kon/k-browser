Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            IconService.Initialize()

            ' A crash skips the cleanup a private window does when it closes. Remove such leftovers
            ' (cookies, cache, history in %TEMP%); sessions still open in another window are left alone.
            System.Threading.Tasks.Task.Run(Sub() PrivateSessionService.SweepStaleSessions(PrivateSessionService.DefaultRoot))
        End Sub

    End Class


End Namespace

