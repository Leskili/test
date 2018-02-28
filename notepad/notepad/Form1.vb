Public Class Form1
    Dim defaultSavePath As String = ""

    Private Sub tsmSave_Click(sender As Object, e As EventArgs) Handles tsmSave.Click
        'will automatically write txtbox.text to the file, unless no file has been saved to yet, otherwise does save as
        If (defaultSavePath.Length > 0) Then    'if there is a default save file, then save to it
            save(defaultSavePath)
        Else
            tsmSaveAs_Click(sender, e)
        End If
    End Sub

    Private Sub save(path As String)
        Using stream As New IO.StreamWriter(path)
            stream.WriteLine(txtBox.Text)
            stream.Close()
        End Using
    End Sub

    Private Sub tsmSaveAs_Click(sender As Object, e As EventArgs) Handles tsmSaveAs.Click
        Dim dlgSave = New SaveFileDialog
        dlgSave.Title = "Save File"
        dlgSave.Filter = "Text File | *.txt"
        dlgSave.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        dlgSave.ShowDialog()
        If (dlgSave.FileName.Length > 0) Then
            save(dlgSave.FileName)
        Else
            MessageBox.Show("File not saved.", "Error Saving", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub tsmOpen_Click(sender As Object, e As EventArgs) Handles tsmOpen.Click
        Dim dlgOpen = New OpenFileDialog
        dlgOpen.Title = "Save File"
        dlgOpen.Filter = "Text File | *.txt"
        dlgOpen.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        dlgOpen.ShowDialog()
        If (dlgOpen.FileName.Length > 0) Then
            Dim myFile = IO.File.OpenText(dlgOpen.FileName)
            txtBox.Text = myFile.ReadToEnd
            myFile.Close()
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtBox_TextChanged(sender As Object, e As EventArgs) Handles txtBox.TextChanged
        'updates word and sentence counts 
        Dim lstWords As List(Of String) = Split(txtBox.Text).ToList()   'creates a list of strings delimited by " "
        lstWords.RemoveAll(Function(value As String) value = "")    'removes from the list all empty strings
        statWordCount.Text = lstWords.Count.ToString()

        Dim lstSentences As List(Of String) = Split(txtBox.Text, ".").ToList()  'same as above, but with period
        lstSentences.RemoveAll(Function(value As String) value = "")
        statSentenceCount.Text = lstSentences.Count.ToString()
    End Sub

    Private Sub FindToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindToolStripMenuItem.Click
        findString(InputBox("Search for a string.", "Find"))    'searches for a string using an input box as the search key
    End Sub

    Private Sub FindNextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindNextToolStripMenuItem.Click
        findString()    'searches for a string using the most recently used search key
    End Sub

    Private Sub findString(Optional ByVal strFind As String = "")
        Dim text As String = txtBox.Text
        Static key As String   'static string, used as search key
        Static intLastStart As Integer = 1  'static int, tracks last index of found string

        If (strFind <> "") Then
            key = strFind  'if no argument passed, use most recent search key
        End If

        If (key = "") Then 'if there is no search key, get one from the user (happens when find next before find)
            key = InputBox("Search for a string.", "Find")
        End If

        Dim index As Integer = InStr(intLastStart, text, key)  'gets the index of the first occurrence of the key term
        If (index > 0) Then 'selects the key term if it exists in the txtbox
            txtBox.SelectionStart = index - 1
            txtBox.SelectionLength = key.Length
            intLastStart = index + key.Length   'updates the index to avoid finding the same part of the string over and over
        End If

    End Sub
End Class
