Module modConsole
    Public Silent As Boolean = False

    Sub Main()
        Dim bHelp As Boolean
        Console.Title = "IW4Connecter"
        If My.Application.CommandLineArgs.Count > 0 Then
            'Call ShowFolders() : Console.ReadKey()
            Environment.CurrentDirectory = System.AppDomain.CurrentDomain.BaseDirectory()
            'Console.WriteLine("Original code and concept by NTAuthority of alterIW.net")
            'Console.WriteLine()

            Dim Temp As String
            Dim strArray() As String
            Temp = Replace(LCase(My.Application.CommandLineArgs(0)), "react://", "")
            Temp = Replace(Temp, "iw4://", "")

            strArray = Split(Temp, "/")

            For i = LBound(strArray) To UBound(strArray)

                Select Case (strArray(i))

                    Case Is = "connect"
                        Dim strArgs As String
                        If i + 1 <= UBound(strArray) Then
                            strArgs = "iw4://connect/" & strArray(i + 1)
                            i += 1
                            If ConnectionManager.CheckProcess = True Then
                                ConnectionManager.Handle(strArgs, False)
                            Else
                                ConnectionManager.Handle(strArgs, False)
                            End If
                        End If

                    Case Is = "start"
                        If ConnectionManager.StartMW2() = False Then
                            Console.WriteLine("IW4 could not be started!")
                            Console.ReadKey(True)
                        End If

                    Case Is = "uninstall"
                        ConnectionManager.UnInstall()

                    Case Is = "uninstallaiw"
                        ConnectionManager.UnInstallAIW()

                    Case Is = "suninstallaiw"
                        Silent = True
                        ConnectionManager.UnInstallAIW()

                    Case Is = "suninstall"
                        Silent = True
                        ConnectionManager.UnInstall()

                    Case Is = "sinstall"
                        Silent = True
                        ConnectionManager.Install()

                    Case Is = "install"
                        ConnectionManager.Install()

                    Case Is = "pause"
                        Console.ReadKey()

                    Case Is = "help"
                        PrintHelp()
                        End

                    Case Else
                        If Len(strArray(i)) > 1 Then Console.WriteLine("Unknown command: " & strArray(i)) : bHelp = True
                End Select

            Next

        Else
            bHelp = True
        End If
        If bHelp = True Then Console.Clear() : PrintHelp()
        End

    End Sub

    'Public Sub ShowFolders()
    '    Console.WriteLine("1 " & System.AppDomain.CurrentDomain.BaseDirectory()) : Console.WriteLine()
    '    Console.WriteLine("2 " & System.Reflection.Assembly.GetExecutingAssembly.Location.ToString) : Console.WriteLine()
    '    Console.WriteLine("CurrDir: " & Environment.CurrentDirectory) : Console.WriteLine()
    '    Console.WriteLine("Initial: " & Environment.CurrentDirectory) : Console.WriteLine()
    'End Sub

    Public Sub PrintHelp()
        On Error GoTo err
        Dim cki As ConsoleKeyInfo
        Dim temp As String

        'Console.Clear()
        Console.WriteLine("Original code and concept by NTAuthority of alterIW.net")
        Console.WriteLine()
        If My.Application.CommandLineArgs.Count > 0 Then Console.WriteLine("Your incorrect arguments: " & My.Application.CommandLineArgs(0)) : Console.WriteLine()

        Console.WriteLine("Correct Command Line Usage:")
        Console.WriteLine("To connect to a server: iw4://connect/[IP/Hostname]:[PORT] (Default port: 28960)")
        Console.WriteLine("To install the iw4:// protocol: /install")
        Console.WriteLine("to uninstall the iw4:// protocol: /uninstall")
        Console.WriteLine("To just launch IW4: /start (iw4://start in RUN.)")
        Console.WriteLine()
        Console.WriteLine("From RUN box: iw4://<command>/<command>/")
        Console.WriteLine("RUN Example: iw4://connect/192.168.1.100:28960")
        Console.WriteLine("From Command Prompt: IW4Connecter.exe /<command>/<command>")
        Console.WriteLine("Command Prompt example: IW4Connecter.exe /connect/192.168.1.100:28960")
        Console.WriteLine("Usable <commands>:")
        Console.WriteLine()
        Console.WriteLine("help " & vbTab & vbTab & "Display this help.")
        Console.WriteLine("pause " & vbTab & vbTab & "Pause the program after completing a Connect parse.")
        Console.WriteLine("install " & vbTab & "Install the IW4 protocol.")
        Console.WriteLine("sinstall " & vbTab & "Install the IW4 protocol silently.")
        Console.WriteLine("uninstall " & vbTab & "Remove the IW4 protocol.")
        Console.WriteLine("suninstall " & vbTab & "Remove the IW4 protocol Silently.")
        Console.WriteLine("start " & vbTab & vbTab & "Attempt to start IW4 without a server selected.")
        Console.WriteLine("connect " & vbTab & "Start a connection, use /connect/<IP:PORT>")

        Console.WriteLine()


