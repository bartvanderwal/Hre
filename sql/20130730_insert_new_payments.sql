-- Undo queries.
-- update sportseventparticipation set ParticipationAmountPaidInEuroCents=null where ParticipationAmountPaidInEuroCents is not null and sportseventId=2;
-- End of undo queries.

-- Analysis queries.
-- select * from sportseventparticipation where sportseventId=2 and ParticipationAmountInEuroCents is null;
-- select UserId, max(UserEmailAddress), sum(amountPayedInEuroCents) from payment group by UserId;
-- End of analysis queries.

CREATE TABLE payment_backup_20130730 LIKE payment;
INSERT payment_backup_20130730 SELECT * FROM payment;

DELETE from payment;

-- Load the .csv file into the database.
-- This table was exported to an insert script from MySql Workbench because we cannot read in files on production.
-- So on production Instead of reading in the file with this command, instead we execute the 'insert' script created locally.
LOAD DATA LOCAL INFILE 'C:\\Users\\Bart\\Desktop\\BetalingenH2RE30Juli2013.csv' 
	INTO TABLE hre.payment 
	FIELDS TERMINATED BY ';' 
	-- ENCLOSED BY '\''
	LINES TERMINATED BY '\r\n' (UserEmailAddress, AmountPayedInEuroCents);

-- 

update payment set DateUpdated=DateCreated;

-- Get user Id from user e-mail address.
update payment p set userId=(
        select Id from LogonUser u where u.EmailAddress=p.UserEmailAddress
    )  where userId is null;


update sportseventparticipation set ParticipationAmountPaidInEuroCents=null where ParticipationAmountPaidInEuroCents is not null and sportseventId=2;

-- Update the amounts in the participation table with the data in the payment table.
update sportseventparticipation s
	set ParticipationAmountPaidInEuroCents=
		(select sum(AmountPayedInEuroCents) 
			from Payment p 
			where UserId is not null and UserId <> '' and s.UserId=p.UserId)
	where s.SportsEventId=2
