
IF NOT EXISTS(SELECT * FROM UserCredentials U WHERE U.LoginID='superadmin')
BEGIN
 INSERT INTO UserCredentials(Id,FullName,LoginID,Password,UserTypeId,IsActive)
 values ('f4d24bd8-1052-4007-b164-000311af65b9','Super Admin','superadmin','E10ADC3949BA59ABBE56E057F20F883E',1,1)
END
Go

update a set a.CompanyAgentId=u.CompanyAgentId,a.CompanyId=u.CompanyId
 from Attendance a
inner join EmployeeUser u on a.UserId=u.UserId
go
--
alter table CompanyAgent add OfficeStartTime varchar(50) null
alter table CompanyAgent add OfficeEndTime varchar(50) null

ALTER TABLE EmployeeUser
ALTER COLUMN Designation varchar(30) NULL;

ALTER TABLE EmployeeUser
ALTER COLUMN IsAutoCheckPoint bit NULL;