Start:

        Console.WriteLine("")
        Console.WriteLine("Enter the number to select an option:")
        Console.WriteLine("1. Install IW4connecter's iw4:// protocol.")
        Console.WriteLine("2. Install IW4connecter as react:// protocol.")
        Console.WriteLine("3. Uninstall IW4connecter's iw4:// protocol.")
        Console.WriteLine("4. Uninstall react:// protocol.")
        Console.WriteLine("5. Start IW4 and connect to an IP and port.")
        Console.WriteLine("6. Farewell AIW, you were amazing..")



        cki = Console.ReadKey(True)

        Select Case cki.KeyChar
            Case Is = "1"
                ConnectionManager.Install()
            Case Is = "2"
                ConnectionManager.Install(True)
            Case Is = "3"
                ConnectionManager.UnInstall()
            Case Is = "4"
                ConnectionManager.UnInstall()
            Case Is = "5"

                'Console.Clear()
repeat:         Console.WriteLine()
                Console.WriteLine("Please type the IP you would like to connect to. (Port 28960 is implied)")
                Console.WriteLine("(Or leave empty to return to menu)")
                Console.WriteLine("Example: 192.168.1.100:28960")
                Console.WriteLine()
                temp = Console.ReadLine

                If Len(temp) = 0 Then GoTo Start
                'If Not InStr(temp, ":") > 0 Then Console.WriteLine("Error: No PORT specified. You must include the port after the colon.") : GoTo repeat
                If InStr(temp, "/") > 0 Then Console.WriteLine("Error: No slashes of any sort allowed. IP:Port only.") : Console.Clear() : GoTo repeat
                If InStr(temp, "\") > 0 Then Console.WriteLine("Error: No slashes of any sort allowed. IP:Port only.") : Console.Clear() : GoTo repeat

                If ConnectionManager.CheckProcess = True Then
                    Console.WriteLine("IW4 process detected, attempting to connect to server..")
                    ConnectionManager.Handle("iw4://connect/" & temp, True)
                Else
                    ConnectionManager.Handle("iw4://connect/" & temp, False)
                End If

            Case Is = "6"
                'Console.WriteLine()
                Console.WriteLine("   _____  .__   __               .___ __      __              __   ")
                Console.WriteLine("  /  _  \ |  |_/  |_  ___________|   /  \    /  \____   _____/  |_ ")
                Console.WriteLine(" /  /_\  \|  |\   __\/ __ \_  __ \   \   \/\/   /    \_/ __ \   __\")
                Console.WriteLine("/    |    \  |_|  | \  ___/|  | \/   |\        /   |  \  ___/|  |  ")
                Console.WriteLine("\____|__  /____/__|  \___  >__|  |___| \__/\  /|___|  /\___  >__|  ")
                Console.WriteLine("        \/               \/                 \/      \/     \/      ")
                Console.WriteLine()
                Console.WriteLine("Simply amazing. The amount of work that went into aIW was astounding. Reverse engineering extremely complex encryption schemes, designing and creating server components that emulated that of Activision, and making dedicated servers.")
                Console.WriteLine()
                Console.WriteLine("Modern Warfare 2, Black Ops, and Modern Warfare 3. We didn't deserve any it but we thank you endlessly. I spent many hours with friends on MW2. I had several LAN parties in which I'd host the server written by aIW and everyone would join. Not having to rely on an internet connection or some master server that will go down some day is quite liberating.")
                Console.WriteLine()
                Console.WriteLine("Thankfully, I will still be able to have LANs with MW2. I've got the source backed up for the server and I'll keep a copy of the aIW's MW2 for decades. What a magnificent accomplishment. Thank you so much.")
                Console.WriteLine()
                Console.WriteLine(" __                     ")
                Console.WriteLine("/__ _  _  _| |_    _    ")
                Console.WriteLine("\_|(_)(_)(_| |_)\/(/_oo ")
                Console.WriteLine("                /       ")
                Console.WriteLine("     -Dragoonadept/LostSoulFly")
                Console.ReadKey()
        End Select

        End

err:
        Console.WriteLine()
        Console.WriteLine("Error: " & Err.Number & " - " & Err.Description)


    End Sub

End Module
