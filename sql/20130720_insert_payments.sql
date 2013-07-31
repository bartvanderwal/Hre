-- Undo queries.
-- drop table payment;
-- update sportseventparticipation set ParticipationAmountPaidInEuroCents=null where ParticipationAmountPaidInEuroCents is not null and sportseventId=2;
-- End of undo queries.

-- Analysis queries.
-- select * from sportseventparticipation where sportseventId=2 and ParticipationAmountInEuroCents is null;
-- select UserId, max(UserEmailAddress), sum(amountPayedInEuroCents) from payment group by UserId;
-- GRANT INSERT ON vanderwahre TO 'VanderwaVanderwa';
-- End of analysis queries.

create table Payment (
    Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    UserId int,
    UserEmailAddress varchar(127),
    AmountPayedInEuroCents int NULL,
    DateCreated timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DateUpdated timestamp -- NOT NULL ON UPDATE CURRENT_TIMESTAMP -- (only works in MySQL v5.6.5+)
);

-- This table was converted to insert script.
LOAD DATA LOCAL INFILE 'C:\\Users\\Bart\\Desktop\\BetalingenH2REJuli2013.csv' 
	INTO TABLE hre.payment 
	FIELDS TERMINATED BY ';' 
	-- ENCLOSED BY '\''
	LINES TERMINATED BY '\r\n' (UserEmailAddress, AmountPayedInEuroCents);

-- Get user Id from user e-mail address.
update payment p set userId=(
        select Id from LogonUser u where u.EmailAddress=p.UserEmailAddress
    )  where userId is null

update payment 
	set userId=(select Id from LogonUser where LogonUser.EmailAddress=Payment.UserEmailAddress)

-- U
update sportseventparticipation s
	set ParticipationAmountPaidInEuroCents=
		(select sum(AmountPayedInEuroCents) 
			from Payment p 
			where UserId is not null and UserId <> '' and s.UserId=p.UserId
						group by UserId)
	where s.SportsEventId=2
