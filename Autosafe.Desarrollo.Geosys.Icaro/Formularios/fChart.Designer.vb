<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fChart
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim DataPoint1 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0R, 1.0R)
        Dim DataPoint2 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0R, 1.0R)
        Dim DataPoint3 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0R, 1.0R)
        Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Me.oChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.oChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'oChart
        '
        ChartArea1.Name = "ChartArea1"
        Me.oChart.ChartAreas.Add(ChartArea1)
        Me.oChart.Dock = System.Windows.Forms.DockStyle.Fill
        Legend1.Name = "Legend1"
        Me.oChart.Legends.Add(Legend1)
        Me.oChart.Location = New System.Drawing.Point(0, 0)
        Me.oChart.Name = "oChart"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.Legend = "Legend1"
        Series1.Name = "Calificacion"
        DataPoint1.Color = System.Drawing.Color.Green
        DataPoint1.Label = "Bueno"
        DataPoint2.Color = System.Drawing.Color.Yellow
        DataPoint2.Label = "Regular"
        DataPoint3.Color = System.Drawing.Color.Red
        DataPoint3.Label = "Malo"
        Series1.Points.Add(DataPoint1)
        Series1.Points.Add(DataPoint2)
        Series1.Points.Add(DataPoint3)
        Me.oChart.Series.Add(Series1)
        Me.oChart.Size = New System.Drawing.Size(259, 185)
        Me.oChart.TabIndex = 0
        Me.oChart.Text = "Chart"
        Title1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Title1.Name = "Titulo"
        Title1.Text = "Titulo de Grafico"
        Me.oChart.Titles.Add(Title1)
        '
        'fChart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(259, 185)
        Me.Controls.Add(Me.oChart)
        Me.Name = "fChart"
        Me.Text = "fChart"
        CType(Me.oChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents oChart As System.Windows.Forms.DataVisualization.Charting.Chart
End Class
