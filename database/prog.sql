CREATE PROC dbo.ddef_generate_usertoken @result VARCHAR(100) OUTPUT AS BEGIN
	DECLARE @token VARBINARY(500);
	DECLARE @base64token VARCHAR(100);
	SET @token = CRYPT_GEN_RANDOM(100);
	SET @base64token = CAST(N'' AS XML).value('xs:base64Binary(sql:variable("@Token"))', 'VARCHAR(MAX)');
	SET @base64token = REPLACE(REPLACE(@base64token, '+', 'A'), '/', 'B');
	SET @result = LEFT(@base64token, 100)
END;

CREATE PROC dbo.ddef_register
(
	@p_f_name NVARCHAR(30),
	@p_l_name NVARCHAR(30),
	@p_username NVARCHAR(30),
	@p_password NVARCHAR(30)  
)
AS BEGIN
	DECLARE @token VARCHAR(100);
	DECLARE @password NVARCHAR(32);
	EXEC dbo.ddef_generate_usertoken @token OUTPUT;
	IF EXISTS(SELECT * FROM dbo.ddef_user WHERE [username] = @p_username) BEGIN SELECT 'User already exists' as [message] RETURN END;
	SET @password = HASHBYTES('SHA2_256', @p_password);
	INSERT INTO dbo.ddef_user VALUES (@token, @p_username, @password, @p_f_name, @p_l_name);
	DECLARE @inserted_id BIGINT = SCOPE_IDENTITY();
	INSERT INTO dbo.ddef_user_plan VALUES (@inserted_id, 0, DATEADD(MONTH, 1, GETDATE()));
	SELECT [id] AS [user_id], [plan_id], [token], [username], [f_name], [l_name] FROM dbo.ddef_user 
		INNER JOIN dbo.ddef_user_plan ON [id] = [user_id]
	WHERE [id] = @inserted_id
END;

CREATE PROC dbo.ddef_login 
(
	@p_username NVARCHAR(30),
	@p_password NVARCHAR(30) 
)
AS BEGIN
	DECLARE @uid BIGINT;
	SELECT @uid = id FROM dbo.ddef_user WHERE [username] = @p_username
	IF @uid IS NULL BEGIN SELECT 'Invalid password or login' AS [message] RETURN END;
	SELECT [id] AS [user_id], [plan_id], [token], [username], [f_name], [l_name] FROM dbo.ddef_user 
		INNER JOIN dbo.ddef_user_plan ON [id] = [user_id]
	WHERE [id] = @uid
END;

CREATE PROC dbo.ddef_drop_token
(
	@user_id BIGINT,
	@token VARCHAR(100)
)
AS BEGIN
	DECLARE @new_token VARCHAR(100);
	DECLARE @user_exists BIT;
	EXEC dbo.ddef_generate_usertoken @new_token OUTPUT;
	EXEC dbo.ddef_confirm_identity @user_id, @token, @user_exists OUTPUT;
	IF (@user_exists = 0) BEGIN SELECT 'Token renewing faulure' AS [message] RETURN END;
	UPDATE dbo.ddef_user SET token = @new_token WHERE [id] = @user_id
	SELECT [id] AS [user_id], [plan_id], [token], [username], [f_name], [l_name] FROM dbo.ddef_user 
		INNER JOIN dbo.ddef_user_plan ON [id] = [user_id]
	WHERE [id] = @user_id
END;

CREATE PROC dbo.ddef_confirm_identity
(
	@user_id BIGINT,
	@token VARCHAR(100),
	@confirmed BIT OUTPUT
)
AS BEGIN
	IF EXISTS (SELECT * FROM dbo.ddef_user WHERE [id] = @user_id AND [token] = @token) SET @confirmed = 1;
    ELSE SET @confirmed = 0;
END;

CREATE PROC dbo.ddef_report
(
	@user_id BIGINT,
	@token VARCHAR(100),
	@packets INT,
	@addresses INT,
	@apps INT
)
AS BEGIN
	DECLARE @user_exists BIT;
	EXEC dbo.ddef_confirm_identity @user_id, @token, @user_exists OUTPUT;
	IF (@user_exists = 0) BEGIN SELECT 'Unable to identify user' AS [message] RETURN END;
	INSERT INTO dbo.ddef_user_report VALUES (@user_id, DEFAULT, @packets, @addresses, @apps)
	SELECT 'Success' AS [message]
END;

CREATE PROC dbo.ddef_get_bad_applications
(
	@user_id BIGINT,
	@token VARCHAR(100)
)
AS BEGIN
	DECLARE @user_exists BIT;
	DECLARE @plan_id TINYINT;
	DECLARE @expiration_date DATE;
	EXEC dbo.ddef_confirm_identity @user_id, @token, @user_exists OUTPUT;
	IF (@user_exists = 0) BEGIN SELECT 'Unable to identify user' AS [message] RETURN END;
	SELECT @plan_id = [plan_id], @expiration_date = [expiration_date] FROM dbo.ddef_user_plan WHERE [user_id] = @user_id
	IF (@plan_id = 0) BEGIN SELECT 'Plan is not supported' AS [message] RETURN END;
	IF (@expiration_date < GETDATE()) BEGIN SELECT 'Plan expired' AS [message] RETURN END;
	SELECT * FROM dbo.ddef_bad_application
