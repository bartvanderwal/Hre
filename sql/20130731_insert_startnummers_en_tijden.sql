-- Bart van der Wal - 31-7-2013

-- Undo queries.
-- drop table RaceSlot;
-- update SportsEventParticipation set Racenumber = null, PlannedStartTime = null where SportsEventId=2;
-- End of undo queries.

-- Analysis queries.
-- select * from SportsEventParticipation where SportsEventId=2 and RaceNumber is null;
-- select * from RaceSlot;
-- select r.EmailAddress, count(r.Id) from RaceSlot r join LogonUser u on r.EmailAddress= u.EmailAddress group by r.EmailAddress having count(r.id)>1;
-- End of analysis queries.

create table RaceSlot (
    Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    UserId int,
    RaceNumber int,
    PlannedStartTime Time,
    FullName varchar(100),
    EmailAddress varchar(64)
);

-- Load the .csv file into the database.
-- This table was exported to an insert script from MySql Workbench because we cannot read in files on production.
-- So on production Instead of reading in the file with this command, instead we execute the 'insert' script created locally.
LOAD DATA LOCAL INFILE 'C:\\Users\\Bart\\Desktop\\HRE-Master-31-7-2013-1112.csv' 
	INTO TABLE RaceSlot
	FIELDS TERMINATED BY ';' 
	-- ENCLOSED BY '\''
	LINES TERMINATED BY '\r\n' (RaceNumber,PlannedStartTime,FullName,EmailAddress);

-- Get user Id from user e-mail address.
update RaceSlot r set UserId=(
        select Id from LogonUser u where u.EmailAddress=r.EmailAddress
    )  where userId is null;

-- Update de racenumbers in de SportsEventParticipation table met de gegevens uit de RaceNumber tabel.
update SportsEventParticipation s
	set RaceNumber=
		(select RaceNumber from RaceSlot r 
			where s.UserId=r.UserId)
	where s.SportsEventId=2;

-- En update ook de toegewezen starttijden in de SportsEventParticipation tabel.
update SportsEventParticipation s
	set PlannedStartTime =
		(select PlannedStartTime from RaceSlot r 
			where s.UserId=r.UserId)
	where s.SportsEventId=2;
