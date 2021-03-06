CREATE TABLE $(QualifiedTableName) (
	ChangeId VARCHAR(64) NOT NULL,
	Folder VARCHAR(20) NOT NULL,
	ScriptNumber SMALLINT NOT NULL,
	ScriptName VARCHAR(256) NOT NULL,
	StartDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	AppliedBy VARCHAR(64) NOT NULL,
	ScriptStatus TINYINT NOT NULL,
	ScriptOutput TEXT NULL,
	PRIMARY KEY (ChangeId),
	UNIQUE (Folder, ScriptNumber));