-- Bart van der Wal - 11-03-2014:
-- Add H3RE.

-- Undo queries
-- delete from SportsEvent where Name='H3RE 2014';
-- delete from SportsEvent where Name='H4RE 2015';
-- alter table SportsEventParticipation drop BankCode;
-- alter table SportsEventParticipation drop SisowTransactionID;
alter table SportsEvent drop column AantalStartPlekken, AantalPlekkenReserveLijst, AantalEarlyBirdStartPlekken,
    EindDatumEarlyBirdKorting, OpeningsdatumAlgemeneInschrijving, SluitingsDatumAlgemeneInschrijving, HuidigeDeelnameBedrag int,
    KostenNtbDagLicentie, KostenHuurMyLapsChipGeel, KostenGebruikMyLapsChipGroen, KostenEten, HoogteEarlyBirdKorting;

-- End of undo queries

-- ALTER TABLE SportsEvent AUTO_INCREMENT = 3

insert into SportsEvent(Name, EventDate, EventPlace, DateCreated, DateUpdated, ExternalEventIdentifier, ExternalEventSerieIdentifier) values
 ('H3RE 2014', cast('2014-08-02 15:00:00' as datetime), 'Vinkeveen - Zandeiland 1', current_timestamp, current_timestamp, 2006240, 6941)

alter table SportsEventParticipation add BankCode varchar(3);

alter table SportsEventParticipation add SisowTransactionID varchar(32);

alter table SportsEvent 
    add AantalStartPlekken int,
    add AantalPlekkenReserveLijst int,
    add AantalEarlyBirdStartPlekken int,
    add EindDatumEarlyBirdKorting DateTime,
    add OpeningsdatumAlgemeneInschrijving DateTime,
    add SluitingsDatumAlgemeneInschrijving DateTime,
    add HuidigeDeelnameBedrag int,
    add KostenNtbDagLicentie int,
    add KostenHuurMyLapsChipGeel int,
    add KostenGebruikMyLapsChipGroen int,
    add KostenEten int,
    add HoogteEarlyBirdKorting int;

UPDATE sportsevent SET AantalStartPlekken=360, AantalPlekkenReserveLijst=80, AantalEarlyBirdStartPlekken=0, EindDatumEarlyBirdKorting='2012-03-01', OpeningsdatumAlgemeneInschrijving='2012-03-01', SluitingsDatumAlgemeneInschrijving='2012-08-01', HuidigeDeelnameBedrag=2500, KostenNtbDagLicentie=220, KostenHuurMyLapsChipGeel=200, KostenGebruikMyLapsChipGroen=0, KostenEten=600, HoogteEarlyBirdKorting=0 WHERE Id=1;
UPDATE sportsevent SET AantalStartPlekken=560, AantalPlekkenReserveLijst=100, AantalEarlyBirdStartPlekken=200, EindDatumEarlyBirdKorting='2013-03-01', OpeningsdatumAlgemeneInschrijving='2013-03-01', SluitingsDatumAlgemeneInschrijving='2013-08-01', HuidigeDeelnameBedrag=2750, KostenNtbDagLicentie=220, KostenHuurMyLapsChipGeel=200, KostenGebruikMyLapsChipGroen=0, KostenEten=1000, HoogteEarlyBirdKorting=500 WHERE Id=2;
UPDATE sportsevent SET AantalStartPlekken=700, AantalPlekkenReserveLijst=100, AantalEarlyBirdStartPlekken=200, EindDatumEarlyBirdKorting='2014-05-01', OpeningsdatumAlgemeneInschrijving='2014-04-05', SluitingsDatumAlgemeneInschrijving='2014-07-27', HuidigeDeelnameBedrag=2500, KostenNtbDagLicentie=300, KostenHuurMyLapsChipGeel=200, KostenGebruikMyLapsChipGroen=150, KostenEten=1000, HoogteEarlyBirdKorting=500 WHERE Id=3;

insert into SportsEvent(`Name`, EventDate, EventPlace, DateCreated, DateUpdated, ExternalEventIdentifier, ExternalEventSerieIdentifier, 
    AantalStartPlekken, AantalPlekkenReserveLijst, AantalEarlyBirdStartPlekken, EindDatumEarlyBirdKorting, OpeningsdatumAlgemeneInschrijving,
    SluitingsDatumAlgemeneInschrijving, HuidigeDeelnameBedrag, KostenNtbDagLicentie, KostenHuurMyLapsChipGeel, KostenGebruikMyLapsChipGroen, KostenEten, HoogteEarlyBirdKorting) 
    values
    ('H4RE 2015', cast('2015-08-01 15:00:00' as datetime), 'Vinkeveen - Zandeiland 1', current_timestamp, current_timestamp, null, null,
    800, 100, 200, cast('2015-03-01 0:00:00' as datetime),  cast('2015-03-01 0:00:00' as datetime), 
    cast('2015-07-27)' as datetime), 2500, 300, 200, 150, 1000, 500); 

alter table SportsEvent 
    modify AantalStartPlekken int not null,
    modify AantalPlekkenReserveLijst int not null,
    modify AantalEarlyBirdStartPlekken int not null,
    modify EindDatumEarlyBirdKorting DateTime not null,
    modify OpeningsdatumAlgemeneInschrijving DateTime not null,
    modify SluitingsDatumAlgemeneInschrijving DateTime not null,
    modify HuidigeDeelnameBedrag int not null,
    modify KostenNtbDagLicentie int not null,
    modify KostenHuurMyLapsChipGeel int not null,
    modify KostenGebruikMyLapsChipGroen int not null,
    modify KostenEten int not null,
    modify HoogteEarlyBirdKorting int not null;

-- For on test.UPDATE sportsevent SET OpeningsdatumAlgemeneInschrijving='2014-04-03 00:00:00' WHERE Id=3;
