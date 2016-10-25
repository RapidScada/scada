
CREATE TABLE CmdType
( 
	CmdTypeID            int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_CmdType PRIMARY KEY (CmdTypeID ASC)
)
go



CREATE TABLE CmdVal
( 
	CmdValID             int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Val                  nvarchar(100)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_CmdVal PRIMARY KEY (CmdValID ASC)
)
go



CREATE TABLE CnlType
( 
	CnlTypeID            int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	ShtName              nvarchar(20)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_CnlType PRIMARY KEY (CnlTypeID ASC)
)
go



CREATE TABLE CommLine
( 
	CommLineNum          int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_CommLine PRIMARY KEY (CommLineNum ASC)
)
go



CREATE TABLE CtrlCnl
( 
	CtrlCnlNum           int  NOT NULL ,
	Active               bit  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	CmdTypeID            int  NOT NULL ,
	ObjNum               int  NULL ,
	KPNum                int  NULL ,
	CmdNum               int  NULL ,
	CmdValID             int  NULL ,
	FormulaUsed          bit  NOT NULL ,
	Formula              nvarchar(100)  NULL ,
	EvEnabled            bit  NOT NULL ,
	ModifiedDT           datetime  NOT NULL ,
	CONSTRAINT pk_CtrlCnl PRIMARY KEY (CtrlCnlNum ASC)
)
go



CREATE INDEX idx_CtrlCnl_KPNum ON CtrlCnl
( 
	KPNum                 ASC
)
go



CREATE INDEX idx_CtrlCnl_ObjNum ON CtrlCnl
( 
	ObjNum                ASC
)
go



CREATE TABLE EvType
( 
	CnlStatus            int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Color                nvarchar(20)  NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_EvType PRIMARY KEY (CnlStatus ASC)
)
go



CREATE TABLE Format
( 
	FormatID             int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	ShowNumber           bit  NOT NULL ,
	DecDigits            int  NULL ,
	CONSTRAINT pk_Format PRIMARY KEY (FormatID ASC)
)
go



CREATE TABLE Formula
( 
	Name                 nvarchar(50)  NOT NULL ,
	Source               nvarchar(1000)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_Formula PRIMARY KEY (Name ASC)
)
go



CREATE TABLE InCnl
( 
	CnlNum               int  NOT NULL ,
	Active               bit  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	CnlTypeID            int  NOT NULL ,
	ObjNum               int  NULL ,
	KPNum                int  NULL ,
	Signal               int  NULL ,
	FormulaUsed          bit  NOT NULL ,
	Formula              nvarchar(100)  NULL ,
	Averaging            bit  NOT NULL ,
	ParamID              int  NULL ,
	FormatID             int  NULL ,
	UnitID               int  NULL ,
	CtrlCnlNum           int  NULL ,
	EvEnabled            bit  NOT NULL ,
	EvSound              bit  NOT NULL ,
	EvOnChange           bit  NOT NULL ,
	EvOnUndef            bit  NOT NULL ,
	LimLowCrash          float  NULL ,
	LimLow               float  NULL ,
	LimHigh              float  NULL ,
	LimHighCrash         float  NULL ,
	ModifiedDT           datetime  NOT NULL ,
	CONSTRAINT pk_InCnl PRIMARY KEY (CnlNum ASC)
)
go



CREATE INDEX idx_InCnl_KPNum ON InCnl
( 
	KPNum                 ASC
)
go



CREATE INDEX idx_InCnl_ObjNum ON InCnl
( 
	ObjNum                ASC
)
go



CREATE TABLE Interface
( 
	ItfID                int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_Interface PRIMARY KEY (ItfID ASC)
)
go



CREATE TABLE KP
( 
	KPNum                int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	KPTypeID             int  NOT NULL ,
	Address              int  NULL ,
	CallNum              nvarchar(20)  NULL ,
	CommLineNum          int  NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_KP PRIMARY KEY (KPNum ASC)
)
go



