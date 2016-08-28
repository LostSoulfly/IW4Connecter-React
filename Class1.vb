Imports Microsoft.Win32
Imports System
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.IO.File

    Public Class ConnectionManager
        ' Methods
        Private Shared Function GetEditText(ByVal hwnd As IntPtr) As String
            Dim capacity As Integer = ConnectionManager.SendMessage(hwnd, 14, 0, DirectCast(Nothing, StringBuilder))
            Dim lParam As New StringBuilder(capacity)
            ConnectionManager.SendMessage(hwnd, 13, CInt((capacity + 1)), lParam)
            Return lParam.ToString
        End Function

        Private Shared Function GetLogContent() As String
            Return ConnectionManager.GetEditText(ConnectionManager._hwndLog)
        End Function
    Public Shared Function CheckProcess() As Boolean
        If (Process.GetProcessesByName("iw4m").Length > 0) Then
            Return True
        ElseIf (Process.GetProcessesByName("iw4m.exe").Length > 0) Then
            Return True
        ElseIf (Process.GetProcessesByName("IW4 Multiplayer*").Length > 0) Then
            Return True
        End If

        Return False
    End Function

    Public Shared Function StartMW2(Optional ByVal strFile As String = "iw4m.exe") As Boolean
        If CheckProcess() = False Then
            'Console.WriteLine("Dir:" & Environment.CurrentDirectory())
            If Exists(Environment.CurrentDirectory() & "/" & strFile) Then
                Console.WriteLine("IW4 not running, attempting to start it..")
                Try
                    Dim nProcess As New Process
                    With nProcess
                        .StartInfo.UseShellExecute = False
                        .StartInfo.FileName = strFile
                        .Start()
                    End With
                Catch e As Exception
                    Console.WriteLine((e.Message))
                    Return False
                End Try
                Return True
            End If
            'If Shell("iw4m.exe", AppWinStyle.NormalFocus, False) > 0 Then StartMW2 = True
        Else
            'Console.WriteLine("iw4m.exe does not exist.")
            Return True
        End If

        Return False
    End Function

    Public Shared Sub Handle(ByVal args As String, ByVal isInitial As Boolean)
        Dim str As String = LCase(args) 'String.Join(" ", args)
        str = str.Replace("react://", "iw4://")
        If str.StartsWith("iw4://") Then
            Dim strArray As String() = str.Replace("iw4://", "").Split(New Char() {"/"c})
            Dim str4 As String = strArray(0)

            If (((Not str4 Is Nothing) AndAlso (str4 = "connect")) AndAlso (strArray.Length = 2)) Then
                Dim ip As String = strArray(1)

                If StartMW2() = False Then
                    Console.WriteLine("IW4 was not found or could not be started.")
                    Console.ReadKey()
                    End
                Else
                    ConnectionManager.PerformConnect(ip, isInitial)
                End If


            Else
                'Console.Write("You forgot something vital in the string! {0} doesn't exist!")
                PrintHelp()
            End If
        Else
            PrintHelp()
        End If
    End Sub

    Public Shared Sub Install(Optional ByVal aIW As Boolean = False)
        If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
            If aIW = True Then
                Try
                    Dim location As String = Assembly.GetExecutingAssembly.Location
                    Dim key2 As RegistryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\Classes\aiw")
                    key2.SetValue(Nothing, "URL:AIW Protocol")
                    key2.SetValue("URL Protocol", "")
                    Dim key3 As RegistryKey = key2.CreateSubKey("DefaultIcon")
                    key3.SetValue(Nothing, (location & ",1"))
                    key3.Close()
                    Dim key4 As RegistryKey = key2.CreateSubKey("shell\open\command")
                    key4.SetValue(Nothing, String.Format("""{0}"" ""%1""", location))
                    key4.Close()
                    key2.Close()
                    If Silent = False Then Console.WriteLine()
                    If Silent = False Then Console.WriteLine("Installed successfully. Press any key to continue.")
                    If Silent = False Then Console.ReadKey(True)
                Catch exception As Exception
                    If Silent = False Then Console.WriteLine(("THIS ERROR IS NOT FATAL: " & exception.ToString))
                End Try
            Else
                Try
                    Dim location As String = Assembly.GetExecutingAssembly.Location
                    Dim key2 As RegistryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\Classes\IW4")
                    key2.SetValue(Nothing, "URL:IW4 Protocol")
                    key2.SetValue("URL Protocol", "")
                    Dim key3 As RegistryKey = key2.CreateSubKey("DefaultIcon")
                    key3.SetValue(Nothing, (location & ",1"))
                    key3.Close()
                    Dim key4 As RegistryKey = key2.CreateSubKey("shell\open\command")
                    key4.SetValue(Nothing, String.Format("""{0}"" ""%1""", location))
                    key4.Close()
                    key2.Close()
                    If Silent = False Then Console.WriteLine()
                    If Silent = False Then Console.WriteLine("Installed successfully. Press any key to continue.")
                    If Silent = False Then Console.ReadKey(True)
                Catch exception As Exception
                    If Silent = False Then Console.WriteLine(("THIS ERROR IS NOT FATAL: " & exception.ToString))
                End Try
            End If
        End If
    End Sub

    Public Shared Sub UnInstall()
        If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
            Try
                Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\Classes\IW4")

                If Silent = False Then Console.WriteLine()
                If Silent = False Then Console.WriteLine("Uninstalled successfully. Press any key to continue.")
                If Silent = False Then Console.ReadKey(True)
            Catch exception As Exception
                If Silent = False Then Console.WriteLine(("THIS ERROR IS NOT FATAL: " & exception.ToString))
            End Try
        End If
    End Sub

    Public Shared Sub UnInstallAIW()
        If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
            Try
                Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\Classes\aiw")

                If Silent = False Then Console.WriteLine()
                If Silent = False Then Console.WriteLine("Uninstalled successfully. Press any key to continue.")
                If Silent = False Then Console.ReadKey(True)
            Catch exception As Exception
                If Silent = False Then Console.WriteLine(("THIS ERROR IS NOT FATAL: " & exception.ToString))
            End Try
        End If
    End Sub

    Private Shared Function OpenIW4Process() As IntPtr
        Dim procHwnd As Integer

        Console.Write("Waiting for IW4's process..")
        For i = 1 To 120
            procHwnd = FindWindow("IW4 WinConsole", vbNullString)
            If (procHwnd <> IntPtr.Zero) Then
                Return procHwnd
            End If
            Thread.Sleep(100)
            If IsEven(i) Then Console.Write(".")
        Next
        'Dim processesByName As Process() = Process.GetProcessesByName("iw4m")
        'Dim processArray2 As Process() = processesByName
        'Dim index As Integer = 0
        'Do While (index < processArray2.Length)
        '    Dim process As Process = processArray2(index)
        '    Return process.Handle
        'Loop

        'Return IntPtr.Zero
    End Function

    Private Shared Sub PerformConnect(ByVal ip As String, ByVal isInitial As Boolean)
        Dim ptr As IntPtr = ConnectionManager.OpenIW4Process

        If (ptr <> IntPtr.Zero) Then
            Console.WriteLine()
            Console.WriteLine("Waiting for IW4 to be ready..")
            ConnectionManager._process = ptr

            If ConnectionManager.WaitForConsole() = False Then End
            Console.WriteLine()
            If Not isInitial Then
                If ConnectionManager.WaitForLog("Successfully read stats data from IWNet") = False Then
                    Console.WriteLine("Unable to determine if console is ready.")
                    Console.ReadKey()
                    End
                End If
            End If
            ConnectionManager.SendToInput(("connect " & ip.Replace(" ", "").Replace(";", "")))
        Else
            Console.WriteLine("Error locating IW4's process!")
            Console.ReadKey(True)
            End
        End If
    End Sub

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, <Out()> ByVal lpBuffer As Byte(), ByVal dwSize As Integer, <Out()> ByRef lpNumberOfBytesRead As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Shared Function FindWindowExA(ByVal hWnd1 As Integer, ByVal hWnd2 As Integer, ByVal lpsz1 As String, ByVal lpsz2 As String) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetWindow(ByVal hWnd As IntPtr, ByVal uCmd As Integer) As IntPtr
    End Function

    '<DllImport("user32.dll")>
    'Private Shared Function SendMessageA(ByVal hWnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As String) As Int32
    'End Function

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInt32, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInt32, ByVal wParam As Integer, ByVal lParam As StringBuilder) As Integer
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInt32, ByVal wParam As IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByVal lParam As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInt32, ByVal wParam As IntPtr, ByVal lParam As StringBuilder) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As HandleRef, ByVal Msg As UInt32, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    '<DllImport("user32.dll")> _
    'Private Shared Function SendMessageW(ByVal hWnd As IntPtr, ByVal Msg As UInt32, ByVal wParam As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As String) As IntPtr
    'End Function

    Private Shared Sub SendToInput(ByVal [text] As String)
        ConnectionManager.SendMessage(ConnectionManager._hwndInput, 12, IntPtr.Zero, [text])
        ConnectionManager.SendMessage(ConnectionManager._hwndInput, &H102, 13, 0)

        'Dim textToSend As New StringBuilder([text])
        'iConsoleHwnd = FindWindow("IW4 WinConsole", vbNullString)
        'iCmdHwnd = FindWindowExA(iConsoleHwnd, 0, "edit", vbNullString)
        'iLogHwnd = GetWindow(iCmdHwnd, 2)
        'ConnectionManager.SendMessage(_hwndInput, WM_SETTEXT, IntPtr.Zero, textToSend)

    End Sub

    Private Shared Function WaitForConsole() As Boolean
        Console.Write("Waiting for console")
        For i = 1 To 120

            ConnectionManager._hwndInput = FindWindowExA(_process, 0, "edit", vbNullString)
            ConnectionManager._hwndLog = GetWindow(ConnectionManager._hwndInput, 2)

            If ((ConnectionManager._hwndInput <> IntPtr.Zero) AndAlso (ConnectionManager._hwndLog <> IntPtr.Zero)) Then
                Return True
            End If
            Thread.Sleep(500)
            If IsEven(i) Then Console.Write(".")
        Next
        Console.WriteLine()
        Console.WriteLine("Timed out, please try again")
        Console.ReadKey()
        Return False
    End Function

    Private Shared Function WaitForLog(ByVal contains As String) As Boolean
        Console.Write("Waiting for log")
        For i = 1 To 120
            If IsEven(i) Then Console.Write(".")
            If ConnectionManager.GetLogContent.Contains(contains) Then
                Return True
            End If
            Thread.Sleep(500)
        Next
        Console.WriteLine()
        Console.WriteLine("Timed out, please try again")
        Console.ReadKey()
        Return False
    End Function

    Public Shared Function IsEven(ByVal Number As Long) As Boolean
        IsEven = (Number Mod 2 = 0)
    End Function

    ' Fields
    Private Shared _hwndInput As IntPtr
    Private Shared _hwndLog As IntPtr
    Private Shared _process As IntPtr
    Public Const WM_CHAR As UInt32 = &H102
    Public Const WM_GETTEXT As UInt32 = 13
    Public Const WM_GETTEXTLENGTH As UInt32 = 14
    Public Const WM_SETTEXT As UInt32 = 12
End Class
