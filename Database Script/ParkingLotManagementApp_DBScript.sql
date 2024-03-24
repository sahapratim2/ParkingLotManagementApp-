
CREATE TABLE ParkingRecord(
	Id bigint IDENTITY(1,1) NOT NULL,
	TagNumber varchar(50) NOT NULL,
	CheckInTime datetime NOT NULL,
	CheckOutTime datetime NULL,
	HourlyFee decimal(18, 2) NOT NULL,
	TotalAmount decimal(18, 2) NOT NULL,
	AddedBy varchar(50) NOT NULL,
	AddedDate datetime NOT NULL,
	AddedIP varchar(200) NOT NULL,
	UpdatedBy varchar(50) NULL,
	UpdatedDate datetime NULL,
	UpdatedIP varchar(200) NULL,
	Deleted nchar(10) NOT NULL,
	DeletedBy varchar(50) NULL,
	DeletedDate datetime NULL,
	DeletedIP varchar(200) NULL,
CONSTRAINT PK_ParkingRecord_Id PRIMARY KEY CLUSTERED (Id))
GO

ALTER TABLE ParkingRecord ADD  CONSTRAINT DF_ParkingRecord_HourlyFee  DEFAULT ((0)) FOR HourlyFee
GO

ALTER TABLE ParkingRecord ADD  CONSTRAINT DF_ParkingRecord_TotalAmount  DEFAULT ((0)) FOR TotalAmount
GO

ALTER TABLE ParkingRecord ADD  CONSTRAINT DF_ParkingRecord_Deleted  DEFAULT ((0)) FOR Deleted
GO

----------------------------------------------------------------------
Create or Alter Procedure GetCurrentParking
As
--Exec GetCurrentParking
Begin
	Select Id,TagNumber,CheckInTime, HourlyFee
	from ParkingRecord 
	Where CheckOutTime is  NULL
End
GO

----------------------------------------------------------------------
Create or Alter Procedure GetCurrentParkingByTagNumber
@TagNumber varchar(50)
As
--Exec GetCurrentParkingByTagNumber A7
Begin
	Select Id,TagNumber,CheckInTime,HourlyFee
	from ParkingRecord 
	Where TagNumber=@TagNumber and  CheckOutTime is  NULL
	
End
GO
----------------------------------------------------------------------

Create or Alter Procedure GetParkingRecord
As
--Exec GetCurrentParkingByTagNumber
Begin
	Select * from ParkingRecord
End
GO

----------------------------------------------------------------------

Create or Alter Procedure InsertParkingRecord
@TagNumber  varchar(50),
@CheckInTime datetime,
@HourlyFee decimal(10,2),
@AddedBy varchar(50),
@AddedDate datetime,
@AddedIP varchar(200)

As
Begin

	Insert Into ParkingRecord(TagNumber,CheckInTime,HourlyFee,AddedBy,AddedDate,AddedIP) 
	Values (@TagNumber,@CheckInTime,@HourlyFee,@AddedBy,@AddedDate,@AddedIP)

End
GO
----------------------------------------------------------------------

Create or Alter Procedure UpdateParkingRecord
@TagNumber  varchar(50),
@CheckOutTime datetime,
@TotalAmount decimal(10,2),
@UpdatedBy varchar(50),
@UpdatedDate datetime,
@UpdatedIP varchar(200)

As
Begin

	Update ParkingRecord Set CheckOutTime=@CheckOutTime,TotalAmount=@TotalAmount,
	UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate,UpdatedIP=@UpdatedIP
	Where TagNumber=@TagNumber and   CheckOutTime is  NULL

End
GO
----------------------------------------------------------------------

Create or Alter Procedure GetParkingStats
@TotalSpots int=0
As
--Exec GetParkingStats 15
Begin
	Select 1 as Id, 'Number of spots available as of now: ' As StatsText, Cast(@TotalSpots-count(*)As varchar(20)) As StatsValue
	from ParkingRecord 
	Where CheckOutTime is  NULL

	Union 
	Select 2 as Id,'Today’s revenue as of now: ' As StatsText, '$'+Cast(SUM(TotalAmount) As varchar(20)) As StatsValue 
	From ParkingRecord Where Cast(CheckOutTime as date)= Cast(GETDATE() as date)

	Union
	Select 3 as Id, 'Average number of cars per day (past 30 days): ' As StatsText, Cast(AVG(C.CarsPerDay)  As varchar(20)) As StatsValue
	From(Select CAST(CheckInTime as date) AS CheckInDate, COUNT(*) AS CarsPerDay 
		 From ParkingRecord Where CheckInTime >= DATEADD(day, -29, GETDATE())
		 Group By CAST(CheckInTime AS date)
		) C

	Union
	Select 4 as Id, 'Average revenue per day (past 30 days): ' As StatsText, '$'+Cast(Cast(ROUND(AVG(C.DailyRevenue), 2) as decimal(18,2))  As varchar(20)) As StatsValue
	From(Select CAST(CheckInTime as date) AS CheckInDate, SUM(TotalAmount) As DailyRevenue
		 From ParkingRecord Where CheckInTime >= DATEADD(day, -29, GETDATE())
		 Group By CAST(CheckInTime as date)
		) C
End
GO

