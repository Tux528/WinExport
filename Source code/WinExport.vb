Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Public Class WinExport

    ReadOnly UserConfigurationFile As String = Path.Combine(Application.StartupPath & "WinExport.ini")

    Private Sub WinExport_Load(sender As Object, e As EventArgs) Handles Me.Load
        If File.Exists(UserConfigurationFile) Then
            LoadConfiguration(UserConfigurationFile)
        End If
    End Sub

    Private Sub LoadConfiguration(FilePath As String)
        For Each Line As String In File.ReadAllLines(FilePath, Encoding.UTF8)
            Dim Match As Match = Regex.Match(Line, "[a-zA-Z]+=[a-zA-Z]{4,5}")

            If Match.Success Then
                Dim CheckBox As CheckBox = Controls.Find(Match.Value.Split("=")(0), True).OfType(Of CheckBox).FirstOrDefault
                Dim CheckBoxState As String = Match.Value.Split("=")(1)

				If CheckBox IsNot Nothing AndAlso (CheckBoxState.Equals("True", StringComparison.OrdinalIgnoreCase) OrElse CheckBoxState.Equals("False", StringComparison.OrdinalIgnoreCase)) Then
                    CheckBox.Checked = Boolean.Parse(CheckBoxState)
                End If
            End If
        Next

        Dim DestinationDirectory As String = File.ReadLines(UserConfigurationFile).Where(Function(o) o.StartsWith("DestinationDirectory", StringComparison.OrdinalIgnoreCase)).First.Split("=")(1)

		If Not String.IsNullOrEmpty(DestinationDirectory) Then
            DestinationDirectoryTextBox.Text = DestinationDirectory
        End If
    End Sub

    Private Sub ExportConfiguration(ConfigurationFile As String)
        File.WriteAllText(ConfigurationFile, "[Configuration]" & Environment.NewLine, Encoding.UTF8)
        Dim Configuration As New StringBuilder

		For Each TabPage As TabPage In TabControl.TabPages
            For Each CheckBox As CheckBox In TabPage.Controls.OfType(Of CheckBox).OrderBy(Function(c) c.TabIndex)
                Configuration.Append(Environment.NewLine & CheckBox.Name & "=" & CheckBox.Checked)
            Next
        Next

		Configuration.Append(Environment.NewLine & "DestinationDirectory=" & DestinationDirectoryTextBox.Text.Replace("""", ""))
        File.AppendAllText(ConfigurationFile, Configuration.ToString, Encoding.UTF8)
    End Sub

    Private Sub ExportWizardButton_Click(sender As Object, e As EventArgs)
        CustomExportPanel.Visible = False
        OptionsPanel.Visible = False
        ExportWizardPanel.Visible = True
    End Sub

    Private Sub CustomExportButton_Click(sender As Object, e As EventArgs) Handles CustomExportButton.Click
        ExportWizardPanel.Visible = False
        ToolsPanel.Visible = False
        OptionsPanel.Visible = False
        CustomExportPanel.Visible = True
    End Sub

    Private RegFilePath As String

    Private Sub ImportConfigurationFileButton_Click(sender As Object, e As EventArgs) Handles ImportConfigurationFileButton.Click
        OpenFileDialog.Title = "Select a configuration file to import in WinExport..."

        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            LoadConfiguration(OpenFileDialog.FileName)
            MessageBox.Show("The selected configuration file has been successfully imported.", "Import of the configuration completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ExportCurrentConfigurationButton_Click(sender As Object, e As EventArgs) Handles ExportCurrentConfigurationButton.Click
        SaveFileDialog.Title = "Select a destination directory for the configuration file to export..."
        Dim Result As DialogResult = SaveFileDialog.ShowDialog()

        If Result = DialogResult.OK Then
            ExportConfiguration(SaveFileDialog.FileName)
            MessageBox.Show("The current configuration of the software has been successfully exported.", "Export of the configuration completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub CheckAllButton_Click(sender As Object, e As EventArgs) Handles CheckAllButton.Click
        GroupedChecking(True)
    End Sub

    Private Sub UncheckAllButton_Click(sender As Object, e As EventArgs) Handles UncheckAllButton.Click
        GroupedChecking(False)
    End Sub

    Private Sub GroupedChecking(CheckBoxesState As Boolean)
        For Each CheckBox As CheckBox In SystemSettingsTab.Controls.OfType(Of CheckBox)
            CheckBox.Checked = CheckBoxesState
        Next
    End Sub

    Private Sub GeneralSettingsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles GeneralSettingsCheckBox.CheckedChanged
        Dim GeneralSettings As New List(Of CheckBox) From {ComputerName, DateandHourSettings, RegionalOptions, NotificationsSettings, SearchSettings, ClipboardSettings}

        CheckBoxes(GeneralSettings, GeneralSettingsCheckBox)
    End Sub

    Private Sub AppearanceSettingsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles AppearanceSettingsCheckBox.CheckedChanged
        Dim AppearanceSettings As New List(Of CheckBox) From {Colors, WallpapersSettings, VisualEffectsSettings, ThemesSettings, MouseCursorsSettings, DesktopAppearanceSettings, StartMenuSettings, TaskbarSettings, AccessibilitySettings, LockScreenSettings, NotificationCenterQuickActions, DisplayOrientation, TabletModeSettings, VideoPlaybackSettings}

        CheckBoxes(AppearanceSettings, AppearanceSettingsCheckBox)
    End Sub

    Private Sub BehavioralSettingsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles BehavioralSettingsCheckBox.CheckedChanged
        Dim BehavioralSettings As New List(Of CheckBox) From {UACSettings, FilesandApplicationsAssociationSettings, InputSettings, AdsSettings, PersonalizationSettings, TelemetrySettings, ApplicationsPrivacySettings, NightLightSettings, AutoPlaySettings, GameSettings, DefragmentationSettings}

        CheckBoxes(BehavioralSettings, BehavioralSettingsCheckBox)
    End Sub

    Private Sub DevicesSettingsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DevicesSettingsCheckBox.CheckedChanged
        Dim DevicesSettings As New List(Of CheckBox) From {Printers, KeyboardSettings, MouseSettings, TouchpadSettings, USBDevicesSettings}

        CheckBoxes(DevicesSettings, DevicesSettingsCheckBox)
    End Sub

    Private Sub AdvancedSettingsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles AdvancedSettingsCheckBox.CheckedChanged
        Dim AdvancedSettings As New List(Of CheckBox) From {LanguageBarSettings, UninstalledOptionalFeatures, DeliveryOptimizationSettings}

        CheckBoxes(AdvancedSettings, AdvancedSettingsCheckBox)
    End Sub

    Private Sub WindowsApplicationsCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles WindowsApplicationsCheckBox.CheckedChanged
        Dim ApplicationsSettings As New List(Of CheckBox) From {WindowsUpdateSettings, MicrosoftDefenderSettings, CortanaSettings, FileExplorerSettings, TaskManagerSettings, PaintSettings, NotepadSettings, WordPadSettings, WindowsMediaPlayerSettings, SpeechRecognitionSettings, StorageSenseSettings, NarratorSettings, VisualKeyboardSettings, MagnifierSettings, TerminalsSettings}

        CheckBoxes(ApplicationsSettings, WindowsApplicationsCheckBox)
    End Sub

    Private Shared Sub CheckBoxes(CheckBoxesList As List(Of CheckBox), CategoryCheckBox As CheckBox)
        For Each CheckBox As CheckBox In CheckBoxesList
            CheckBox.Checked = CategoryCheckBox.Checked
        Next
    End Sub

    Private Sub OpenFolderButton_Click(sender As Object, e As EventArgs) Handles OpenFolderButton.Click
        Dim DestinationDirectory As String = DestinationDirectoryTextBox.Text.Replace("""", "")

		If String.IsNullOrEmpty(DestinationDirectory) Then
            MessageBox.Show("Please select or enter a destination directory for the settings file.", "Missing destination directory", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf Directory.Exists(DestinationDirectory) Then
            Process.Start("Explorer.exe", DestinationDirectory)
        Else
            MessageBox.Show("The specified folder does not exist or contains invalid characters. Please check your input or select another location.", "Nonexistent or invalid destination directory", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click
        If FolderBrowserDialog.ShowDialog() = DialogResult.OK Then
            DestinationDirectoryTextBox.Text = FolderBrowserDialog.SelectedPath
        End If
    End Sub

    Private Sub ExportSelectedSettingsButton_Click(sender As Object, e As EventArgs) Handles ExportSelectedSettingsButton.Click
        Dim DestinationDirectory As String = DestinationDirectoryTextBox.Text.Replace("""", "")

		If String.IsNullOrEmpty(DestinationDirectory) Then
            MessageBox.Show("Please select or enter a destination directory for the settings file.", "Missing destination directory", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return
        ElseIf Directory.Exists(DestinationDirectory) Then
            RegFilePath = RegFile()
        Else
            MessageBox.Show("The specified folder does not exist or contains invalid characters. Please check your input or select another location.", "Nonexistent or invalid destination directory", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return
        End If

        Dim SettingsNumber As Integer = 0

		For Each TabPage As TabPage In TabControl.TabPages
            For Each CheckBox As CheckBox In TabPage.Controls.OfType(Of CheckBox).Where(Function(c) c.Checked AndAlso Not String.IsNullOrEmpty(c.Text))
                SettingsNumber += 1
            Next
        Next

        If SettingsNumber = 0 Then
            MessageBox.Show("Please check one or more options to start the process of exporting settings.", "No option checked", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If ThemesSettings.Checked OrElse MouseCursorsSettings.Checked OrElse Printers.Checked Then
            Dim Result As DialogResult = MessageBox.Show("The selected settings may refer to dynamic information that corresponds to the current configuration of this version of Windows." & Environment.NewLine & "Therefore, these settings may not be correctly restored on another computer and may cause bugs or errors if the configuration is different." & Environment.NewLine & "This concerns notably the paths of wallpapers, themes, mouse pointers, or other information customized by the user." & Environment.NewLine & "Are you sure to export the selected settings?", "Custom settings", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If Result = DialogResult.No Then
                ExportSelectedSettingsButton.Enabled = True

                Return
            End If
        End If

        ExportSelectedSettingsButton.Enabled = False
        File.WriteAllText(RegFilePath, "Windows Registry Editor Version 5.00", Encoding.Unicode)

        If ComputerName.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName", "/v", "/i:v", "ComputerName")
        End If

        If DateandHourSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SYSTEM\CurrentControlSet\Control\TimeZoneInformation")
            ExportRegistryKey(RegistryHive.LocalMachine, "System\ControlSet001\Services\tzautoupdate", "/v", "/i:v", "Start")
            ExportRegistryKey(RegistryHive.LocalMachine, "System\ControlSet001\Services\W32Time", "/v", "/i:v", "Start")
            ExportRegistryKey(RegistryHive.LocalMachine, "System\ControlSet001\Services\W32Time\Parameters", "/v", "/i:v", "Type")
            ExportRegistryKey(RegistryHive.LocalMachine, "Software\Microsoft\Windows NT\CurrentVersion\Schedule\WP\TaskScheduler\Parameters", "/v", "/i:v", "TimeZoneInfo")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\$$windows.data.lunarcalendar\Current")
        End If

        If RegionalOptions.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\International")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\International\Geo")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\International\User Profile", "/r")
        End If

        If NotificationsSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\PushNotifications")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings", "/r", "/i:t", "DWord")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\UserProfileEngagement", "/v", "/i:v", "ScoobeSystemSettingEnabled")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "/v", "/i:v", "SubscribedContent-338389Enabled")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "/v", "/i:v", "SubscribedContent-310093Enabled")

			For Each SubKey As String In {"$$windows.data.notifications.quiethourssettings", "$microsoft.quiethoursprofile.priorityonly$windows.data.notifications.quiethoursprofile"}
                ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount\" & SubKey & "\Current")
            Next
        End If

        If SearchSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Search", "/v", "/i:t", "DWord")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Search\Preferences")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\ConnectedSearch")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Windows Search")
        End If

        If ClipboardSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Clipboard")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Clipboard")
        End If

        If Colors.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\DWM")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\History", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Colors")
            ExportRegistryKey(RegistryHive.CurrentUser, "HKEY_CURRENT_USER\Control Panel\Desktop", "/v", "/i:v", "AutoColorization")
        End If

        If WallpapersSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Desktop", "/v", "/i:v", "WallPaper")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Colors", "/v", "/i:v", "Background")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Personalization\Desktop Slideshow")
        End If

        If VisualEffectsSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "/r")
        End If

        If ThemesSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Themes", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ThemeManager")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\DWM")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Accessibility", "/r")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Cursors", "/r")
        End If

        If MouseCursorsSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Cursors", "/r")
        End If

        If DesktopAppearanceSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Desktop")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Desktop\WindowMetrics")
        End If

        If StartMenuSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "/v", "/i:v", "Start_")

            For Each KeyName As String In {"windows.data.unifiedtile.startglobalproperties", "start.tilegrid$windows.data.curatedtilecollection.tilecollection"}
                ExportRegistryKey(RegistryHive.CurrentUser, REG_QUERY("SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\Cache\DefaultAccount", KeyName) & "\Current")
            Next

            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "/v", "/i:v", "SubscribedContent-338388Enabled")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "/v", "/i:v", "StartMenuLogOff")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Desktop", "/v", "/i:v", "MenuShowDelay")
        End If

        If TaskbarSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "/v", "/i:v", "Task")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MMStuckRects3")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Feeds")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Policies\Microsoft\Windows\Windows Feeds")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "/v", "/i:v", "EnableAutoTray")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "/v", "/i:v", "DisablePreviewDesktop")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "/v", "/i:v", "DontUsePowerShellOnWinX")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\TabletTip\1.7", "/v", "/i:v", "TipbandDesiredVisibility")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Touchpad")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StuckRects2")
        End If

        If AccessibilitySettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Accessibility", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Accessibility", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Appearance", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\ColorFiltering")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Accessibility", "/v", "/i:v", "Configuration")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Cursors")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Themes", "/v", "/i:v", "CurrentTheme")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\DWM")
        End If

        If LockScreenSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\SystemProtectedUserData" & Security.Principal.WindowsIdentity.GetCurrent().User.ToString & "\AnyoneRead\LockScreen")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData", "/v", "/i:v", "AllowLockScreen")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Lock Screen", "/v", "/i:t", "DWord")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "/v", "/i:v", "LockScreen")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager", "/v", "/i:v", "SubscribedContent-338387Enabled")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Policies\Microsoft\Windows\Personalization", "/v", "/i:v", "LockScreen")
        End If

        If NotificationCenterQuickActions.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Quick Actions", "/r")
        End If

        If DisplayOrientation.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\AutoRotation")
        End If

        If TabletModeSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "/r")
        End If

        If VideoPlaybackSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\VideoSettings")
        End If

        If UACSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "/v", "/i:v", "ConsentPromptBehavior")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "/v", "/i:v", "EnableLUA")
        End If

        If FilesandApplicationsAssociationSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ApplicationAssociationToasts")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\RegisteredApplications")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations", "/r")
        End If

        If InputSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\TabletTip\1.7")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Input\Settings")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Input\TIPC")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\InputPersonalization")
        End If

        If AdsSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\AdvertisingInfo")
        End If

        If PersonalizationSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager")
        End If

        If ApplicationsPrivacySettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager", "/r")
        End If

        If TelemetrySettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Policies\Microsoft\Windows\DataCollection")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy")
        End If

        If NightLightSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.settings\windows.data.bluelightreduction.settings")
        End If

        If AutoPlaySettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\UserChosenExecuteHandlers", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\EventHandlersDefaultSelection", "/r")
        End If

        If GameSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\GameBar")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR")
        End If

        If DefragmentationSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Dfrg\TaskSettings")
        End If

        If Printers.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Printers", "/r")
            ExportRegistryKey(RegistryHive.LocalMachine, "SYSTEM\CurrentControlSet\Control\Print\Printers")
        End If

        If KeyboardSettings.Checked Then
            ExportRegistryKey(RegistryHive.Users, ".DEFAULT\Control Panel\Keyboard")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Keyboard")
        End If

        If MouseSettings.Checked Then
            ExportRegistryKey(RegistryHive.Users, ".DEFAULT\Control Panel\Mouse")
            ExportRegistryKey(RegistryHive.CurrentUser, "Control Panel\Mouse")
        End If

        If TouchpadSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "/r")
			Dim Key As String = REG_QUERY("SOFTWARE\Synaptics\SynTP", "TouchPad")

            If Not String.IsNullOrEmpty(Key) Then
                ExportRegistryKey(RegistryHive.CurrentUser, Key)
            End If

            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Synaptics\SynTP", "/r")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Synaptics\SynTPCpl")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Synaptics\SynTPEnh", "/r")
        End If

        If USBDevicesSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Shell\USB")
        End If

        If LanguageBarSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\CTF\LangBar")
        End If

        If UninstalledOptionalFeatures.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Features on Demand\Removed FOD Markers")
        End If

        If DeliveryOptimizationSettings.Checked Then
            ExportRegistryKey(RegistryHive.Users, "S-1-5-20\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization", "/r")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization", "/r")
        End If

        If WindowsUpdateSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\WindowsUpdate\UpdatePolicy")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\WindowsUpdate\UX\Settings")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "/r")
        End If

        If MicrosoftDefenderSettings.Checked Then
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows Defender Security Center", "/r")
        End If

        If CortanaSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Cortana")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Policies\Microsoft\Windows\Windows Search", "/v", "/i:v", "Cortana")
        End If

        If FileExplorerSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "/v", "/i:v", "Show")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Search", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer")
        End If

        If TaskManagerSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\TaskManager")
        End If

        If PaintSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Paint", "/r")
        End If

        If NotepadSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Notepad")
        End If

        If WordPadSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Wordpad", "/r")
        End If

        If WindowsMediaPlayerSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\MediaPlayer", "/r")
        End If

        If StorageSenseSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\StorageSense\Parameters\StoragePolicy")
        End If

        If SpeechRecognitionSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Speech_OneCore\Settings", "/r")
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Speech_OneCore\Preferences")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Speech_OneCore\Settings", "/r")
            ExportRegistryKey(RegistryHive.LocalMachine, "SOFTWARE\Microsoft\Speech_OneCore\Preferences")
        End If

        If NarratorSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Narrator", "/r")
        End If

        If VisualKeyboardSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\Osk")
        End If

        If MagnifierSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "SOFTWARE\Microsoft\ScreenMagnifier")
        End If

        If TerminalsSettings.Checked Then
            ExportRegistryKey(RegistryHive.CurrentUser, "Console", "/r")
        End If

        Dim Message As String = SettingsNumber & " "c

        If SettingsNumber = 1 Then
            Message &= "setting has been"
        ElseIf SettingsNumber >= 2 Then
            Message &= "settings have been"
        End If
        Message &= " successfully exported."

        MessageBox.Show(Message, "Export of selected settings completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ExportSelectedSettingsButton.Enabled = True
    End Sub

    Private Shared Function REG_QUERY(KeyPath As String, KeyName As String)
        Dim RegistryKey As RegistryKey = Registry.CurrentUser.OpenSubKey(KeyPath)

        If RegistryKey IsNot Nothing Then
            Dim Key As String = String.Empty

            For Each SubKey As String In RegistryKey.GetSubKeyNames
                If SubKey.Contains(KeyName, StringComparison.OrdinalIgnoreCase) Then
                    Key = SubKey
                End If
            Next

            Return KeyPath & "\"c & Key
        Else
            Return Nothing
        End If
    End Function

    Private Sub ExportRegistryKey(Hive As RegistryHive, SubKey As String, Optional ExportationMode As String = "/v", Optional Filter As String = Nothing, Optional FilteredItem As String = Nothing)
        Dim Key As RegistryKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default).OpenSubKey(SubKey)

		If Key IsNot Nothing Then
            ExportSubKeys(Key, ExportationMode, Filter, FilteredItem)
            Key.Close()
        End If
    End Sub

    Private Sub ExportSubKeys(Key As RegistryKey, ExportationMode As String, Optional Filter As String = Nothing, Optional FilteredItem As String = Nothing)
        RegFilePath = RegFile()
        Dim ValueNames As String() = Key.GetValueNames()

		If ValueNames IsNot Nothing AndAlso ValueNames.Length > 0 Then
            File.AppendAllText(RegFilePath, String.Format(Environment.NewLine & Environment.NewLine & "[{0}]", Key.ToString), Encoding.Unicode)

            Dim HexadecimalValue As New StringBuilder

			For Each ValueName As String In ValueNames.OrderBy(Function(v) v)
                HexadecimalValue.Clear()
                Dim Value As String = String.Empty
                Dim Pattern As String = """{0}""="
                Dim ValueType As RegistryValueKind = Key.GetValueKind(ValueName)
                Dim RawValue As Object = Key.GetValue(ValueName, ValueType, RegistryValueOptions.DoNotExpandEnvironmentNames)

				If Not String.IsNullOrEmpty(Filter) Then
                    Select Case Filter.Split(":")(1)
                        Case "t"
                            If Not String.Equals(ValueType.ToString, FilteredItem, StringComparison.OrdinalIgnoreCase) Then
                                Continue For
                            End If
                        Case "v"
                            If Not String.IsNullOrEmpty(FilteredItem) AndAlso Not ValueName.Contains(FilteredItem, StringComparison.OrdinalIgnoreCase) Then
                                Continue For
                            End If
                    End Select
                End If

                If String.IsNullOrEmpty(ValueName) Then
                    Pattern = "{0}="
                    ValueName = "@"
                End If

                Select Case ValueType
                    Case RegistryValueKind.String
                        Pattern &= """{1}"""
                        Value = RawValue

                        If Value.Contains("\"c) Then
                            Value = Value.Replace("\"c, "\\")
                        End If

                        If Value.Contains(""""c) Then
                            Value = Value.Replace("""", "\""")
                        End If
                    Case RegistryValueKind.DWord
                        Pattern &= "dword:{1}"
                        Value = Convert.ToInt32(RawValue).ToString("x8")
                    Case RegistryValueKind.Binary
                        Pattern &= "hex:{1}"
                        Dim BinaryValue As Byte() = RawValue

                        If BinaryValue.Length = 0 Then
                            Value = ""
                        Else
                            Dim Separator As Integer = 0
                            For Each B As Byte In BinaryValue
                                HexadecimalValue.AppendFormat("{0:x2},", B)

                                Dim Length As Integer = ValueName.Length + HexadecimalValue.Length + 7

								If Separator = 72 OrElse Length = 78 OrElse Length = 79 OrElse Length = 80 Then
                                    HexadecimalValue.Append("\"c & Environment.NewLine & "  ")
                                    Separator = 0
                                Else
                                    Separator += 3
                                End If
                            Next

							HexadecimalValue.Length -= 1
                            Value = HexadecimalValue.ToString
                        End If
                    Case RegistryValueKind.QWord
                        Pattern &= "hex(b):{1}"
                        Value = Convert.ToInt64(RawValue).ToString("x16")

						For i As Integer = Value.Length - 2 To 0 Step -2
                            HexadecimalValue.Append(Value, i, 2)

                            If i > 0 Then
                                HexadecimalValue.Append(","c)
                            End If
                        Next

						While HexadecimalValue.Length < 23
                            HexadecimalValue.Append(",00")
                        End While

						Value = HexadecimalValue.ToString
                    Case RegistryValueKind.MultiString
                        Pattern &= "hex(7):{1},00,00"

						Dim MultiStringValue As String() = RawValue
                        Value = ConvertValuetoHexadecimal(String.Join(Environment.NewLine, MultiStringValue), ValueName.Length)
                    Case RegistryValueKind.ExpandString
                        Pattern &= "hex(2):{1}"
                        Value = ConvertValuetoHexadecimal(RawValue, ValueName.Length)
                    Case RegistryValueKind.None
                        Pattern &= "hex(0):"
                    Case Else
                        MessageBox.Show(String.Format("New type of Registry value detected: {0}; Key: {1}; Value name: {2}", ValueType, Key, ValueName))
                End Select

				File.AppendAllText(RegFilePath, Environment.NewLine & String.Format(Pattern, ValueName, Value), Encoding.Unicode)
            Next
        End If

        If ExportationMode.Equals("/r", StringComparison.OrdinalIgnoreCase) Then
            Dim SubKeys As String() = Key.GetSubKeyNames()

			If SubKeys IsNot Nothing Then
                For Each SubKey As String In SubKeys
                    ExportSubKeys(Key.OpenSubKey(SubKey), "/r", Filter, FilteredItem)
                Next
            End If
        End If
    End Sub

    Private Shared Function ConvertValuetoHexadecimal(Value As String, ValueNameLength As Integer)
        Dim Separator As Integer = 0
        Dim HexadecimalValue As New StringBuilder

        For Each C As Char In Value
            HexadecimalValue.AppendFormat("{0:x2},", Convert.ToByte(C))

            Dim Length As Integer = ValueNameLength + HexadecimalValue.Length + 10

            If Separator = 72 OrElse Length = 78 OrElse Length = 79 OrElse Length = 80 Then
                HexadecimalValue.Append("\"c & Environment.NewLine & "  ")
                Separator = 0
            Else
                HexadecimalValue.Append("00,")
                Separator += 6
            End If
        Next

        Return HexadecimalValue.Append("00,00").ToString
    End Function

    Private Function RegFile()
        Dim FolderPath As String = String.Empty

        If Not String.IsNullOrEmpty(DestinationDirectoryTextBox.Text) Then
            FolderPath = DestinationDirectoryTextBox.Text
        ElseIf Not String.IsNullOrEmpty(FolderBrowserDialog.SelectedPath) Then
            FolderPath = FolderBrowserDialog.SelectedPath
        End If

        Return Path.Combine(FolderPath, "Settings.reg")
    End Function

    Private Sub OptionsButton_Click(sender As Object, e As EventArgs) Handles OptionsButton.Click
        ExportWizardPanel.Visible = False
        CustomExportPanel.Visible = False
        OptionsPanel.Visible = True

        If String.IsNullOrEmpty(ThemeComboBox.SelectedItem) Then
            ThemeComboBox.SelectedItem = "Classic Theme"
        End If
    End Sub

    Private Sub ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThemeComboBox.SelectedIndexChanged
        Select Case ThemeComboBox.Text
            Case "Classic Theme"
                Panel.BackColor = Color.Gray
            Case "Light Theme"
                Panel.BackColor = Color.White
            Case "Dark Theme"
                Panel.BackColor = Color.Black
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ScanButton.Click
        SoftwareListView.Items.Clear()
        Dim SoftwareList As New List(Of String)

        For Each Hive As RegistryHive In {RegistryHive.CurrentUser, RegistryHive.LocalMachine}
            For Each UninstallKey As String In {"SOFTWARE", "SOFTWARE\WOW6432Node"}
                Using Key As RegistryKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default).OpenSubKey(UninstallKey & "\Microsoft\Windows\CurrentVersion\Uninstall")
                    If Key IsNot Nothing Then
                        For Each SubKeyName As String In Key.GetSubKeyNames()
                            Using SubKey As RegistryKey = Key.OpenSubKey(SubKeyName)
                                Dim Value As String = SubKey.GetValue("DisplayName")

                                If Not String.IsNullOrEmpty(Value) AndAlso SubKey.GetValue("SystemComponent") Is Nothing Then
                                    SoftwareList.Add(Value)
                                End If
                            End Using
                        Next
                    End If
                End Using
            Next
        Next

        SoftwareList.Sort()
        For Each Item In SoftwareList
            SoftwareListView.Items.Add(Item)
        Next
    End Sub

    Private Sub WinExport_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ExportConfiguration(UserConfigurationFile)
    End Sub

End Class