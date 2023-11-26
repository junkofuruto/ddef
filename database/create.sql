CREATE TABLE dbo.ddef_plan
(
	[id]				TINYINT NOT NULL,
	[title]				NVARCHAR(65) NOT NULL,
	[description]		NVARCHAR(500) NOT NULL,
	[dashboard_access]	BIT NOT NULL DEFAULT 0,
	[console_access]	BIT NOT NULL DEFAULT 0,

	PRIMARY KEY([id])
)
	
CREATE TABLE dbo.ddef_user
(
	[id]				BIGINT NOT NULL IDENTITY(0, 1),
	[token]				VARCHAR(100) NOT NULL UNIQUE,
	[username]			NVARCHAR(30) NOT NULL UNIQUE,
	[password]			NVARCHAR(32) NOT NULL,
	[f_name]			NVARCHAR(30) NOT NULL,
	[l_name]			NVARCHAR(30) NOT NULL,
		
	PRIMARY KEY ([id])
)

CREATE TABLE dbo.ddef_user_plan
(
	[user_id]			BIGINT NOT NULL,
	[plan_id]			TINYINT NOT NULL,
	[expiration_date]	DATE NOT NULL,

	PRIMARY KEY ([user_id]),
	FOREIGN KEY ([user_id]) REFERENCES dbo.ddef_user ([id]),
	FOREIGN KEY ([plan_id]) REFERENCES dbo.ddef_plan ([id])
)

CREATE TABLE dbo.ddef_user_report
(
	[id]				BIGINT NOT NULL IDENTITY(0, 1),
	[user_id]			BIGINT NOT NULL,
	[timestamp]			DATETIME NOT NULL DEFAULT GETDATE(),
	[packets]			INT NOT NULL,
	[addresses]			INT NOT NULL,
	[apps]				INT NOT NULL,

	PRIMARY KEY ([id]),
	FOREIGN KEY ([user_id]) REFERENCES dbo.ddef_user ([id]),
)

CREATE TABLE dbo.ddef_bad_application
(
	[id]				BIGINT NOT NULL IDENTITY(0, 1),
	[name]				NVARCHAR(30) NOT NULL UNIQUE,
	[reason]			NVARCHAR(65) NOT NULL,
	[message]			NVARCHAR(500) NOT NULL,

	PRIMARY KEY ([id])
)

CREATE TABLE dbo.ddef_bad_address
(
	[id]				BIGINT NOT NULL IDENTITY(0, 1),
	[host]				NVARCHAR(30) NOT NULL UNIQUE,
	[reason]			NVARCHAR(65) NOT NULL,
	[message]			NVARCHAR(500) NOT NULL,

	PRIMARY KEY ([id])
)