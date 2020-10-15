<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fZonas
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grdDatos = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colFechaHora = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVelocidad = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colRumbo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colKilometraje = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colEvento = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCalle = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colLatitud = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colLongitud = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPunto = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colGPS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNivelBateria = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVoltajeBateria = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVoltajeAlimentacion = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colZonas = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAlias = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdDatos
        '
        Me.grdDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdDatos.Location = New System.Drawing.Point(0, 0)
        Me.grdDatos.LookAndFeel.SkinName = "Office 2010 Blue"
        Me.grdDatos.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.grdDatos.MainView = Me.GridView1
        Me.grdDatos.Name = "grdDatos"
        Me.grdDatos.Size = New System.Drawing.Size(1000, 261)
        Me.grdDatos.TabIndex = 2
        Me.grdDatos.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colAlias, Me.colFechaHora, Me.colVelocidad, Me.colRumbo, Me.colKilometraje, Me.colEvento, Me.colCalle, Me.colLatitud, Me.colLongitud, Me.colPunto, Me.colGPS, Me.colNivelBateria, Me.colVoltajeBateria, Me.colVoltajeAlimentacion, Me.colZonas})
        Me.GridView1.GridControl = Me.grdDatos
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PaintStyleName = "Style3D"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colFechaHora, DevExpress.Data.ColumnSortOrder.Descending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colAlias, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colFechaHora
        '
        Me.colFechaHora.Caption = "Fecha Hora"
        Me.colFechaHora.FieldName = "Date_Time"
        Me.colFechaHora.Name = "colFechaHora"
        Me.colFechaHora.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value
        Me.colFechaHora.Visible = True
        Me.colFechaHora.VisibleIndex = 1
        Me.colFechaHora.Width = 120
        '
        'colVelocidad
        '
        Me.colVelocidad.Caption = "Velocidad"
        Me.colVelocidad.FieldName = "Speed"
        Me.colVelocidad.Name = "colVelocidad"
        Me.colVelocidad.Visible = True
        Me.colVelocidad.VisibleIndex = 2
        Me.colVelocidad.Width = 78
        '
        'colRumbo
        '
        Me.colRumbo.Caption = "Rumbo"
        Me.colRumbo.FieldName = "Heading"
        Me.colRumbo.Name = "colRumbo"
        Me.colRumbo.Visible = True
        Me.colRumbo.VisibleIndex = 3
        Me.colRumbo.Width = 78
        '
        'colKilometraje
        '
        Me.colKilometraje.Caption = "Kilometraje"
        Me.colKilometraje.FieldName = "Kilometraje"
        Me.colKilometraje.Name = "colKilometraje"
        Me.colKilometraje.Visible = True
        Me.colKilometraje.VisibleIndex = 8
        Me.colKilometraje.Width = 49
        '
        'colEvento
        '
        Me.colEvento.Caption = "Evento"
        Me.colEvento.FieldName = "DEvento"
        Me.colEvento.Name = "colEvento"
        Me.colEvento.Visible = True
        Me.colEvento.VisibleIndex = 6
        Me.colEvento.Width = 130
        '
        'colCalle
        '
        Me.colCalle.Caption = "Calle"
        Me.colCalle.FieldName = "Calle"
        Me.colCalle.Name = "colCalle"
        Me.colCalle.Visible = True
        Me.colCalle.VisibleIndex = 7
        Me.colCalle.Width = 200
        '
        'colLatitud
        '
        Me.colLatitud.Caption = "Latitud"
        Me.colLatitud.FieldName = "Latitude"
        Me.colLatitud.Name = "colLatitud"
        Me.colLatitud.Visible = True
        Me.colLatitud.VisibleIndex = 4
        Me.colLatitud.Width = 78
        '
        'colLongitud
        '
        Me.colLongitud.Caption = "Longitud"
        Me.colLongitud.FieldName = "Loogitude"
        Me.colLongitud.Name = "colLongitud"
        Me.colLongitud.Visible = True
        Me.colLongitud.VisibleIndex = 5
        Me.colLongitud.Width = 78
        '
        'colPunto
        '
        Me.colPunto.Caption = "Punto Cercano"
        Me.colPunto.FieldName = "Pto. Cercano"
        Me.colPunto.Name = "colPunto"
        Me.colPunto.Visible = True
        Me.colPunto.VisibleIndex = 9
        Me.colPunto.Width = 49
        '
        'colGPS
        '
        Me.colGPS.Caption = "Estado GPS"
        Me.colGPS.FieldName = "EstadoGPS"
        Me.colGPS.Name = "colGPS"
        Me.colGPS.Visible = True
        Me.colGPS.VisibleIndex = 10
        Me.colGPS.Width = 49
        '
        'colNivelBateria
        '
        Me.colNivelBateria.Caption = "Nivel Bateria"
        Me.colNivelBateria.FieldName = "NIvelBateria"
        Me.colNivelBateria.Name = "colNivelBateria"
        Me.colNivelBateria.Visible = True
        Me.colNivelBateria.VisibleIndex = 11
        Me.colNivelBateria.Width = 49
        '
        'colVoltajeBateria
        '
        Me.colVoltajeBateria.Caption = "Voltaje Bateria"
        Me.colVoltajeBateria.FieldName = "VoltajeBateria"
        Me.colVoltajeBateria.Name = "colVoltajeBateria"
        Me.colVoltajeBateria.Visible = True
        Me.colVoltajeBateria.VisibleIndex = 12
        Me.colVoltajeBateria.Width = 49
        '
        'colVoltajeAlimentacion
        '
        Me.colVoltajeAlimentacion.Caption = "Voltaje Alimentacion"
        Me.colVoltajeAlimentacion.FieldName = "VoltajeAlimentacion"
        Me.colVoltajeAlimentacion.Name = "colVoltajeAlimentacion"
        Me.colVoltajeAlimentacion.Visible = True
        Me.colVoltajeAlimentacion.VisibleIndex = 13
        Me.colVoltajeAlimentacion.Width = 67
        '
        'colZonas
        '
        Me.colZonas.Caption = "Zona"
        Me.colZonas.FieldName = "Zona"
        Me.colZonas.Name = "colZonas"
        Me.colZonas.Visible = True
        Me.colZonas.VisibleIndex = 14
        '
        'colAlias
        '
        Me.colAlias.Caption = "Alias"
        Me.colAlias.FieldName = "VID"
        Me.colAlias.Name = "colAlias"
        Me.colAlias.Visible = True
        Me.colAlias.VisibleIndex = 0
        '
        'fZonas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1000, 261)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fZonas"
        Me.Text = "fZonas"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colFechaHora As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVelocidad As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colRumbo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colKilometraje As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEvento As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCalle As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLatitud As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLongitud As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPunto As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colGPS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNivelBateria As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVoltajeBateria As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVoltajeAlimentacion As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colZonas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAlias As DevExpress.XtraGrid.Columns.GridColumn
End Class
