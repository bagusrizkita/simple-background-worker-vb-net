Imports System.Threading
Imports MySql.Data.MySqlClient
Imports System.ComponentModel

Public Class Form1
    Dim iMax As Integer = 5 'iterasi maksimal
    Dim aMax As Integer = 5 'iterasi maksimal
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'untuk mencegah error saat background proses mengubah UI
        Control.CheckForIllegalCrossThreadCalls = False

        With bw1
            .WorkerReportsProgress = True 'catat nilai progress
            .WorkerSupportsCancellation = True 'supaya bisa cancel
        End With
        With bw2
            .WorkerReportsProgress = True 'catat nilai progress
            .WorkerSupportsCancellation = True 'supaya bisa cancel
        End With

        'setting nilai maksimum progress bar
        ProgressBar1.Maximum = iMax
        ProgressBar2.Maximum = aMax

    End Sub
    Private Sub btnBarang_Click(sender As Object, e As EventArgs) Handles btnBarang.Click
        bw2.RunWorkerAsync()
    End Sub
    Sub DataKaryawan()
        Call bukaDB()
        DA = New MySqlDataAdapter("SELECT * FROM karyawan", Conn)
        DS = New DataSet
        DA.Fill(DS, "karyawan")
        DataGridView1.DataSource = DS.Tables("karyawan")
        DataGridView1.ReadOnly = True
    End Sub
    Sub DataBarang()
        Call bukaDB()
        DA = New MySqlDataAdapter("SELECT * FROM barang", Conn)
        DS = New DataSet
        DA.Fill(DS, "barang")
        DataGridView2.DataSource = DS.Tables("barang")
        DataGridView2.ReadOnly = True
    End Sub

    Private Sub btnKaryawan_Click(sender As Object, e As EventArgs) Handles btnKaryawan.Click
        bw1.RunWorkerAsync()
    End Sub

    Private Sub bw1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bw1.DoWork
        For i = 0 To iMax
            'handle jika ada cancel
            If bw1.CancellationPending = True Then
                e.Cancel = True
                Exit For
            Else
                'proses normal tanpa cancel
                lblProgres1.Text = i 'menampilkan iterasi ke pada textbox
                'mengirim progress iterasi
                bw1.ReportProgress(i)
                'supaya proses berhenti sejenak 1000 milidetik = 1 detik
                System.Threading.Thread.Sleep(1000)
            End If
        Next

    End Sub
    Private Sub bw1_ProgressChanged(sender As Object,
            e As ProgressChangedEventArgs) _
            Handles bw1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage

    End Sub
    Private Sub bw1_RunWorkerCompleted(sender As Object,
            e As RunWorkerCompletedEventArgs) _
            Handles bw1.RunWorkerCompleted
        MsgBox("Proses Data Karyawan Selesai")
        Call DataKaryawan()

    End Sub

    Private Sub bw2_DoWork(sender As Object, e As DoWorkEventArgs) Handles bw2.DoWork
        For a = 0 To aMax
            'handle jika ada cancel
            If bw2.CancellationPending = True Then
                e.Cancel = True
                Exit For
            Else
                'proses normal tanpa cancel
                lblProgres2.Text = a 'menampilkan iterasi ke pada textbox
                'mengirim progress iterasi
                bw2.ReportProgress(a)
                'supaya proses berhenti sejenak 1000 milidetik = 1 detik
                System.Threading.Thread.Sleep(1000)
            End If
        Next
    End Sub
    Private Sub bw2_ProgressChanged(sender As Object,
            e As ProgressChangedEventArgs) _
            Handles bw2.ProgressChanged
        ProgressBar2.Value = e.ProgressPercentage

    End Sub
    Private Sub bw2_RunWorkerCompleted(sender As Object,
            e As RunWorkerCompletedEventArgs) _
            Handles bw2.RunWorkerCompleted
        MsgBox("Proses Data Barang Selesai")
        Call DataBarang()

    End Sub
End Class