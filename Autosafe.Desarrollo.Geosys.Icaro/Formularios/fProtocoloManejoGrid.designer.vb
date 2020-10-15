<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fProtocoloManejoGrid
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
        Me.grdDatos = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colFecha = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPlaca = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVelocidadMax = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTotalvelocidadMax = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTiempoVelocidadMax = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colDistanciaMax = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTiempoTotalConducción = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNumeroVecesVelocidad = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNumeroFrenadasViolentas = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNumeroAceleracionesViolentas = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNumeroGirosViolentos = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNumeroImpactosViolentos = New DevExpress.XtraGrid.Columns.GridColumn()
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
        Me.grdDatos.Size = New System.Drawing.Size(1055, 291)
        Me.grdDatos.TabIndex = 0
        Me.grdDatos.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colFecha, Me.colPlaca, Me.colVelocidadMax, Me.colTotalvelocidadMax, Me.colTiempoVelocidadMax, Me.colDistanciaMax, Me.colTiempoTotalConducción, Me.colNumeroVecesVelocidad, Me.colNumeroFrenadasViolentas, Me.colNumeroAceleracionesViolentas, Me.colNumeroGirosViolentos, Me.colNumeroImpactosViolentos})
        Me.GridView1.GridControl = Me.grdDatos
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PaintStyleName = "Style3D"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colFecha, DevExpress.Data.ColumnSortOrder.Descending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPlaca, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colFecha
        '
        Me.colFecha.Caption = "FECHA"
        Me.colFecha.FieldName = "fecha"
        Me.colFecha.Name = "colFecha"
        Me.colFecha.Visible = True
        Me.colFecha.VisibleIndex = 0
        '
        'colPlaca
        '
        Me.colPlaca.Caption = "PLACA"
        Me.colPlaca.FieldName = "placa"
        Me.colPlaca.Name = "colPlaca"
        Me.colPlaca.Visible = True
        Me.colPlaca.VisibleIndex = 1
        '
        'colVelocidadMax
        '
        Me.colVelocidadMax.Caption = "Velocidad máxima de Manejo (km/h)"
        Me.colVelocidadMax.FieldName = "velocidadMax"
        Me.colVelocidadMax.Name = "colVelocidadMax"
        Me.colVelocidadMax.Visible = True
        Me.colVelocidadMax.VisibleIndex = 2
        '
        'colTotalvelocidadMax
        '
        Me.colTotalvelocidadMax.Caption = "Cantidad de veces que llegó a esa Velocidad Máxima"
        Me.colTotalvelocidadMax.FieldName = "totalvelocidadMax"
        Me.colTotalvelocidadMax.Name = "colTotalvelocidadMax"
        Me.colTotalvelocidadMax.Visible = True
        Me.colTotalvelocidadMax.VisibleIndex = 3
        '
        'colTiempoVelocidadMax
        '
        Me.colTiempoVelocidadMax.Caption = "Duración Total de la Velocidad Máxima (hh:mm:ss)"
        Me.colTiempoVelocidadMax.FieldName = "tiempoVelocidadMax"
        Me.colTiempoVelocidadMax.Name = "colTiempoVelocidadMax"
        Me.colTiempoVelocidadMax.Visible = True
        Me.colTiempoVelocidadMax.VisibleIndex = 4
        '
        'colDistanciaMax
        '
        Me.colDistanciaMax.Caption = "Distancia recorrida durante la Velocidad Máxima (mts)"
        Me.colDistanciaMax.FieldName = "distanciaMax"
        Me.colDistanciaMax.Name = "colDistanciaMax"
        Me.colDistanciaMax.Visible = True
        Me.colDistanciaMax.VisibleIndex = 5
        '
        'colTiempoTotalConducción
        '
        Me.colTiempoTotalConducción.Caption = "Tiempo Total de Conducción (hh:mm:ss)"
        Me.colTiempoTotalConducción.FieldName = "TiempoTotalConducción"
        Me.colTiempoTotalConducción.Name = "colTiempoTotalConducción"
        Me.colTiempoTotalConducción.Visible = True
        Me.colTiempoTotalConducción.VisibleIndex = 6
        '
        'colNumeroVecesVelocidad
        '
        Me.colNumeroVecesVelocidad.Caption = "Número de Veces que superó la velocidad en la Vía"
        Me.colNumeroVecesVelocidad.FieldName = "NumeroVecesVelocidad"
        Me.colNumeroVecesVelocidad.Name = "colNumeroVecesVelocidad"
        Me.colNumeroVecesVelocidad.Visible = True
        Me.colNumeroVecesVelocidad.VisibleIndex = 7
        '
        'colNumeroFrenadasViolentas
        '
        Me.colNumeroFrenadasViolentas.Caption = "Número de Frenadas Violentas"
        Me.colNumeroFrenadasViolentas.FieldName = "NumeroFrenadasViolentas"
        Me.colNumeroFrenadasViolentas.Name = "colNumeroFrenadasViolentas"
        Me.colNumeroFrenadasViolentas.Visible = True
        Me.colNumeroFrenadasViolentas.VisibleIndex = 8
        '
        'colNumeroAceleracionesViolentas
        '
        Me.colNumeroAceleracionesViolentas.Caption = "Número de Aceleraciones Violentas"
        Me.colNumeroAceleracionesViolentas.FieldName = "NumeroAceleracionesViolentas"
        Me.colNumeroAceleracionesViolentas.Name = "colNumeroAceleracionesViolentas"
        Me.colNumeroAceleracionesViolentas.Visible = True
        Me.colNumeroAceleracionesViolentas.VisibleIndex = 9
        '
        'colNumeroGirosViolentos
        '
        Me.colNumeroGirosViolentos.Caption = "Número de Giros Violentos"
        Me.colNumeroGirosViolentos.FieldName = "NumeroGirosViolentos"
        Me.colNumeroGirosViolentos.Name = "colNumeroGirosViolentos"
        Me.colNumeroGirosViolentos.Visible = True
        Me.colNumeroGirosViolentos.VisibleIndex = 10
        '
        'colNumeroImpactosViolentos
        '
        Me.colNumeroImpactosViolentos.Caption = "Número de Impactos Violentos"
        Me.colNumeroImpactosViolentos.FieldName = "NumeroImpactosViolentos"
        Me.colNumeroImpactosViolentos.Name = "colNumeroImpactosViolentos"
        Me.colNumeroImpactosViolentos.Visible = True
        Me.colNumeroImpactosViolentos.VisibleIndex = 11
        '
        'fProtocoloManejoGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1055, 291)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fProtocoloManejoGrid"
        Me.Text = "fGrid"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colFecha As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDistanciaMax As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTiempoTotalConducción As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPlaca As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNumeroVecesVelocidad As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNumeroFrenadasViolentas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNumeroAceleracionesViolentas As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNumeroGirosViolentos As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotalvelocidadMax As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNumeroImpactosViolentos As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVelocidadMax As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTiempoVelocidadMax As DevExpress.XtraGrid.Columns.GridColumn
End Class
