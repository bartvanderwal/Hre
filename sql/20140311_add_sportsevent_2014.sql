-- Bart van der Wal - 11-03-2014:
-- Add H3RE.

-- ALTER TABLE tablename AUTO_INCREMENT = 3

insert into SportsEvent(Name, EventDate, EventPlace, DateCreated, DateUpdated, ExternalEventIdentifier, ExternalEventSerieIdentifier) values
 ('H3RE 2014', cast('2014-08-02 15:00:00' as datetime), 'Vinkeveen - Zandeiland 1', current_timestamp, current_timestamp, 2006240, 6941)


alter table SportsEventParticipation add BankCode varchar(3);