CREATE TABLE KPType
( 
	KPTypeID             int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	DllFileName          nvarchar(20)  NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_KPType PRIMARY KEY (KPTypeID ASC)
)
go



CREATE TABLE Obj
( 
	ObjNum               int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_Obj PRIMARY KEY (ObjNum ASC)
)
go



CREATE TABLE Param
( 
	ParamID              int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Sign                 nvarchar(20)  NULL ,
	IconFileName         nvarchar(20)  NULL ,
	CONSTRAINT pk_Param PRIMARY KEY (ParamID ASC)
)
go



CREATE TABLE Right
( 
	ItfID                int  NOT NULL ,
	RoleID               int  NOT NULL ,
	ViewRight            bit  NOT NULL ,
	CtrlRight            bit  NOT NULL ,
	CONSTRAINT pk_Right PRIMARY KEY (ItfID ASC,RoleID ASC)
)
go



CREATE TABLE Role
( 
	RoleID               int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_Role PRIMARY KEY (RoleID ASC)
)
go



CREATE TABLE Unit
( 
	UnitID               int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Sign                 nvarchar(100)  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_Unit PRIMARY KEY (UnitID ASC)
)
go



CREATE TABLE User
( 
	UserID               int  NOT NULL ,
	Name                 nvarchar(50)  NOT NULL ,
	Password             nvarchar(20)  NULL ,
	RoleID               int  NOT NULL ,
	Descr                nvarchar(100)  NULL ,
	CONSTRAINT pk_User PRIMARY KEY (UserID ASC)
)
go




ALTER TABLE CtrlCnl
	ADD CONSTRAINT fk_CtrlCnl_ObjNum FOREIGN KEY (ObjNum) REFERENCES Obj(ObjNum)
go




ALTER TABLE CtrlCnl
	ADD CONSTRAINT fk_CtrlCnl_KPNum FOREIGN KEY (KPNum) REFERENCES KP(KPNum)
go




ALTER TABLE CtrlCnl
	ADD CONSTRAINT fk_CtrlCnl_CmdTypeID FOREIGN KEY (CmdTypeID) REFERENCES CmdType(CmdTypeID)
go




ALTER TABLE CtrlCnl
	ADD CONSTRAINT fk_CtrlCnl_CmdValID FOREIGN KEY (CmdValID) REFERENCES CmdVal(CmdValID)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_KPNum FOREIGN KEY (KPNum) REFERENCES KP(KPNum)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_ObjNum FOREIGN KEY (ObjNum) REFERENCES Obj(ObjNum)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_CnlTypeID FOREIGN KEY (CnlTypeID) REFERENCES CnlType(CnlTypeID)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_UnitID FOREIGN KEY (UnitID) REFERENCES Unit(UnitID)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_FormatID FOREIGN KEY (FormatID) REFERENCES Format(FormatID)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_ParamID FOREIGN KEY (ParamID) REFERENCES Param(ParamID)
go




ALTER TABLE InCnl
	ADD CONSTRAINT fk_InCnl_CtrlCnlNum FOREIGN KEY (CtrlCnlNum) REFERENCES CtrlCnl(CtrlCnlNum)
go




ALTER TABLE KP
	ADD CONSTRAINT fk_KP_KPTypeID FOREIGN KEY (KPTypeID) REFERENCES KPType(KPTypeID)
go




ALTER TABLE KP
	ADD CONSTRAINT fk_KP_CommLineNum FOREIGN KEY (CommLineNum) REFERENCES CommLine(CommLineNum)
go




ALTER TABLE Right
	ADD CONSTRAINT fk_Right_RoleID FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
go




ALTER TABLE Right
	ADD CONSTRAINT fk_Right_ItfID FOREIGN KEY (ItfID) REFERENCES Interface(ItfID)
go




ALTER TABLE User
	ADD CONSTRAINT fk_User_RoleID FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
go