END;

CREATE PROC dbo.ddef_get_bad_addresses
(
	@user_id BIGINT,
	@token VARCHAR(100)
)
AS BEGIN
	DECLARE @user_exists BIT;
	DECLARE @plan_id TINYINT;
	DECLARE @expiration_date DATE;
	EXEC dbo.ddef_confirm_identity @user_id, @token, @user_exists OUTPUT;
	IF (@user_exists = 0) BEGIN SELECT 'Unable to identify user' AS [message] RETURN END;
	SELECT @plan_id = [plan_id], @expiration_date = [expiration_date] FROM dbo.ddef_user_plan WHERE [user_id] = @user_id
	IF (@plan_id = 0) BEGIN SELECT 'Plan is not supported' AS [message] RETURN END;
	IF (@expiration_date < GETDATE()) BEGIN SELECT 'Plan expired' AS [message] RETURN END;
	SELECT * FROM dbo.ddef_bad_address
END;

CREATE PROC dbo.ddef_modify_user
(
	@user_id BIGINT,
	@token VARCHAR(100),
	@p_f_name NVARCHAR(30) NULL,
	@p_l_name NVARCHAR(30) NULL,
	@p_password NVARCHAR(30) NULL 
)
AS BEGIN
	DECLARE @user_exists BIT;
	EXEC dbo.ddef_confirm_identity @user_id, @token, @user_exists OUTPUT;
	IF (@user_exists = 0) BEGIN SELECT 'Unable to identify user' AS [message] RETURN END;
	IF (@p_f_name IS NOT NULL) UPDATE dbo.ddef_user SET [f_name] = @p_f_name WHERE [id] = @user_id
	IF (@p_l_name IS NOT NULL) UPDATE dbo.ddef_user SET [l_name] = @p_l_name WHERE [id] = @user_id
	IF (@p_password IS NOT NULL) UPDATE dbo.ddef_user SET [password] = HASHBYTES('SHA2_256', @p_password) WHERE [id] = @user_id
	SELECT [username], [f_name], [l_name] FROM dbo.ddef_user WHERE [id] = @user_id
END;

CREATE PROC dbo.ddef_modify_user_plan
(
	@user_id BIGINT,
	@token VARCHAR(100),
	@plan_id TINYINT
)
AS BEGIN
	DECLARE @user_exists BIT;
	EXEC dbo.ddef_confirm_identity @user_id, @token, @user_exists OUTPUT;
	IF (@user_exists = 0) BEGIN SELECT 'Unable to identify user' AS [message] RETURN END;
	UPDATE dbo.ddef_user_plan SET [plan_id] = @plan_id, [expiration_date] = DATEADD(MONTH, 1, GETDATE()) WHERE [user_id] = @user_id
END;

EXEC dbo.ddef_login 'johnsmith1388', '12345';
EXEC dbo.ddef_modify_user 3, 'dWDHshbat4LfUiDfee0BquEIlPQuh4AMDQO0vaRbRFb9Hjv1RWwr8vq6qwW1wqznWpG5k9AyzefLEXo1ISBvqjxnV2cDPEkpK1yz', NULL, NULL, NULL;
EXEC dbo.ddef_register 'John', 'Smith', 'testuser', '12345';
EXEC dbo.ddef_drop_token 3, 'JMLRIg96ihT79kfbraPqKwgOaxpd8QXkHW6g3zBYxhibHcBbO8wjQmryTgELXrxCXYqB5XBcLENo7xBLoFnPDaHZSrnbTjw3yNbS'
EXEC dbo.ddef_report 3, 'PyfHDusuDYb6zzlrlZf6kR43r4NaoBHX4rnUD5c4XQcS0il6BwJB9vBfDRgrRvoIrhocWGsArqzpUyEKtGTeo8BGbkukqDubGgWF', 0, 0, 0
EXEC dbo.ddef_get_bad_applications 3, 'kRrliQqs2SHgdX0LGLiUnMLCbI3MtSDjgIqpRN7J4K7qvn8t7PHBX243KkTVFcYeY4SCYgAx3BPrAQ4K3EkBhV5bfFkTBAleDcVO'
EXEC dbo.ddef_get_bad_addresses 3, 'JMLRIg96ihT79kfbraPqKwgOaxpd8QXkHW6g3zBYxhibHcBbO8wjQmryTgELXrxCXYqB5XBcLENo7xBLoFnPDaHZSrnbTjw3yNbS'
