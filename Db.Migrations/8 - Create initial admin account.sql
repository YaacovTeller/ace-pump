declare @adminId int;

IF (NOT EXISTS (SELECT * FROM tblUsers WHERE Username='admin'))
BEGIN
	insert into tblUsers (Username, HashedPassword, PasswordSalt, IsApproved, PasswordFailuresSinceLastSuccess, LastPasswordFailureDate, LastActivityDate, LastLockoutDate, LastLoginDate, CreateDate, IsLockedOut, LastPasswordChangedDate)
	values ('admin', 'lur2kM2bL39dcmPOINXiXZkWzho=', '6xux1Y4F9P32+BOQrVcAzA==', 1, 0, GETDATE(), GETDATE(), GETDATE(), GETDATE(), GETDATE(), 0, GETDATE());
	
	select @adminId = SCOPE_IDENTITY();

	insert into tblUserRoles (UserID, RoleID)
	values
		(@adminId, 1),	-- account manager
		(@adminId, 2),	-- impersonator
		(@adminId, 3),	-- report viewer
		(@adminId, 4),	-- employee
		(@adminId, 6),	-- type manager
		(@adminId, 7),	-- see cost
		(@adminId, 8),	-- sys admin
		(@adminId, 10)	-- ticket signer
END