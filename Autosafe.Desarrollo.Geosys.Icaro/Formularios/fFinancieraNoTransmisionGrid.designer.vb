<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fFinancieraNoTransmisionGrid
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
        Me.colPLACA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colNRO_OPERACION = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFINANCIERA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFINANCIADO_POR = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colID_CLIENTE_NUEVO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colVID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colESTADO_DISPOSITIVO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCLIENTE = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMOTOR = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCHASIS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMARCA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMODELO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colESTADO_AUTOMOVIL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFECHA_FIN_COB = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colEMAIL = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTELEFONOS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colDIRECCION = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFECHA_UT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFECHA_OT = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colFECHA = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colDIAS = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCODIGO_VEHICULO = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPRODUCTO = New DevExpress.XtraGrid.Columns.GridColumn()
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
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPLACA, Me.colNRO_OPERACION, Me.colFINANCIERA, Me.colFINANCIADO_POR, Me.colID_CLIENTE_NUEVO, Me.colVID, Me.colESTADO_DISPOSITIVO, Me.colCLIENTE, Me.colMOTOR, Me.colCHASIS, Me.colMARCA, Me.colMODELO, Me.colESTADO_AUTOMOVIL, Me.colFECHA_FIN_COB, Me.colEMAIL, Me.colTELEFONOS, Me.colDIRECCION, Me.colFECHA_UT, Me.colFECHA_OT, Me.colFECHA, Me.colDIAS, Me.colCODIGO_VEHICULO, Me.colPRODUCTO})
        Me.GridView1.GridControl = Me.grdDatos
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PaintStyleName = "Style3D"
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPLACA, DevExpress.Data.ColumnSortOrder.Descending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colNRO_OPERACION, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colPLACA
        '
        Me.colPLACA.Caption = "PLACA"
        Me.colPLACA.FieldName = "PLACA"
        Me.colPLACA.Name = "colPLACA"
        Me.colPLACA.Visible = True
        Me.colPLACA.VisibleIndex = 0
        '
        'colNRO_OPERACION
        '
        Me.colNRO_OPERACION.Caption = "NRO_OPERACION"
        Me.colNRO_OPERACION.FieldName = "NRO_OPERACION"
        Me.colNRO_OPERACION.Name = "colNRO_OPERACION"
        Me.colNRO_OPERACION.Visible = True
        Me.colNRO_OPERACION.VisibleIndex = 1
        '
        'colFINANCIERA
        '
        Me.colFINANCIERA.Caption = "FINANCIERA"
        Me.colFINANCIERA.FieldName = "FINANCIERA"
        Me.colFINANCIERA.Name = "colFINANCIERA"
        Me.colFINANCIERA.Visible = True
        Me.colFINANCIERA.VisibleIndex = 17
        '
        'colFINANCIADO_POR
        '
        Me.colFINANCIADO_POR.Caption = "FINANCIADO_POR"
        Me.colFINANCIADO_POR.FieldName = "FINANCIADO_POR"
        Me.colFINANCIADO_POR.Name = "colFINANCIADO_POR"
        Me.colFINANCIADO_POR.Visible = True
        Me.colFINANCIADO_POR.VisibleIndex = 12
        '
        'colID_CLIENTE_NUEVO
        '
        Me.colID_CLIENTE_NUEVO.Caption = "ID_CLIENTE_NUEVO"
        Me.colID_CLIENTE_NUEVO.FieldName = "ID_CLIENTE_NUEVO"
        Me.colID_CLIENTE_NUEVO.Name = "colID_CLIENTE_NUEVO"
        Me.colID_CLIENTE_NUEVO.Visible = True
        Me.colID_CLIENTE_NUEVO.VisibleIndex = 18
        '
        'colVID
        '
        Me.colVID.Caption = "VID"
        Me.colVID.FieldName = "VID"
        Me.colVID.Name = "colVID"
        Me.colVID.Visible = True
        Me.colVID.VisibleIndex = 2
        '
        'colESTADO_DISPOSITIVO
        '
        Me.colESTADO_DISPOSITIVO.Caption = "ESTADO_DISPOSITIVO"
        Me.colESTADO_DISPOSITIVO.FieldName = "ESTADO_DISPOSITIVO"
        Me.colESTADO_DISPOSITIVO.Name = "colESTADO_DISPOSITIVO"
        Me.colESTADO_DISPOSITIVO.Visible = True
        Me.colESTADO_DISPOSITIVO.VisibleIndex = 3
        '
        'colCLIENTE
        '
        Me.colCLIENTE.Caption = "CLIENTE"
        Me.colCLIENTE.FieldName = "CLIENTE"
        Me.colCLIENTE.Name = "colCLIENTE"
        Me.colCLIENTE.Visible = True
        Me.colCLIENTE.VisibleIndex = 7
        '
        'colMOTOR
        '
        Me.colMOTOR.Caption = "MOTOR"
        Me.colMOTOR.FieldName = "MOTOR"
        Me.colMOTOR.Name = "colMOTOR"
        Me.colMOTOR.Visible = True
        Me.colMOTOR.VisibleIndex = 9
        '
        'colCHASIS
        '
        Me.colCHASIS.Caption = "CHASIS"
        Me.colCHASIS.FieldName = "CHASIS"
        Me.colCHASIS.Name = "colCHASIS"
        Me.colCHASIS.Visible = True
        Me.colCHASIS.VisibleIndex = 8
        '
        'colMARCA
        '
        Me.colMARCA.Caption = "MARCA"
        Me.colMARCA.FieldName = "MARCA"
        Me.colMARCA.Name = "colMARCA"
        Me.colMARCA.Visible = True
        Me.colMARCA.VisibleIndex = 11
        '
        'colMODELO
        '
        Me.colMODELO.Caption = "MODELO"
        Me.colMODELO.FieldName = "MODELO"
        Me.colMODELO.Name = "colMODELO"
        Me.colMODELO.Visible = True
        Me.colMODELO.VisibleIndex = 16
        '
        'colESTADO_AUTOMOVIL
        '
        Me.colESTADO_AUTOMOVIL.Caption = "ESTADO_AUTOMOVIL"
        Me.colESTADO_AUTOMOVIL.FieldName = "ESTADO_AUTOMOVIL"
        Me.colESTADO_AUTOMOVIL.Name = "colESTADO_AUTOMOVIL"
        Me.colESTADO_AUTOMOVIL.Visible = True
        Me.colESTADO_AUTOMOVIL.VisibleIndex = 4
        '
        'colFECHA_FIN_COB
        '
        Me.colFECHA_FIN_COB.Caption = "FECHA_FIN_COB"
        Me.colFECHA_FIN_COB.FieldName = "FECHA_FIN_COB"
        Me.colFECHA_FIN_COB.Name = "colFECHA_FIN_COB"
        Me.colFECHA_FIN_COB.Visible = True
        Me.colFECHA_FIN_COB.VisibleIndex = 10
        '
        'colEMAIL
        '
        Me.colEMAIL.Caption = "EMAIL"
        Me.colEMAIL.FieldName = "EMAIL"
        Me.colEMAIL.Name = "colEMAIL"
        Me.colEMAIL.Visible = True
        Me.colEMAIL.VisibleIndex = 6
        '
        'colTELEFONOS
        '
        Me.colTELEFONOS.Caption = "TELEFONOS"
        Me.colTELEFONOS.FieldName = "TELEFONOS"
        Me.colTELEFONOS.Name = "colTELEFONOS"
        Me.colTELEFONOS.Visible = True
        Me.colTELEFONOS.VisibleIndex = 5
        '
        'colDIRECCION
        '
        Me.colDIRECCION.Caption = "DIRECCION"
        Me.colDIRECCION.FieldName = "DIRECCION"
        Me.colDIRECCION.Name = "colDIRECCION"
        Me.colDIRECCION.Visible = True
        Me.colDIRECCION.VisibleIndex = 13
        '
        'colFECHA_UT
        '
        Me.colFECHA_UT.Caption = "FECHA_UT"
        Me.colFECHA_UT.FieldName = "FECHA_UT"
        Me.colFECHA_UT.Name = "colFECHA_UT"
        Me.colFECHA_UT.Visible = True
        Me.colFECHA_UT.VisibleIndex = 14
        '
        'colFECHA_OT
        '
        Me.colFECHA_OT.Caption = "FECHA_OT"
        Me.colFECHA_OT.FieldName = "FECHA_OT"
        Me.colFECHA_OT.Name = "colFECHA_OT"
        Me.colFECHA_OT.Visible = True
        Me.colFECHA_OT.VisibleIndex = 15
        '
        'colFECHA
        '
        Me.colFECHA.Caption = "FECHA"
        Me.colFECHA.FieldName = "FECHA"
        Me.colFECHA.Name = "colFECHA"
        Me.colFECHA.Visible = True
        Me.colFECHA.VisibleIndex = 19
        '
        'colDIAS
        '
        Me.colDIAS.Caption = "DIAS"
        Me.colDIAS.FieldName = "DIAS"
        Me.colDIAS.Name = "colDIAS"
        Me.colDIAS.Visible = True
        Me.colDIAS.VisibleIndex = 20
        '
        'colCODIGO_VEHICULO
        '
        Me.colCODIGO_VEHICULO.Caption = "CODIGO_VEHICULO"
        Me.colCODIGO_VEHICULO.FieldName = "CODIGO_VEHICULO"
        Me.colCODIGO_VEHICULO.Name = "colCODIGO_VEHICULO"
        Me.colCODIGO_VEHICULO.Visible = True
        Me.colCODIGO_VEHICULO.VisibleIndex = 21
        '
        'colPRODUCTO
        '
        Me.colPRODUCTO.Caption = "PRODUCTO"
        Me.colPRODUCTO.FieldName = "PRODUCTO"
        Me.colPRODUCTO.Name = "colPRODUCTO"
        Me.colPRODUCTO.Visible = True
        Me.colPRODUCTO.VisibleIndex = 22
        '
        'fFinancieraNoTransmisionGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1055, 291)
        Me.Controls.Add(Me.grdDatos)
        Me.Name = "fFinancieraNoTransmisionGrid"
        Me.Text = "fGrid"
        CType(Me.grdDatos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdDatos As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPLACA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colESTADO_DISPOSITIVO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNRO_OPERACION As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colESTADO_AUTOMOVIL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEMAIL As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCLIENTE As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMOTOR As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCHASIS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTELEFONOS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFECHA_FIN_COB As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMARCA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFINANCIADO_POR As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDIRECCION As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFECHA_UT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFECHA_OT As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMODELO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFINANCIERA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colID_CLIENTE_NUEVO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFECHA As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDIAS As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCODIGO_VEHICULO As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPRODUCTO As DevExpress.XtraGrid.Columns.GridColumn
End Class
