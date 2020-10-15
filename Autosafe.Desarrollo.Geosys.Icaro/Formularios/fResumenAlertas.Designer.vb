<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fResumenAlertas
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
        Me.grdDatos = New DevExpress.XtraPivotGrid.PivotGridControl()
        Me.DsEventos1 = New Icaro.dsEventos()
        Me.fieldId1 = New DevExpress.XtraPivotGrid.PivotGridField()
        Me.fieldFecha1 = New DevExpress.XtraPivotGrid.PivotGridField()
        Me.fieldEventos1 = New DevExpress.XtraPivotGrid.PivotGridField()
        Me.fieldEvento1 = New DevExpress.XtraPivotGrid.PivotGridField()
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsEventos1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdDatos
        '
        Me.grdDatos.DataMember = "Eventos"
        Me.grdDatos.DataSource = Me.DsEventos1
        Me.grdDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdDatos.Fields.AddRange(New DevExpress.XtraPivotGrid.PivotGridField() {Me.fieldId1, Me.fieldFecha1, Me.fieldEventos1, Me.fieldEvento1})
        Me.grdDatos.Location = New System.Drawing.Point(0, 0)
        Me.grdDatos.Name = "grdDatos"
        Me.grdDatos.Size = New System.Drawing.Size(595, 309)
        Me.grdDatos.TabIndex = 0
        '
        'DsEventos1
        '
        Me.DsEventos1.DataSetName = "dsEventos"
        Me.DsEventos1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'fieldId1
        '
        Me.fieldId1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
        Me.fieldId1.AreaIndex = 0
        Me.fieldId1.Caption = "Alias"
        Me.fieldId1.FieldName = "Id"
        Me.fieldId1.Name = "fieldId1"
        '
        'fieldFecha1
        '
        Me.fieldFecha1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea
        Me.fieldFecha1.AreaIndex = 0
        Me.fieldFecha1.FieldName = "Fecha"
        Me.fieldFecha1.Name = "fieldFecha1"
        '
        'fieldEventos1
        '
        Me.fieldEventos1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea
        Me.fieldEventos1.AreaIndex = 0
        Me.fieldEventos1.Caption = "Cantidad"
        Me.fieldEventos1.FieldName = "Eventos"
        Me.fieldEventos1.Name = "fieldEventos1"
        '
        'fieldEvento1
        '
        Me.fieldEvento1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea
        Me.fieldEvento1.AreaIndex = 1
        Me.fieldEvento1.Caption = "Alertas"
        Me.fieldEvento1.FieldName = "Evento"
        Me.fieldEvento1.Name = "fieldEvento1"
        '
        'fResumenAlertas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(595, 309)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fResumenAlertas"
        Me.Text = "Resumen de Alertas"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsEventos1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraPivotGrid.PivotGridControl
    Friend WithEvents DsEventos1 As dsEventos
    Friend WithEvents fieldId1 As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldFecha1 As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEventos1 As DevExpress.XtraPivotGrid.PivotGridField
    Friend WithEvents fieldEvento1 As DevExpress.XtraPivotGrid.PivotGridField
End Class
