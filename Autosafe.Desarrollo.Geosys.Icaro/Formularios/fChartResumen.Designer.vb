<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fChartResumen
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
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
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
        Series1.BorderColor = System.Drawing.Color.Red
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100
        Series1.Color = System.Drawing.Color.Red
        Series1.IsValueShownAsLabel = True
        Series1.Legend = "Legend1"
        Series1.LegendText = "Malo"
        Series1.Name = "sMalo"
        Series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[String]
        Series2.BorderColor = System.Drawing.Color.Yellow
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100
        Series2.Color = System.Drawing.Color.Yellow
        Series2.IsValueShownAsLabel = True
        Series2.Legend = "Legend1"
        Series2.LegendText = "Regular"
        Series2.Name = "sRegular"
        Series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[String]
        Series3.BorderColor = System.Drawing.Color.Green
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100
        Series3.Color = System.Drawing.Color.Green
        Series3.IsValueShownAsLabel = True
        Series3.Legend = "Legend1"
        Series3.LegendText = "Bueno"
        Series3.Name = "sBueno"
        Series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[String]
        Me.oChart.Series.Add(Series1)
        Me.oChart.Series.Add(Series2)
        Me.oChart.Series.Add(Series3)
        Me.oChart.Size = New System.Drawing.Size(588, 377)
        Me.oChart.TabIndex = 0
        Me.oChart.Text = "Chart1"
        Title1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Title1.Name = "Evolucion Mensual de la Flota"
        Title1.Text = "Evolucion Mensual de la Flota"
        Me.oChart.Titles.Add(Title1)
        '
        'fChartResumen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(588, 377)
        Me.Controls.Add(Me.oChart)
        Me.Name = "fChartResumen"
        Me.Text = "fChartResumen"
        CType(Me.oChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents oChart As System.Windows.Forms.DataVisualization.Charting.Chart
End Class
